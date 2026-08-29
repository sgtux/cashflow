# 12. Projeção financeira

**Controller:** `ProjectionController` · **Service:** `ProjectionService` ·
**Telas:** `projection/Projection.jsx`, `projection/PaymentMonth`

`GET /api/Projection` devolve uma **projeção mês a mês até dezembro do ano seguinte**
(~12+ meses a partir do mês atual). Antes de projetar, **recalcula o saldo remanescente
do mês anterior**. Resultado é **cacheado** por usuário.

Cada mês (`PaymentMonthProjectionModel`) agrega lançamentos (`PaymentProjectionModel`)
de todas as fontes:

| Fonte | Como entra na projeção |
|---|---|
| **Proventos** | mensais replicados em todos os meses; normais só no mês da data |
| **Parcelamentos** | parcelas não pagas/não isentas por mês (ou já pagas no mês do pagamento) |
| **Combustível** | soma real dos abastecimentos do mês; se não houver, usa o **limite de combustível** como valor **residual** |
| **Despesas domésticas** | lançamentos reais (com data de fatura quando em cartão); o restante do **limite de despesas** entra como **residual** |
| **Despesas recorrentes** | histórico do mês corrente quando existir; nos meses futuros, o valor base de cada recorrente ativa |
| **Saldo do mês anterior** | "Saldo Mês Anterior" como entrada (positivo) ou saída (negativo) |

Por mês são calculados **total de entradas**, **total de saídas**, **saldo do mês** e o
**valor acumulado** (somatório corrente de saldo + saldo do mês anterior). A tela lista
cada mês expansível com seus lançamentos e exibe o **Total Acumulado** ao final.
