# 8. Despesas domésticas

**Controller:** `HouseholdExpenseController` · **Service:** `HouseholdExpenseService` ·
**Telas:** `household-expense/HouseholdExpenses.jsx`, `edit-household-expense-modal`

| Ação | Endpoint |
|---|---|
| Listar por mês/ano | `GET /api/HouseholdExpense?month=&year=` |
| Obter uma | `GET /api/HouseholdExpense/{id}` |
| Tipos | `GET /api/HouseholdExpense/Types` |
| Criar | `POST /api/HouseholdExpense` |
| Atualizar | `PUT /api/HouseholdExpense` |
| Remover | `DELETE /api/HouseholdExpense/{id}` |

- Campos: **descrição**, **data**, **valor** (> 0), **tipo** (mesmo enum de
  [Parcelamentos §6.4](06-parcelamentos.md#64-tipos-de-despesa-expensetype)),
  **veículo** opcional e **cartão de crédito** opcional.
- Quando vinculada a um cartão, a despesa passa a compor a **fatura**: a *data de fatura*
  (`InvoiceDate`) é a própria data se o dia for menor que o dia de fechamento do cartão,
  senão o mês seguinte.
- A tela filtra por mês/ano, mostra **totais por categoria** + total geral, e agrupa as
  despesas **por dia**, com ícone da categoria e indicação do cartão utilizado.
- Validações: veículo/cartão informados precisam pertencer ao usuário.
