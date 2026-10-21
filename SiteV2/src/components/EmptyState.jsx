import React from 'react'
import { Box, Button, Card, Typography } from '@mui/material'

// Tela vazia que ensina o próximo passo em vez de só dizer "sem registros".
export function EmptyState({ icon, title, text, actionLabel, onAction }) {
  return (
    <Card sx={{ p: { xs: 3, sm: 5 }, textAlign: 'center', borderStyle: 'dashed' }}>
      {icon && <Box sx={{ color: 'primary.main', '& svg': { fontSize: 56 } }}>{icon}</Box>}
      <Typography variant="h6" sx={{ mt: 1 }}>{title}</Typography>
      {text && <Typography sx={{ color: 'text.secondary', mt: 1, maxWidth: 520, mx: 'auto' }}>{text}</Typography>}
      {actionLabel &&
        <Button variant="contained" sx={{ mt: 2.5 }} onClick={onAction}>{actionLabel}</Button>
      }
    </Card>
  )
}
