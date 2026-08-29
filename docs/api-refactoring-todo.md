# API — Refactoring & melhorias (nível de linha)

Levantado em auditoria de código em 2026-08-01. Ordenado por impacto
(segurança > correção > manutenibilidade > testes > menor).

Caminhos são relativos à raiz do repositório. Doc relacionado:
[architecture-todo.md](architecture-todo.md) (melhorias estruturais).

## Segurança (crítico)

- [ ] **Remover segredos commitados no git** — `Api/Properties/launchSettings.json:25-31` tem senha do SA, JWT secret e chave de criptografia em texto plano. Migrar para user-secrets/vault e limpar do histórico do git.
- [ ] **Unificar validação de JWT** — `Api/Startup.cs:19` registra `AddAuthentication(JwtBearer)` mas nunca chama `.AddJwtBearer(...)`; a validação real acontece manualmente em `Api/Auth/AuthenticationMiddleware.cs:55-69` com `ValidateIssuer/Audience = false`. Consolidar em uma única abordagem (preferencialmente `AddJwtBearer` padrão).
- [ ] **Remover ou corrigir `CryptographyUtils`** — `Api/Utils/CryptographyUtils.cs:35` usa a mesma chave como IV (quebrado criptograficamente). Não é chamado em nenhum lugar do projeto: remover o código morto ou corrigir o tratamento de IV (aleatório por operação) antes de usar de verdade.

## Correção / risco de bug

- [ ] **Corrigir geração de PK com race condition** — `Api/Infra/Repository/BaseRepository.cs:97-102` usa `SELECT MAX(Id)` para gerar próximo ID; inserts concorrentes podem colidir.
- [ ] **Enrolar `Exists()`/`Count()` na transação ambiente** — `Api/Infra/Repository/BaseRepository.cs:83-95` não passam a `Transaction`, ao contrário de `Query`/`Execute`/`NextId`; pode lançar `InvalidOperationException` se chamado dentro de uma transação de escrita aberta.
- [ ] **Habilitar `<Nullable>enable</Nullable>`** nos três `.csproj` (Api, Tests, Migrations) e corrigir desreferências sem checagem, ex: `Api/Services/AccountService.cs:131-133`.

## Manutenibilidade / performance

- [ ] **Trocar `.Result` bloqueante por validação assíncrona** em todos os validators (`Api/Validators/CreditCardValidator.cs`, `UserValidator.cs`, `PaymentValidator.cs`, `HouseholdExpenseValidator.cs`, `RecurringExpenseValidator.cs`, `FuelExpenseValidator.cs`, `VehicleValidator.cs`, `EarningValidator.cs`, `RecurringExpenseHistoryValidator.cs`) — usar `MustAsync`/`ValidateAsync`. Considerar atualizar o FluentValidation (pinado em `10.1.0` em `Api/Cashflow.Api.csproj:18`) para uma versão atual com melhor suporte async.
- [ ] **Usar DI em vez de `new Service(...)` manual** — `Api/Services/CreditCardService.cs:40-42` instancia `PaymentService`, `HouseholdExpenseService` e `RecurringExpenseService` na mão, apesar de já estarem registrados no container em `Api/Extensions/StartupExtensions.cs:16-24`.
- [ ] **Corrigir `LogService` para não reconstruir config/reparsear XML a cada request** — `Api/Services/LogService.cs:15-19` faz `new AppConfig()` e `XmlConfigurator.Configure(...)` a cada instanciação (é `AddScoped`, roda por request). Injetar `IAppConfig` já registrado e configurar o log4net uma vez no startup.
- [ ] **Usar `IHttpClientFactory` no login Google** — `Api/Controllers/AccountController.cs:88` cria `new HttpClient()` por request, risco de esgotamento de sockets.
- [ ] **Migrar config de `Environment.GetEnvironmentVariable` cru para `IOptions<T>`** — `Api/Shared/AppConfig.cs:12-20`, com validação clara no startup em vez de `FormatException` genérico quando falta variável.
- [ ] **Unificar medição de tempo de request** — hoje há duas implementações independentes: `Api/Controllers/BaseController.cs:13-19,34` e `Api/Shared/ExceptionHandler.cs:17-22,48`.

## Testes

- [ ] **Adicionar testes unitários isolados** para lógica de validação e serviços — hoje `Tests/SuiteCases/*` só cobre integração ponta-a-ponta via `Tests/Mocks/WebApi.cs`. Faltam testes unitários para `Api/Validators/*` e lógica mais complexa como o switch de tipos em `Api/Services/CreditCardService.cs:178-190` (`PayCurrentInvoicePayment`).

## Menor

- [ ] **Trocar `Thread.Sleep(500)` por `await Task.Delay(500)` (ou remover)** — `Api/Shared/ExceptionHandler.cs:31-32`, bloqueia thread síncrona dentro de middleware async em dev.
- [ ] **Limpar log triplicado no exception handler** — `Api/Shared/ExceptionHandler.cs:40-43,50-51` loga simultaneamente via `Debug.WriteLine`, `Console.Write` e `logService.Error`.
