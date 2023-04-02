import React, { useState } from 'react'
import { useDispatch } from 'react-redux'

import {
  CardContent,
  Card,
  Button,
  Zoom,
  CircularProgress
} from '@material-ui/core'

import { Visibility, VisibilityOff, Person } from '@material-ui/icons'

import IconTextInput from '../../components/main/IconTextInput'
import { authService } from '../../services'
import { userChanged } from '../../store/actions'

const styles = {
  Card: {
    width: '300px',
    margin: '0 auto',
    textAlign: 'center',
    paddingBottom: '20px',
    paddingTop: '20px',
    marginTop: '150px'
  }
}

export function SignUpScreen({ changeScene }) {

  const [email, setEmail] = useState('')
  const [emailValid, setEmailValid] = useState(false)
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [passwordValid, setPasswordValid] = useState(false)
  const [confirm, setConfirm] = useState('')
  const [showConfirm, setShowConfirm] = useState(false)
  const [confirmValid, setConfirmValid] = useState(false)
  const [loading, setLoading] = useState(false)

  const dispatch = useDispatch()

  function onInputChange(e) {
    if (e.name == 'email') {
      if (/^[a-zA-Z0-9_$#@!&]{1,}$/.test(e.value || '')) {
        setEmail(e.value)
        setEmailValid(e.valid)
      }
    } else if (e.name == 'password') {
      setPassword(e.value)
      setPasswordValid(e.valid)
    } else if (e.name == 'confirm') {
      setConfirm(e.value)
      setConfirmValid(e.valid)
    }
  }

  function send(e) {
    if (e)
      e.preventDefault()
    setLoading(true)
    authService.createAccount({ email, password, confirm })
      .then(res => dispatch(userChanged(res)))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  return (
    <Zoom in={true}>
      <Card style={styles.Card}>
        <CardContent>
          <IconTextInput
            label="Apelido"
            required
            minlength={5}
            name="email"
            value={email}
            onChange={e => onInputChange(e)}
            Icon={<Person />}
          />
          <IconTextInput
            type={showPassword ? 'text' : 'password'}
            required
            label="Senha"
            name="password"
            onChange={e => onInputChange(e)}
            minlength={4}
            Icon={showPassword ? <VisibilityOff /> : <Visibility />}
            iconClick={() => setShowPassword(!showPassword)}
          />
          <IconTextInput
            required
            type={showConfirm ? 'text' : 'password'}
            label="Confirme a Senha"
            name="confirm"
            onChange={e => onInputChange(e)}
            pattern={`^${password}$`}
            patternMessage="As senhas não batem."
            Icon={showConfirm ? <VisibilityOff /> : <Visibility />}
            iconClick={() => setShowConfirm(!showConfirm)}
          />
        </CardContent>
        <br />
        <Button style={{ width: '250px' }}
          variant="contained"
          onClick={e => send(e)}
          disabled={loading || !emailValid || !passwordValid || !confirmValid}
          color="primary">Enviar</Button>
        <br /><br />
        <div style={{ marginBottom: '10px' }} hidden={!loading}>
          <CircularProgress />
        </div>
        <div hidden={loading}>
          <Button variant="outlined"
            onClick={changeScene}
            style={{ width: '250px' }} color="primary">Voltar</Button>
        </div>
      </Card>
    </Zoom>
  )
}