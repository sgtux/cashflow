# Architecture TODO — Cashflow

Melhorias **estruturais** (forma do sistema), levantadas em 2026-08-28 a partir de
leitura de API, SPA, testes e infra. Complementa o
[api-refactoring-todo.md](api-refactoring-todo.md) (nível de linha).

Ordenado por leverage: itens de "Alto impacto" mudam a estrutura e destravam os demais.

---

## Alto impacto

### 1. Introduzir camada de contrato (DTO) na API
- [ ] Criar `records` de request/response por endpoint; parar de fazer bind do body
      direto nas `*Entity` de persistência (`Controllers/HouseholdExpenseController.cs`
      e todos os outros) e de retornar entidades na resposta.
- [ ] Mapear DTO ⇄ Entity explicitamente (aproveitar `Utils/MapperUtils.cs`).
- **Por quê:** hoje o contrato HTTP está acoplado ao schema do banco; é a raiz dos
  riscos de mass-assignment (mitigados na munheca sobrescrevendo `Id`/`UserId` no
  service); regra de validação fica espalhada entre `Validators/*` e controller.
- **Destrava:** versionamento de API, OpenAPI, validação num lugar só.

### 2. Extrair lógica de domínio dos services (torná-la testável isoladamente)
- [ ] `Services/ProjectionService.cs` (254 linhas): separar **carga de dados** do
      **cálculo da projeção**. O cálculo deve ser função pura: recebe as coleções já
      carregadas, devolve `List<PaymentMonthProjectionModel>` — sem repo, sem cache.
- [ ] Limpar código morto em `ProjectionService.LoadDates` (`baseDate.AddMonths(1)`
      com retorno descartado; `baseDate == default` nunca é `true`).
- [ ] Mesmo tratamento para consolidação de fatura em `Services/CreditCardService.cs`
      e para geração de parcelas (`GenerateInstallments`).
- **Por quê:** hoje toda a suíte (`Tests/SuiteCases/*`) é integração ponta-a-ponta via
  `Tests/Mocks/WebApi.cs`. Não há teste unitário de `Validators/*` nem da projeção
  porque a lógica está entrelaçada com repositório.

### 3. Parar de instanciar service com `new` dentro de service
- [ ] `Services/CreditCardService.cs` cria `PaymentService`, `HouseholdExpenseService`
      e `RecurringExpenseService` na mão, apesar de estarem registrados no container
      (`Extensions/StartupExtensions.cs`). Injetar via construtor.
- **Por quê:** quebra DI, testabilidade e o compartilhamento do `IDatabaseContext`
  scoped (transação). É sintoma da ausência da camada de domínio do item 2.

### 4. Rever a estratégia de cache
- [ ] Não depender de `AppCache.Clear(userId)` chamado manualmente em cada método que
      escreve (`Shared/Cache/AppCache.cs`) — qualquer caminho novo de mutação que
      esquecer serve Home/Projeção obsoletos.
- [ ] Definir expiração absoluta em `HomeCache.Update` / `ProjectionCache.Update`
      (hoje `_memoryCache.Set` sem `MemoryCacheEntryOptions`) como safety net.
- [ ] Decidir sobre multi-instância: `IMemoryCache` não funciona com mais de um
      processo (há `Dockerfile` + `heroku-deploy.sh`). Avaliar `IDistributedCache`.
- [ ] Considerar um decorator sobre os services em vez de invalidação espalhada.

### 5. Consolidar autenticação num mecanismo só
- [ ] `Startup.cs` registra `AddAuthentication(JwtBearer)` mas nunca chama
      `.AddJwtBearer(...)`; a validação real está em `Auth/AuthenticationMiddleware.cs`
      (custom, `ValidateIssuer/Audience = false`, ordenado **antes** de
      `UseAuthentication()`).
- [ ] Migrar para `AddJwtBearer` do framework + `[Authorize]` e remover o middleware
      custom (que ainda faz `new LogService()` e loga o token cru).

### 6. Configuração tipada com validação no startup
- [ ] `Shared/AppConfig.cs` lê tudo via `Environment.GetEnvironmentVariable` no
      construtor; `Convert.ToInt32(null)` estoura `FormatException` genérico se faltar
      variável.
- [ ] Migrar para `IOptions<T>` + `ValidateOnStart`; usar o `appsettings.json` que
      hoje está praticamente sem uso.

### 7. Idempotência & concorrência no caminho de escrita
- **Por quê:** o produto é dado financeiro — registro duplicado ou update perdido
  corrompe a projeção e o saldo remanescente. Requisições lentas acontecem (o toast de
  `requestElapsedTime > 500ms` existe por isso), e retry de cliente em timeout / duplo
  clique / rede móvel viram lançamentos duplicados hoje.

- [ ] **`Idempotency-Key` nos POST que criam lançamento**: `POST /api/Payment`,
      `/api/Earning`, `/api/HouseholdExpense`, `/api/FuelExpense`,
      `/api/RecurringExpense/History`. Cliente gera um `uuid` na abertura do form e
      envia no header; tabela `IdempotencyKey(Key PK, UserId, Endpoint, ResponseJson,
      CreatedAt)` + filtro no `Controllers/BaseController.cs` que devolve a resposta
      salva se a chave repetir. TTL de 24–48h.
- [ ] **`PayCurrentInvoicePayment` (ramo recorrente) idempotente**: hoje a regra
      "1 registro por mês/ano" retorna **erro** na 2ª chamada
      (`Services/CreditCardService.cs`). Trocar por "se já existe, devolve o existente
      com 200" — o retry do cliente deixa de virar erro visível.
- [ ] **`DELETE` idempotente**: vários services devolvem `NotFound` como notificação
      de erro quando a linha já sumiu. Retornar 200/204 quando o alvo não existe mais.
- [ ] **Dedupe no frontend**: desabilitar o submit enquanto a request está em voo e/ou
      colapsar chamadas idênticas concorrentes em `src/services/httpService.js`. Barato,
      mata a maioria das duplicatas antes da API.
- [ ] **Concorrência otimista** (`ETag`/`If-Match` ou coluna `RowVersion`): os `PUT`
      são last-write-wins — duas abas editando o mesmo parcelamento, uma sobrescreve a
      outra em silêncio. Idempotency-Key protege contra reenvio da *mesma* escrita;
      versionamento protege contra escritas concorrentes *diferentes*. Casa com o item
      "Gerar PK no banco" (fim do `SELECT MAX(Id)`).
- **Já está certo (manter):** `RemainingBalance/Recalculate` só grava se não existir ou
      `force=true` — idempotente por design; invalidação de cache (`AppCache.Clear`)
      também é idempotente.
- **Fora de escopo por enquanto:** outbox pattern / mensageria / exactly-once só faz
  sentido se entrar processamento assíncrono (e-mail, recálculo agendado, webhook de
  pagamento). Antes disso, não.

---

## Mecânicos / menores (API)

- [ ] **`LogService` singleton, configurado uma vez.** Hoje é `AddScoped` e refaz
      `new AppConfig()` + `XmlConfigurator.Configure` a cada request
      (`Services/LogService.cs`). Alternativa: usar `ILogger<T>`.
- [ ] **SQL embedded via glob.** `Cashflow.Api.csproj` registra uma linha
      `<EmbeddedResource>` por arquivo `.sql` (fácil esquecer, sem checagem em
      compilação). Trocar por `<EmbeddedResource Include="Infra/Sql/**/*.sql" />`.
- [ ] **Padronizar o envelope de resposta.** `Controllers/BaseController.cs` devolve
      `ResultModel` no sucesso e objeto anônimo `{ Errors, RequestElapsedTime }` no
      erro — formatos diferentes pro cliente. Fazer via result filter/middleware em
      vez de cada action chamar `HandleResult`. Isso também remove a duplicação de
      medição de tempo entre `BaseController` e `Shared/ExceptionHandler.cs`.
- [ ] **Gerar PK no banco** (identity/sequence) em vez de `SELECT MAX(Id)` em
      `Infra/Repository/BaseRepository.cs` (race condition).
- [ ] **Minimal hosting.** `Program.cs`/`Startup.cs` usam o padrão `IHostBuilder` +
      classe `Startup` num TFM `net10.0`; migrar para `WebApplication.CreateBuilder`.
- [ ] **Adicionar OpenAPI/Swagger** (`Microsoft.AspNetCore.OpenApi`, já no SDK) para
      contrato versionável e geração de cliente para a SPA.
- [ ] **Unir configs de deploy.** `azure-pipelines.yml`, `heroku-deploy.sh`,
      `.github/`, `Dockerfile`, `docker-compose.yml` — provável drift; escolher uma.

---

## Frontend (SPA)

- [ ] **Adotar React Query / RTK Query** para dado de servidor. Hoje o Redux
      (`src/store/`) só guarda `user`/`menu`/`loader`/`theme`; todo fetch é ad-hoc nas
      scenes com flag de loading manual. Um cache no cliente reduz a pressão sobre o
      cache da API. Com Redux tão magro, dá para trocar por Context + React Query.
- [ ] **Instância dedicada de axios.** `src/services/httpService.js` usa o `axios`
      global (`axios.interceptors...`); `registerCallbackUnauthorized` é singleton
      mutável de módulo. Usar `axios.create()`.
- [ ] **Rever armazenamento do token.** `src/services/storage.service.js` guarda o JWT
      em `localStorage` — exfiltrável por XSS, e combinado com JWT de 7 dias sem
      revogação é exposição real. Avaliar httpOnly cookie + CSRF, ou access token
      curto + refresh.
- [ ] **Separar hooks de data-fetching de componentes de apresentação** nas
      `src/scenes/*` (decorre da adoção de React Query).

---

## Testes

- [ ] Após o item 2, adicionar testes **unitários** para: `Validators/*`, cálculo da
      projeção, consolidação de fatura (`CreditCardService.PayCurrentInvoicePayment`),
      geração de parcelas e cálculo de saldo remanescente.
- [ ] Manter os testes de integração atuais como camada de contrato, não como única
      forma de exercitar regra de negócio.
