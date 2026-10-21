import React, { useState } from 'react'
import { IconButton, InputAdornment, TextField } from '@mui/material'
import { Visibility, VisibilityOff } from '@mui/icons-material'

export function PasswordField(props) {
  const [show, setShow] = useState(false)
  return (
    <TextField
      {...props}
      type={show ? 'text' : 'password'}
      slotProps={{
        input: {
          endAdornment: <InputAdornment position="end">
            <IconButton aria-label={show ? 'Esconder senha' : 'Mostrar senha'} onClick={() => setShow(!show)} edge="end">
              {show ? <VisibilityOff /> : <Visibility />}
            </IconButton>
          </InputAdornment>
        }
      }}
    />
  )
}

export const emailRegex = /^[a-zA-Z0-9!#$.%&*()_+-]{1,64}@[a-zA-Z0-9-]{1,63}(\.[a-zA-Z0-9-]{1,63})+$/
