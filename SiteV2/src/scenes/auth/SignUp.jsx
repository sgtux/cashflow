import React, { useState } from 'react'
import { useDispatch } from 'react-redux'
import { Box, Button, TextField, Typography } from '@mui/material'
import ArrowBackRoundedIcon from '@mui/icons-material/ArrowBackRounded'

import { authService } from '../../services'
import { userChanged, showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { PasswordField, emailRegex } from './PasswordField'

export function SignUpScreen({ changeScene }) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [confirm, setConfirm] = useState('')
  const [touched, setTouched] = useState({})

  const dispatch = useDispatch()

  const emailValid = email.length >= 5 && emailRegex.test(email)
  const passwordValid = password.length >= 4
  const confirmValid = !!confirm && confirm === password

  function send(e) {
    if (e)
      e.preventDefault()
    if (!emailValid || !passwordValid || !confirmValid)
      return
    dispatch(showGlobalLoader())
    authService.createAccount({ email, password, confirm })
      .then(res => dispatch(userChanged(res)))
      .catch(err => console.log(err))
      .finally(() => dispatch(hideGlobalLoader()))
  }

  const touch = name => () => setTouched({ ...touched, [name]: true })

  return (
    <Box component="form" onSubmit={send} noValidate>
      <Button startIcon={<ArrowBackRoundedIcon />} onClick={changeScene} sx={{ ml: -1, mb: 1 }}>Voltar</Button>
      <Typography variant="h5" component="h2">Crie sua conta</Typography>
      <Typography sx={{ color: 'text.secondary', mt: 0.5, mb: 3 }}>É grátis e leva menos de um minuto.</Typography>

      <Box sx={{ display: 'grid', gap: 2 }}>
        <TextField
          label="E-mail"
          type="email"
          autoComplete="email"
          value={email}
          onChange={e => setEmail(e.target.value)}
          onBlur={touch('email')}
          error={touched.email && !emailValid}
          helperText={touched.email && !emailValid ? 'Digite um e-mail válido, como nome@exemplo.com' : ' '}
        />
        <PasswordField
          label="Crie uma senha"
          autoComplete="new-password"
          value={password}
          onChange={e => setPassword(e.target.value)}
          onBlur={touch('password')}
          error={touched.password && !passwordValid}
          helperText={touched.password && !passwordValid ? 'Use pelo menos 4 caracteres' : 'Dica: misture letras e números.'}
        />
        <PasswordField
          label="Repita a senha"
          autoComplete="new-password"
          value={confirm}
          onChange={e => setConfirm(e.target.value)}
          onBlur={touch('confirm')}
          error={touched.confirm && !confirmValid}
          helperText={touched.confirm && !confirmValid ? 'As senhas não estão iguais' : ' '}
        />
      </Box>

      <Button type="submit" variant="contained" size="large" fullWidth sx={{ mt: 1 }}
        disabled={!emailValid || !passwordValid || !confirmValid}>
        Criar conta
      </Button>
    </Box>
  )
}
