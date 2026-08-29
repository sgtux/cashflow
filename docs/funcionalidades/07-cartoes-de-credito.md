# 7. Cartões de crédito e faturas

**Controller:** `CreditCardController` · **Service:** `CreditCardService` ·
**Telas:** `credit-cards/CreditCards.jsx`, `CreditCardEditModal`

| Ação | Endpoint |
|---|---|
| Listar cartões (com itens da fatura) | `GET /api/CreditCard` |
| Criar | `POST /api/CreditCard` |
| Atualizar | `PUT /api/CreditCard` |
| **Pagar item da fatura atual** | `PUT /api/CreditCard/PayCurrentInvoicePayment` |
| Remover | `DELETE /api/CreditCard/{id}` |

- Campos do cartão: **nome**, **dia de fechamento da fatura** e **dia de vencimento**
  (ambos entre 1 e 30).
- A listagem monta, para cada cartão, os **itens da fatura** consolidando três origens:
  - **Parcelamentos** vinculados ao cartão (parcela do mês, saldo devedor, `pagas/total`);
  - **Despesas domésticas** vinculadas ao cartão cuja data de fatura cai no mês atual ou
    no próximo;
  - **Despesas recorrentes** vinculadas ao cartão.
- Para cada item mostra: valor pendente, valor total, valor da fatura do mês e se já
  está **pago**. O cartão agrega **"Fatura Atual"** e **"Crédito Devedor"** totais.
- **Pagar item da fatura**: na tela, o botão "pagar" de um item abre um diálogo de valor
  e registra o pagamento na data de vencimento do cartão:
  - item do tipo **Parcelamento** → marca a parcela do mês como paga;
  - item do tipo **Recorrente** → cria um registro de histórico de pagamento;
  - (item do tipo Despesa doméstica não é pago por aqui).
- **Remoção bloqueada** se o cartão estiver vinculado a algum parcelamento ou despesa.
