import React from 'react'
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography, useMediaQuery } from '@mui/material'
import { useTheme } from '@mui/material/styles'

export function FormDialog({ open, title, subtitle, onClose, onSave, saveLabel = 'Salvar', saveDisabled, maxWidth = 'sm', children, hideActions }) {
  const theme = useTheme()
  const fullScreen = useMediaQuery(theme.breakpoints.down('sm'))

  return (
    <Dialog open={!!open} onClose={onClose} fullWidth maxWidth={maxWidth} fullScreen={fullScreen}>
      <DialogTitle sx={{ pb: subtitle ? 0.5 : 1 }}>
        <Box component="span" sx={{ fontWeight: 800 }}>{title}</Box>
        {subtitle && <Typography variant="body2" sx={{ color: 'text.secondary', mt: 0.5 }}>{subtitle}</Typography>}
      </DialogTitle>
      <DialogContent>
        <Box sx={{ display: 'grid', gap: 2.5, pt: 1.5 }}>
          {children}
        </Box>
      </DialogContent>
      {!hideActions &&
        <DialogActions sx={{ px: 3, pb: 2.5 }}>
          <Button onClick={onClose}>Cancelar</Button>
          <Button variant="contained" disabled={saveDisabled} onClick={onSave}>{saveLabel}</Button>
        </DialogActions>
      }
    </Dialog>
  )
}
