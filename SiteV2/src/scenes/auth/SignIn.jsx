import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Box, Button, Divider, TextField, Typography } from '@mui/material'
import { PublicClientApplication } from '@azure/msal-browser'

import { userChanged, showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { StorageService, authService, appConfig } from '../../services'
import { PasswordField, emailRegex } from './PasswordField'

const msal = new PublicClientApplication({
  auth: {
    clientId: appConfig.microsoftClientId,
    authority: appConfig.microsoftAuthority,
    redirectUri: appConfig.microsoftRedirectUri
  }
})

const oauthButtonSx = {
  position: 'relative',
  display: 'flex',
  alignItems: 'center',
  justifyContent: 'center',
  gap: 1.5,
  width: '100%',
  height: 44,
  borderRadius: 2.5,
  border: 1,
  borderColor: 'divider',
  bgcolor: 'background.paper',
  color: 'text.primary',
  fontFamily: 'inherit',
  fontWeight: 700,
  fontSize: 15,
  cursor: 'pointer',
  overflow: 'hidden',
  '&:hover': { bgcolor: 'action.hover' },
  '&:disabled': { cursor: 'default', opacity: 0.6 }
}

function GoogleLogo() {
  return (
    <svg width="18" height="18" viewBox="0 0 48 48" aria-hidden="true">
      <path fill="#EA4335" d="M24 9.5c3.54 0 6.71 1.22 9.21 3.6l6.85-6.85C35.9 2.38 30.47 0 24 0 14.62 0 6.51 5.38 2.56 13.22l7.98 6.19C12.43 13.72 17.74 9.5 24 9.5z" />
      <path fill="#4285F4" d="M46.98 24.55c0-1.57-.15-3.09-.38-4.55H24v9.02h12.94c-.58 2.96-2.26 5.48-4.78 7.18l7.73 6c4.51-4.18 7.09-10.36 7.09-17.65z" />
      <path fill="#FBBC05" d="M10.53 28.59c-.48-1.45-.76-2.99-.76-4.59s.27-3.14.76-4.59l-7.98-6.19C.92 16.46 0 20.12 0 24c0 3.88.92 7.54 2.56 10.78l7.97-6.19z" />
      <path fill="#34A853" d="M24 48c6.48 0 11.93-2.13 15.89-5.81l-7.73-6c-2.15 1.45-4.92 2.3-8.16 2.3-6.26 0-11.57-4.22-13.47-9.91l-7.98 6.19C6.51 42.62 14.62 48 24 48z" />
    </svg>
  )
}

function MicrosoftLogo() {
  return (
    <svg width="18" height="18" viewBox="0 0 21 21" aria-hidden="true">
      <rect x="1" y="1" width="9" height="9" fill="#F25022" />
      <rect x="11" y="1" width="9" height="9" fill="#7FBA00" />
      <rect x="1" y="11" width="9" height="9" fill="#00A4EF" />
      <rect x="11" y="11" width="9" height="9" fill="#FFB900" />
    </svg>
  )
}

export function SignInScreen({ changeScene }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [touched, setTouched] = useState({})
  const [microsoftLoading, setMicrosoftLoading] = useState(false)

  const dispatch = useDispatch()

  const emailValid = emailRegex.test(email)
  const passwordValid = password.length >= 4

  useEffect(() => {
    initializeGoogleOauth()
    msal.initialize().catch(err => console.log(err))
  }, [])

  function login(e) {
    if (e)
      e.preventDefault()
    if (!emailValid || !passwordValid)
      return
    dispatch(showGlobalLoader())
    authService.login({ email, password })
      .then(user => dispatch(userChanged(user)))
      .catch(err => console.log(err))
      .finally(() => dispatch(hideGlobalLoader()))
  }

  function initializeGoogleOauth() {
    try {
      google.accounts.id.initialize({
        client_id: appConfig.googleClientId,
        callback: handleGoogleOauthResponse
      })
      const googleButton = document.getElementById('googleButton')
      googleButton.innerHTML = ''
      google.accounts.id.renderButton(googleButton, { size: 'large', width: 360 })
      google.accounts.id.prompt()
    } catch (ex) {
      console.log(ex)
    }
  }

  async function handleGoogleOauthResponse(response) {
    try {
      dispatch(showGlobalLoader())
      const user = await authService.googleSignIn(response.credential)
      user.picture = StorageService.getPicture()
      dispatch(userChanged(user))
    } catch (ex) {
      console.log(ex)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  async function microsoftSignIn() {
    if (microsoftLoading)
      return
    setMicrosoftLoading(true)
    try {
      const result = await msal.loginPopup({ scopes: ['openid', 'profile', 'email'] })
      const user = await authService.oauthSignIn(result.account.idToken, 'MICROSOFT')
      user.picture = StorageService.getPicture()
      dispatch(userChanged(user))
    } catch (error) {
      if (error.errorCode === 'interaction_in_progress')
        window.sessionStorage.removeItem('msal.interaction.status')
      console.error('ERRO MSAL:', error)
    } finally {
      setMicrosoftLoading(false)
    }
  }

  return (
    <Box component="form" onSubmit={login} noValidate>
      <Typography variant="h5" component="h2">Que bom ver você!</Typography>
      <Typography sx={{ color: 'text.secondary', mt: 0.5, mb: 3 }}>Entre para continuar organizando suas finanças.</Typography>

      <Box sx={{ display: 'grid', gap: 2 }}>
        <TextField
          label="E-mail"
          type="email"
          autoComplete="email"
          value={email}
          onChange={e => setEmail(e.target.value)}
          onBlur={() => setTouched({ ...touched, email: true })}
          error={touched.email && !emailValid}
          helperText={touched.email && !emailValid ? 'Digite um e-mail válido, como nome@exemplo.com' : ' '}
        />
        <PasswordField
          label="Senha"
          autoComplete="current-password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          onBlur={() => setTouched({ ...touched, password: true })}
          error={touched.password && !passwordValid}
          helperText={touched.password && !passwordValid ? 'A senha tem pelo menos 4 caracteres' : ' '}
        />
      </Box>

      <Button type="submit" variant="contained" size="large" fullWidth disabled={!emailValid || !passwordValid} sx={{ mt: 1 }}>
        Entrar
      </Button>

      <Divider sx={{ my: 3, color: 'text.secondary', fontSize: 13 }}>ou entre com</Divider>

      <Box sx={{ display: 'grid', gap: 1.5 }}>
        <Box sx={oauthButtonSx}>
          <GoogleLogo /> Google
          {/* O botão oficial do Google fica invisível por cima, para manter o visual do app. */}
          <Box id="googleButton" sx={{ position: 'absolute', inset: 0, zIndex: 1, opacity: 0.0001, overflow: 'hidden' }} />
        </Box>
        <Box component="button" type="button" disabled={microsoftLoading} onClick={microsoftSignIn} sx={oauthButtonSx}>
          <MicrosoftLogo /> Microsoft
        </Box>
      </Box>

      <Typography sx={{ textAlign: 'center', mt: 3, color: 'text.secondary' }}>
        Primeira vez por aqui?{' '}
        <Button onClick={changeScene} sx={{ px: 1 }}>Criar uma conta grátis</Button>
      </Typography>
    </Box>
  )
}
