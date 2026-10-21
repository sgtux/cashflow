import React from 'react'
import { IconButton, Tooltip } from '@mui/material'

export function ActionIcon({ title, onClick, color = 'default', children, ...props }) {
  return (
    <Tooltip title={title}>
      <IconButton aria-label={title} color={color} onClick={onClick} {...props}>
        {children}
      </IconButton>
    </Tooltip>
  )
}
