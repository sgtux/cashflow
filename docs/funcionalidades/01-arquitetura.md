# 1. Visão geral da arquitetura

| Camada | Tecnologia |
|---|---|
| **API** (`Api/`) | ASP.NET Core (.NET 10), Controllers + Services + Repositories, Dapper + Slapper.AutoMapper sobre **SQL Server** |
| **SPA** (`Site/`) | React 18, Redux, Material UI (MUI 6), React Router (HashRouter), Axios, Webpack |
| **Migrations** (`Migrations/`) | FluentMigrator (scripts DDL/DML versionados) |
| **Autenticação** | JWT Bearer + middleware próprio; senhas com BCrypt |
| **Cache** | `IMemoryCache` em memória, por usuário, para Home e Projeção |
| **Testes** (`Tests/`) | Suíte de casos por funcionalidade com mocks de repositório |

Padrão de resposta da API: todo endpoint devolve um envelope
(`ResultModel` / `ResultDataModel<T>`) com `data`, lista de `notifications` (erros de
validação) e `requestElapsedTime`. Erros de validação retornam **400** com a lista de
mensagens; entidade não processável retorna **422**; falha de autenticação retorna **401**.

As datas do servidor usam o fuso **America/São Paulo** (`DateTimeUtils.CurrentDate`).

## Navegação da SPA

| Rota | Tela | Menu lateral |
|---|---|---|
| `/` | Home | Home |
| `/payments` | Parcelamentos | Parcelamentos |
| `/projection` | Projeção | Projeção |
| `/credit-cards` | Cartões / Faturas | Créditos/Faturas |
| `/earnings` | Ganhos | Ganhos |
| `/household-expenses` | Despesas domésticas | Despesas |
| `/vehicles` | Veículos | Veículos |
| `/recurring-expenses` | Despesas recorrentes | Recorrentes |
| `/remaining-balance` | Saldo remanescente | Remanescente |
| `/account` | Conta | (avatar / toolbar) |
| `/edit-payment/:id` | Cadastro/edição de parcelamento | — |
| `/edit-recurring-expenses/:id` | Cadastro/edição de despesa recorrente | — |
