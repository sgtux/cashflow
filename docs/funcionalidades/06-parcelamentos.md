# 6. Parcelamentos

**Controller:** `PaymentController` · **Service:** `PaymentService` ·
**Telas:** `payments/Payments.jsx`, `payments/EditPayment/*`, `payments/PaymentFilter`

Um **parcelamento** é uma compra dividida em **parcelas** (`InstallmentEntity`), com ou
sem vínculo a um cartão de crédito.

| Ação | Endpoint |
|---|---|
| Listar (com filtro) | `GET /api/Payment?description=&done=&startDate=&endDate=&creditCardIds=` |
| Obter um | `GET /api/Payment/{id}` |
| Tipos de despesa | `GET /api/Payment/Types` |
| **Gerar parcelas** (pré-visualização) | `GET /api/Payment/GenerateInstallments?value=&amount=&date=&creditCardId=` |
| Criar | `POST /api/Payment` |
| Atualizar | `PUT /api/Payment` |
| Remover | `DELETE /api/Payment/{id}` |

## 6.1 Geração automática de parcelas
`GenerateInstallments` recebe **valor total**, **quantidade** (1 a 72), **data da compra**
e cartão opcional, e devolve a lista de parcelas:
- o valor por parcela é o total dividido pela quantidade (arredondado para baixo);
- a **diferença de arredondamento é jogada na primeira parcela**;
- **se houver cartão**, a data da primeira parcela é ajustada para o **dia de
  vencimento** da fatura, considerando o dia de fechamento — compras após o fechamento
  caem para a fatura seguinte (1 ou 2 meses à frente, conforme fechamento x vencimento).

## 6.2 Parcelas
Cada parcela tem número, valor, data de vencimento, e opcionalmente **valor pago** +
**data de pagamento**, ou a marca **isenta** (`Exempt`) para parcelas que não serão
cobradas. Na tela de edição é possível ajustar parcelas individualmente
(`EditInstallmentModal`).

Regras de validação (`PaymentValidator`): descrição obrigatória; ao menos 1 parcela e no
máximo **72**; números de parcela únicos e entre 1 e 72; valor da parcela > 0; parcela
paga precisa de valor pago > 0 e data de pagamento; parcela isenta **não** pode ter valor
pago; cartão informado precisa pertencer ao usuário.

## 6.3 Listagem e filtro
A tela lista descrição, tipo, período (primeira → última parcela), `N x valor = total`,
cartão e progresso `parcelas pagas / total`. Marca **"Concluído"** quando todas as
parcelas foram pagas/isentas e **"pago"** quando a parcela do mês atual está quitada.
O filtro (`PaymentFilter`) permite buscar por descrição, período e por
**concluídos / não concluídos**.

## 6.4 Tipos de despesa (`ExpenseType`)
Alimentação, Mercado, Lanche, Estética, Educação, Veículo, Pets, Lazer, Bebida,
Festa, Transporte, Multas/Juros, Roupas, Contas de consumo, Limpeza, Câmbio,
Investimento, Telefone, Jogos, Outros, Doação, Streams, Empréstimo, Financiamento,
Tecnologia. (usados também nas despesas domésticas)
