# 5. Ganhos (Proventos)

**Controller:** `EarningController` · **Service:** `EarningService` ·
**Tela:** `earning/Earnings.jsx` + `EditEarningModal`

CRUD completo:

| Ação | Endpoint |
|---|---|
| Listar (a partir de uma data) | `GET /api/Earning?fromDate=` |
| Obter um | `GET /api/Earning/{id}` |
| Tipos disponíveis | `GET /api/Earning/Types` |
| Criar | `POST /api/Earning` |
| Atualizar | `PUT /api/Earning` |
| Remover | `DELETE /api/Earning/{id}` |

- Campos: **descrição**, **valor** (> 0), **data**, **tipo**.
- **Tipos de provento** (`EarningType`): **Mensal** (`Monthy`) e **Normal**. Proventos
  mensais são replicados mês a mês na projeção; os normais valem apenas no mês da data.
- A tela filtra por "Desde: mês/ano" e permite **copiar um provento para o mês atual**
  (botão de cópia aparece quando o provento não é do mês corrente).
- Validação: descrição obrigatória, data válida, valor > 0, tipo válido.
