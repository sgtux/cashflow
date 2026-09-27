import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Person, Visibility, VisibilityOff } from '@mui/icons-material'
import {
  CardContent,
  Card,
  Button,
  Zoom,
  Divider,
  Box
} from '@mui/material'

import IconTextInput from '../../components/main/IconTextInput'
import { userChanged, showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { StorageService, authService, appConfig } from '../../services'

import { PublicClientApplication } from '@azure/msal-browser'

const msalConfig = {
  auth: {
    clientId: appConfig.microsoftClientId,
    authority: appConfig.microsoftAuthority,
    redirectUri: appConfig.microsoftRedirectUri
  }
}

const msal = new PublicClientApplication(msalConfig)

const styles = {
  Card: {
    width: '300px',
    margin: '0 auto',
    textAlign: 'center',
    paddingBottom: '20px',
    paddingTop: '20px',
    marginTop: '150px'
  },
  Or: {
    textAlign: 'center',
    color: 'var(--mui-palette-text-secondary)',
    marginTop: '25px',
    fontWeight: 'bold',
    fontFamily: 'GraphikRegular'
  },
  OAuthButton: {
    display: 'flex',
    alignItems: 'center',
    position: 'relative',
    width: 240,
    height: 40,
    margin: '0 auto',
    marginTop: '10px',
    padding: '0 12px',
    backgroundColor: '#202124',
    color: '#E8EAED',
    border: 'none',
    borderRadius: '4px',
    boxSizing: 'border-box',
    overflow: 'hidden',
    cursor: 'pointer',
    fontFamily: "'Google Sans', arial, sans-serif",
    fontSize: 14,
    fontWeight: 500,
    letterSpacing: '0.25px',
    whiteSpace: 'nowrap',
    outline: 'none',
    '&::after': {
      content: '""',
      position: 'absolute',
      inset: 0,
      transition: 'background-color .218s',
      pointerEvents: 'none'
    },
    '&:hover::after': { backgroundColor: 'rgba(255, 255, 255, .24)' },
    '&:active::after, &:focus-visible::after': { backgroundColor: 'rgba(255, 255, 255, .32)' },
    '&:disabled': { cursor: 'default', opacity: 0.6 }
  },
  OAuthButtonIcon: {
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
    width: 36,
    minWidth: 36,
    height: 36,
    marginLeft: '-10px',
    marginRight: '12px',
    backgroundColor: '#fff',
    borderTopLeftRadius: '3px',
    borderBottomLeftRadius: '3px'
  },
  OAuthButtonText: {
    flexGrow: 1,
    textAlign: 'center',
    overflow: 'hidden',
    textOverflow: 'ellipsis'
  },
  GoogleButtonOverlay: {
    position: 'absolute',
    inset: 0,
    zIndex: 1,
    opacity: 0.0001,
    overflow: 'hidden'
  }
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

  const [password, setPassword] = useState('')
  const [passwordValid, setPasswordValid] = useState('')
  const [email, setEmail] = useState(false)
  const [emailValid, setEmailValid] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [microsoftLoading, setMicrosoftLoading] = useState(false)

  const dispatch = useDispatch()

  useEffect(() => {
    async function initializeOauth() {
      await initializeGoogleOauth()
      await msal.initialize()
    }
    initializeOauth()
  }, [])

  function onInputChange(e) {
    if (e.name === 'email') {
      setEmail(e.value)
      setEmailValid(e.valid)
    } else if (e.name === 'password') {
      setPassword(e.value)
      setPasswordValid(e.valid)
    }
  }

  function login(e) {
    if (e)
      e.preventDefault()
    dispatch(showGlobalLoader())
    authService.login({ email, password })
      .then(user => dispatch(userChanged(user)))
      .catch(err => console.log(err))
      .finally(() => dispatch(hideGlobalLoader()))
  }

  async function initializeMicrosoftOauth() {
    if (microsoftLoading) return
    setMicrosoftLoading(true)

    try {
      const login = await msal.loginPopup({
        scopes: ["openid", "profile", "email"]
      });

      const response = await authService.oauthSignIn(login.account.idToken, 'MICROSOFT')
      console.log(response)
      handleAuthUserResponse(response)

    } catch (error) {
      if (error.errorCode === 'interaction_in_progress') {
        window.sessionStorage.removeItem('msal.interaction.status')
      }
      console.error("ERRO MSAL:", error);
    } finally {
      setMicrosoftLoading(false)
    }
  }

  async function initializeGoogleOauth() {
    dispatch(showGlobalLoader())
    try {
      google.accounts.id.initialize({
        client_id: appConfig.googleClientId,
        callback: handleGoogleOauthResponse
      })
      const googleButton = document.getElementById('googleButton')
      googleButton.innerHTML = ''
      google.accounts.id.renderButton(
        googleButton,
        {
          theme: 'filled_black',
          size: 'large',
          width: 240,
        }
      )
      google.accounts.id.prompt()
    } catch (ex) {
      console.log(ex)
    } finally {
      dispatch(hideGlobalLoader())
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
    }
    finally { dispatch(hideGlobalLoader()) }
  }

  async function handleAuthUserResponse(response) {
    StorageService.setToken(response.token)
    dispatch(userChanged(response.data))
  }

  return (
    <Zoom in={true}>
      <Card style={styles.Card}>
        <form onSubmit={e => login(e)}>
          <CardContent>
            <IconTextInput
              label="Email"
              required
              name="email"
              onChange={e => onInputChange(e)}
              Icon={<Person />}
            />
            <IconTextInput
              type={showPassword ? 'text' : 'password'}
              label="Senha"
              required
              minlength={4}
              onChange={e => onInputChange(e)}
              name="password"
              Icon={showPassword ? <VisibilityOff /> : <Visibility />}
              iconClick={() => setShowPassword(!showPassword)} />
          </CardContent>
          <br />
          <div>
            <Button style={{ width: '250px' }}
              variant="contained"
              disabled={!emailValid || !passwordValid}
              type="submit"
              onClick={e => login(e)}
              color="primary">Entrar</Button>
            <br /><br />
            <Button style={{ width: '250px' }}
              variant="outlined"
              onClick={changeScene}
              color="primary">Criar Conta</Button>
          </div>
          <br />
          <Divider />
          <Box sx={{ ...styles.OAuthButton, marginTop: '20px' }}>
            <span style={styles.OAuthButtonIcon}>
              <GoogleLogo />
            </span>
            <span style={styles.OAuthButtonText}>Entrar com Google</span>
            <div id="googleButton" style={styles.GoogleButtonOverlay}></div>
          </Box>
          <Box
            component="button"
            type="button"
            disabled={microsoftLoading}
            onClick={() => initializeMicrosoftOauth()}
            sx={styles.OAuthButton}
          >
            <span style={styles.OAuthButtonIcon}>
              <MicrosoftLogo />
            </span>
            <span style={styles.OAuthButtonText}>Entrar com Microsoft</span>
          </Box>
        </form>
      </Card>
    </Zoom>
  )
}