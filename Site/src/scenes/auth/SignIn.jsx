import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Person, Visibility, VisibilityOff } from '@mui/icons-material'
import {
  CardContent,
  Card,
  Button,
  Zoom,
  CircularProgress,
  Divider
} from '@mui/material'

import IconTextInput from '../../components/main/IconTextInput'
import { userChanged } from '../../store/actions'
import { authService } from '../../services'

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
    color: '#666',
    marginTop: '25px',
    fontWeight: 'bold',
    fontFamily: 'Arial'
  }
}

export function SignInScreen({ changeScene }) {

  const [password, setPassword] = useState('')
  const [passwordValid, setPasswordValid] = useState('')
  const [email, setEmail] = useState(false)
  const [emailValid, setEmailValid] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [loading, setLoading] = useState(false)

  const dispatch = useDispatch()

  useEffect(() => {
    initializeGoogleOauth()
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
    if (loading)
      return
    setLoading(true)
    authService.login({ email, password })
      .then(user => dispatch(userChanged(user)))
      .catch(() => { })
      .finally(() => setLoading(false))
  }

  async function initializeGoogleOauth() {
    const googleClientId = await authService.getGoogleClientId()
    google.accounts.id.initialize({
      client_id: googleClientId,
      callback: handleCredentialResponse
    })
    google.accounts.id.renderButton(
      document.getElementById('buttonDiv'),
      {
        theme: 'outline',
        size: 'large',
      }
    )
    google.accounts.id.prompt()
  }

  async function handleCredentialResponse(response) {
    try {
      const user = await authService.googleSignIn(response.credential)
      dispatch(userChanged(user))
    } catch (ex) {
      console.log(ex)
    }
  }

  return (
    <Zoom in={true}>
      <Card style={styles.Card}>
        <form onSubmit={e => login(e)}>
          <CardContent>
            <IconTextInput
              label="Email"
              required
              disabled={loading}
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
              disabled={loading}
              Icon={showPassword ? <VisibilityOff /> : <Visibility />}
              iconClick={() => setShowPassword(!showPassword)} />
          </CardContent>
          <br />
          <div style={{ marginBottom: '10px' }} hidden={!loading}>
            <CircularProgress />
          </div>
          <div hidden={loading}>
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
          <div style={{ margin: '0 auto', marginTop: 20, width: 240 }} id="buttonDiv"></div>
        </form>
      </Card>
    </Zoom>
  )
}