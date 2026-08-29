# 11. Abastecimentos (despesas de combustível)

**Controller:** `FuelExpenseController` · **Service:** `FuelExpenseService` ·
**Tela:** `vehicles/VehicleModal/EditFuelExpensesModal.jsx`

| Ação | Endpoint |
|---|---|
| Criar | `POST /api/FuelExpense` |
| Atualizar | `PUT /api/FuelExpense/{id}` |
| Remover | `DELETE /api/FuelExpense/{id}` |

- Campos: **quilometragem/hodômetro**, **preço por litro**, **valor abastecido**,
  **data** e veículo. **Litros abastecidos** é calculado (valor ÷ preço por litro).
- Todos os valores numéricos devem ser > 0 e abaixo de 1.000.000.000.
- Validação de consistência **data × quilometragem**: rejeita um abastecimento cuja
  quilometragem conflita com outro do mesmo veículo (mesma quilometragem, ou datas e
  hodômetros em ordem invertida) — mensagem *"Data e Quilometragem não batem devido à
  outro abastecimento"*.
- O veículo precisa pertencer ao usuário.
