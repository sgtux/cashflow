# Cashflow

Aplicação de controle de **fluxo de caixa pessoal**. Registra ganhos, despesas,
parcelamentos, cartões de crédito, veículos e abastecimentos e, a partir disso, gera
uma **projeção financeira** dos próximos meses e o **saldo remanescente** de cada mês.

Documentação:
- [`docs/funcionalidades.md`](docs/funcionalidades.md) — documentação funcional detalhada
- [`docs/architecture-todo.md`](docs/architecture-todo.md) — backlog de melhorias estruturais
- [`docs/api-refactoring-todo.md`](docs/api-refactoring-todo.md) — refactorings pontuais da API

## Stack

| Parte | Tecnologia |
|---|---|
| **API** (`Api/`) | ASP.NET Core (.NET 10) · Controllers → Services → Repositories · Dapper + Slapper.AutoMapper · **SQL Server** |
| **SPA** (`Site/`) | React 18 · Redux · Material UI 6 · React Router (HashRouter) · Axios · Webpack (build com pnpm) |
| **Migrations** (`Migrations/`) | FluentMigrator — projeto console com os scripts DDL/DML versionados |
| **Testes** (`Tests/`) | MSTest — casos de integração ponta a ponta via `Microsoft.AspNetCore.Mvc.Testing` + seeders FluentMigrator |
| **Auth** | JWT Bearer (middleware próprio) · senhas com BCrypt · login com Google (OAuth) |
| **Cache** | `IMemoryCache` em memória, por usuário, para Home e Projeção |

O bundle da SPA é gerado em `Api/wwwroot` e servido pela própria API como arquivos
estáticos — em produção há um único processo.

## Estrutura do repositório

```
Api/          Backend ASP.NET Core
  Controllers/  Services/  Validators/  Infra/ (Entity, Repository, Sql, Filters)
Site/         Frontend React (src/scenes, src/components, src/services, src/store)
Migrations/   Schema + dados de referência (FluentMigrator)
Tests/        Suíte de testes (MSTest)
docs/         Documentação funcional e de arquitetura
```

Padrão de resposta da API: todo endpoint devolve um envelope
(`ResultModel` / `ResultDataModel<T>`) com `data`, `notifications` (erros de validação) e
`requestElapsedTime`. Validação → **400**; entidade não processável → **422**;
falha de autenticação → **401**. Datas do servidor no fuso **America/São_Paulo**.

## Rodando localmente

Pré-requisitos: **.NET SDK 10**, **Node 22**, **pnpm 11**, **Docker**.

### 1. Banco de dados

```bash
docker compose up -d db          # SQL Server em 172.31.31.10:1433 (sa / Mssql123)
```

### 2. Migrations

```bash
./Migrations/run-migrations.sh   # cria o database "cashflow" e aplica as migrations
```

### 3. API

```bash
dotnet run --project Api         # usa o profile "files" (http://localhost:56666)
```

O profile `files` em `Api/Properties/launchSettings.json` já injeta as variáveis de
ambiente para desenvolvimento local.

### 4. SPA

```bash
cd Site
pnpm install
pnpm start                       # webpack-dev-server; faz proxy de /api -> :56666
```

Para gerar o bundle de produção (servido pela API):

```bash
pnpm build:prod                  # saída em Api/wwwroot
```

## Variáveis de ambiente (API)

| Variável | Descrição |
|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Development` / `Production` |
| `DATABASE_CONNECTION_STRING` | connection string do SQL Server |
| `COOKIE_EXPIRES_IN_MINUTES` | validade do token JWT, em minutos |
| `SECRET_JWT_KEY` | chave de assinatura do JWT |
| `DATA_ENCRYPTION_KEY` | chave para o helper de criptografia |
| `GOOGLE_CLIENT_ID` | client id do OAuth do Google |
| `GOOGLE_OAUTH_ACCESS_TOKEN` / `GOOGLE_OAUTH_ID_TOKEN` | endpoints do Google usados na validação do login |

## Testes

```bash
dotnet test
```

Os testes sobem a API em memória e recriam o schema + dados via seeders do FluentMigrator
(`Tests/Mocks/Database/`), exercitando cada funcionalidade ponta a ponta.

## CI/CD

GitHub Actions (Node 22 + pnpm 11, .NET 10):

- **`build.yml`** — em qualquer branch exceto `main`: build da SPA + `dotnet test`.
- **`build-deploy.yml`** — em `main`: build, testes, execução das migrations e deploy
  no Azure Web App `cashfloweb` (Production).
