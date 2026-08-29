# 9. Despesas recorrentes

**Controller:** `RecurringExpenseController` · **Service:** `RecurringExpenseService` ·
**Telas:** `recurring-expenses/RecurringExpenses.jsx`, `EditRecurringExpense`,
`RecurringExpenseHistoryModal`

Gastos que se repetem todo mês (assinaturas, aluguel, etc.), com **valor base** e um
**histórico de pagamentos** mês a mês.

| Ação | Endpoint |
|---|---|
| Listar (`active` opcional) | `GET /api/RecurringExpense?active=` |
| Obter uma | `GET /api/RecurringExpense/{id}` |
| Criar | `POST /api/RecurringExpense` |
| Atualizar | `PUT /api/RecurringExpense/{id}` |
| Remover | `DELETE /api/RecurringExpense/{id}` |
| Adicionar pagamento ao histórico | `POST /api/RecurringExpense/History` |
| Editar pagamento do histórico | `PUT /api/RecurringExpense/History/{id}` |
| Remover pagamento do histórico | `DELETE /api/RecurringExpense/{recurringExpenseId}/History/{id}` |

- Campos: **descrição**, **valor base** (> 0), **cartão** opcional, e `InactiveAt` para
  **inativar** a despesa (deixa de contar dali em diante).
- Situação exibida por linha: **Inativo**, **Pago** (há histórico no mês atual) ou
  **Pendente**. O botão **"Pagar"** cria um item de histórico com o valor base na data
  atual.
- Histórico: valor pago + data; **não pode haver mais de um registro para o mesmo
  mês/ano**; valor pago deve ser > 0.
- **Remoção bloqueada** se já houver histórico registrado.
- A tela permite exibir/ocultar inativas e ver os 10 últimos pagamentos por despesa.
