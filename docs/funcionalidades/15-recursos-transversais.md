# 15. Recursos transversais

- **Cache por usuário** (`AppCache` → `HomeCache`, `ProjectionCache`): Home e Projeção
  são servidas do cache; **qualquer** criação/edição/remoção de ganho, despesa,
  parcelamento, recorrente, veículo, abastecimento ou saldo **limpa o cache** do usuário.
  Quando a resposta vem do cache, a SPA exibe um toast *"Obtido do cache."*.
- **Aviso de lentidão**: se `requestElapsedTime > 500 ms`, a SPA mostra um toast de
  alerta com a rota e o tempo.
- **Notificações**: erros de validação da API viram toasts vermelhos automaticamente
  (`react-toastify`); sucessos mostram toasts verdes.
- **Tema claro/escuro**: alternável na toolbar, persistido em `localStorage`
  (`@CASHFLOW_THEME_MODE`).
- **Loader global**: overlay animado durante requisições (`showGlobalLoader` /
  `hideGlobalLoader`).
- **Sessão expirada**: 401 em qualquer chamada autenticada dispara o modal
  "Sessão Expirada!" e logout.
- **Segurança**: JWT Bearer validado por middleware próprio + `[Authorize]` nos
  controllers; arquivos estáticos e endpoints sem `[Authorize]` (Status, criação de
  conta, login, Google) passam livres. Em produção, a SPA força redirecionamento para
  HTTPS.
- **Isolamento por usuário**: todo service confere que a entidade pertence ao `UserId`
  extraído do token antes de ler, alterar ou remover.
