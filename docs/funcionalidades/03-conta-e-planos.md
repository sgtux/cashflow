# 3. Conta e planos

**Controller:** `AccountController` · **Tela:** `account/Account.jsx`

- `GET /api/Account` retorna dados do usuário: e-mail, plano, limites e registros usados.
- `PUT /api/Account` atualiza os **limites mensais** do usuário:
  - **Limite para Despesas** (`ExpenseLimit`) — teto de despesas domésticas.
  - **Limite para Combustível** (`FuelExpenseLimit`) — teto de gastos com combustível.
  - Ambos aceitam valores entre **0 e 99.999**.
- Esses limites alimentam as barras de "Limites" na Home e os valores **residuais** da
  projeção (ver [Home](04-home.md) e [Projeção financeira](12-projecao-financeira.md)).
- **Planos** (somente informativos na tela; definem a cota de registros):
  | Plano | Custo exibido | Registros disponíveis |
  |---|---|---|
  | Free | R$ 0,00 | 10.000 |
  | Basic | R$ 5,99 | 100.000 |
  | Premium | R$ 19,99 | 1.000.000 |
- A tela mostra **"X registros utilizados de Y"**; a contagem (`RecordsUsed`) é
  recalculada no login e ao salvar a conta.
