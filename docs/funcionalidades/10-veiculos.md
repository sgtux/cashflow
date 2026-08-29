# 10. Veículos

**Controller:** `VehicleController` · **Service:** `VehicleService` ·
**Telas:** `vehicles/Vehicles.jsx`, `VehicleModal/EditVehicleModal`

| Ação | Endpoint |
|---|---|
| Obter um | `GET /api/Vehicle/{id}` |
| Listar (`showInactives`) | `GET /api/Vehicle?showInactives=` |
| Criar | `POST /api/Vehicle` |
| Atualizar | `PUT /api/Vehicle/{id}` |
| Remover | `DELETE /api/Vehicle/{id}` |

- Campos: **descrição** (obrigatória, até 200 caracteres) e **ativo/inativo**.
- A partir dos abastecimentos, cada veículo calcula:
  - **Quilômetros percorridos** = maior hodômetro − menor hodômetro;
  - **Média km/l** = km percorridos ÷ (litros abastecidos, exceto o último abastecimento);
  - lista dos **10 abastecimentos mais recentes**.
- A listagem por padrão só mostra veículos ativos (e com movimentação nos últimos ~2
  meses); há checkbox **"Exibir Inativos"**.
- **Remoção bloqueada** se o veículo tiver abastecimentos cadastrados.
