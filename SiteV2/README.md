# SiteV2 — "Bê-a-bá da Grana · Conta"

Nova interface do Cashflow (app **Conta**, do ecossistema **Bê-a-bá da Grana**: Conta, Rende e Magnata — "cada tostão conta"), pensada para quem está dando os primeiros passos na organização
financeira. Usa **a mesma API e as mesmas regras** do `Site/` (mesmos endpoints, rotas e
fluxos), com um layout didático:

- menu organizado em passos: **1. O que entra → 2. O que sai → 3. Planejar**;
- bloco **"Lição do Bê-a-bá"** em cada página (pode ser recolhido; fica salvo no navegador);
- termos financeiros com dica **"?"** e uma página de **Glossário** (`src/content/glossary.js`);
- **Primeiros passos** na tela inicial, com checklist de onboarding;
- resumo do mês em linguagem simples ("entram X, saem Y, sobram Z") e alertas na projeção;
- tema claro por padrão, com tema escuro opcional.

Stack: React 18 · Redux · Material UI 6 · React Router (HashRouter) · Axios · Webpack (pnpm).

## Rodando

```bash
cd SiteV2
pnpm install
pnpm start          # http://localhost:8081 — proxy de /api -> :56666 (API local)
pnpm build:prod     # saída em SiteV2/dist
```

Para gerar direto no `wwwroot` da API (substituindo o Site atual):

```bash
OUTPUT_PATH=../Api/wwwroot pnpm build:prod
```

## Estrutura

```
src/
  content/     textos didáticos: glossário e guias de cada tela
  components/  peças reutilizáveis (Page, Money, Term/HelpTip, campos, diálogos...)
  layout/      menu lateral, barra superior, rotas e sessão
  scenes/      telas (recurring/ atende Ganhos fixos e Contas fixas com a mesma base)
  services/    chamadas à API (mesmo contrato do Site)
```
