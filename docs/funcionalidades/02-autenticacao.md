# 2. Autenticação e criação de conta

**Controllers:** `AccountController`, `TokenController` · **Service:** `AccountService` ·
**Telas:** `auth/SignIn.jsx`, `auth/SignUp.jsx`

## 2.1 Criar conta (e-mail + senha)
- `POST /api/Account` com `{ email, password, confirm }`.
- Regras (`UserValidator`): e-mail em formato válido e ainda não utilizado; senha com no
  mínimo 8 caracteres; a tela ainda exige confirmação de senha idêntica.
- Há um **limite global de usuários do sistema** (parâmetro `MAXIMUM_SYSTEM_USERS` na
  tabela de parâmetros). Atingido o limite, o cadastro é bloqueado.
- Ao criar, a senha é gravada com **hash BCrypt**, o plano inicial é **Free** e retorna
  já um **token JWT** (login automático).

## 2.2 Login por e-mail e senha
- `POST /api/token` com `{ email, password }`.
- Valida o hash BCrypt; em caso de falha retorna **401** com "Usuário ou senha inválidos.".
- No login, o sistema **recalcula o saldo remanescente do mês anterior** antes de
  devolver o token.

## 2.3 Login com Google (OAuth)
- `GET /api/Account/GoogleClientId` devolve o *client id* para o botão do Google.
- `POST /api/Account/GoogleSignIn` aceita `idToken` **ou** `accessToken`; a API consulta
  o endpoint OAuth do Google para validar e obter e-mail e foto.
- Se o e-mail ainda não existe, uma conta é **criada automaticamente** (respeitando o
  limite de usuários). Retorna token JWT + foto do perfil.
- Também dispara o recálculo do saldo do mês anterior e a atualização da contagem de
  registros usados.

## 2.4 Sessão
- O token é enviado no header `Authorization: Bearer <jwt>` em todas as chamadas
  autenticadas; validade em minutos vem de `COOKIE_EXPIRES_IN_MINUTES`.
- A SPA guarda usuário/token/foto/tema no `localStorage`.
- Ao receber **401** em qualquer requisição (exceto no próprio `/api/token`), a SPA abre
  o modal **"Sessão Expirada!"** e faz logout.
