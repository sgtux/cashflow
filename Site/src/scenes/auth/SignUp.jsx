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
import { userChanged } from '../../actions'

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

  const [nickName, setNickName] = useState('')
  const [nickNameValid, setNickNameValid] = useState(false)
  const [password, setPassword] = useState('')
  const [showPassword, setShowPassword] = useState(false)
  const [passwordValid, setPasswordValid] = useState(false)
  const [confirm, setConfirm] = useState('')
  const [showConfirm, setShowConfirm] = useState(false)
  const [confirmValid, setConfirmValid] = useState(false)
  const [loading, setLoading] = useState(false)

  const dispatch = useDispatch()

  function onInputChange(e) {
    if (e.name == 'nickName') {
      setNickName(e.value)
      setNickNameValid(e.valid)
    } else if (e.name == 'password') {
      setPassword(e.value)
      setPasswordValid(e.valid)
    } else if (e.name == 'confirm') {
      setConfirm(e.value)
      setConfirmValid(e.valid)
    }
  }

  function send() {
    setLoading(true)
    authService.createAccount({ nickName, password, confirm })
      .then(res => dispatch(userChanged(res)))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  return (
    <Zoom in={true}>
      <Card style={styles.Card}>
        <CardContent>
          <IconTextInput
            label="Nick Name"
            required
            minlength={5}
            name="nickName"
            onChange={e => onInputChange(e)}
            Icon={<Person />}
          />
          <IconTextInput
            type={showPassword ? 'text' : 'password'}
            required
            label="Password"
            name="password"
            onChange={e => onInputChange(e)}
            minlength={4}
            Icon={showPassword ? <VisibilityOff /> : <Visibility />}
            iconClick={() => setShowPassword(!showPassword)}
          />
          <IconTextInput
            required
            type={showConfirm ? 'text' : 'password'}
            label="Confirm Password"
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
          onClick={() => send()}
          disabled={!nickNameValid || !passwordValid || !confirmValid}
          color="primary">Send</Button>
        <br /><br />
        <div style={{ marginBottom: '10px' }} hidden={!loading}>
          <CircularProgress />
        </div>
        <div hidden={loading}>
          <Button variant="outlined"
            onClick={changeScene}
            style={{ width: '250px' }} color="primary">back to Login</Button>
        </div>
      </Card>
    </Zoom>
  )
}