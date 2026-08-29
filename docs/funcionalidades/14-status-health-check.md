# 14. Status / health check

**Controller:** `StatusController` — endpoint **público** (não exige token).

`GET /api/Status` retorna:
- `DbCurrentDate` — data/hora atual lida do banco (ou `"ERRO"` se o banco não responder);
- `ApiCurrentDate` — data/hora atual da API (fuso de São Paulo).

Serve para monitorar se a API e o banco estão no ar e com o relógio coerente.
