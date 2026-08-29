# 13. Saldo remanescente

**Controller:** `RemainingBalanceController` · **Service:** `RemainingBalanceService` ·
**Tela:** `remaining-balance/RemainingBalance.jsx`

O **saldo remanescente** de um mês é quanto sobrou (ou faltou) depois de todas as
entradas e saídas daquele mês, **acumulando o saldo do mês anterior**.

| Ação | Endpoint |
|---|---|
| Listar todos os meses (+ simulação do mês atual) | `GET /api/RemainingBalance` |
| Ajustar manualmente o valor de um mês | `PUT /api/RemainingBalance` |
| Forçar recálculo de um mês | `PUT /api/RemainingBalance/Recalculate` |

## Cálculo (`Recalculate`)
`total = Σ proventos do mês − combustível do mês − despesas domésticas do mês
− parcelas pagas no mês − Σ despesas recorrentes do mês + saldo remanescente do mês anterior`

- Roda automaticamente para o **mês anterior** a cada login e a cada abertura da Projeção;
  só grava se ainda não existir registro (ou se `force = true`).
- O mês **corrente** é sempre devolvido como **simulação** (não é persistido).
- **Ajuste manual**: na tela é possível editar o valor de um mês, inclusive marcá-lo como
  **"Devedor"** (valor negativo).
- **Recalcular**: botão que refaz o cálculo de um mês específico sobrescrevendo o valor.
- A tela mostra os 5 meses mais recentes por padrão, com opção **"Exibir Todos"**.
