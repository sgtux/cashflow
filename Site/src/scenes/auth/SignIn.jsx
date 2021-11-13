import React, { useState } from 'react'
import { useDispatch } from 'react-redux'
import { Person, Visibility, VisibilityOff } from '@material-ui/icons'
import {
  CardContent,
  Card,
  Button,
  Zoom,
  CircularProgress
} from '@material-ui/core'

import IconTextInput from '../../components/main/IconTextInput'
import { userChanged } from '../../actions'
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
  const [nickName, setNickName] = useState(false)
  const [nickNameValid, setNickNameValid] = useState(false)
  const [showPassword, setShowPassword] = useState(false)
  const [loading, setLoading] = useState(false)

  const dispatch = useDispatch()


  function onInputChange(e) {
    if (e.name === 'nickName') {
      setNickName(e.value)
      setNickNameValid(e.valid)
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
    authService.login({ nickName, password })
      .then(user => dispatch(userChanged(user)))
      .catch(() => { })
      .finally(() => setLoading(false))
  }

  return (
    <Zoom in={true}>
      <Card style={styles.Card}>
        <form onSubmit={e => login(e)}>
          <CardContent>
            <IconTextInput
              label="Apelido"
              required
              disabled={loading}
              name="nickName"
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
              disabled={!nickNameValid || !passwordValid}
              type="submit"
              onClick={e => login(e)}
              color="primary">Entrar</Button>
            <br /><br />
            <Button style={{ width: '250px' }}
              variant="outlined"
              onClick={changeScene}
              color="primary">Criar Conta</Button>
          </div>
        </form>
      </Card>
    </Zoom>
  )
}