# 4. Home (painel do mês)

**Controller:** `HomeController` · **Service:** `HomeService` · **Tela:** `home/Home.jsx`

`GET /api/Home?month={mm}&year={yyyy}` monta o panorama de **um mês de referência**
(seletor de mês/ano no topo da tela). Resultado é **cacheado** por usuário.

Blocos da tela:

- **Entradas** — lista de proventos do mês e total.
- **Saídas** — total agregado por origem:
  - *Despesas Domésticas* (soma do mês; se zero, usa o limite do usuário);
  - *Parcelamentos* (soma das parcelas do mês);
  - *Combustível* (soma dos abastecimentos do mês; se zero, usa o limite de combustível);
  - *Despesas Recorrentes* (soma das ativas).
- **Pendências** — parcelas de parcelamentos e despesas recorrentes ainda **não pagas**
  no mês, com ícone de alerta e total. Indica quando o item está vinculado a cartão.
- **Limites** — barras de progresso "gasto / limite" para Despesas Domésticas e
  Combustível, mudando de cor conforme o percentual consumido (verde &lt; 65%, amarelo
  &lt; 80%, vermelho &lt; 95%, vermelho-escuro acima disso).
