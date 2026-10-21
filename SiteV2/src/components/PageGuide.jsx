import React, { useState } from 'react'
import { Box, Button, Collapse, Paper, Typography } from '@mui/material'
import LightbulbRoundedIcon from '@mui/icons-material/LightbulbRounded'
import CheckCircleRoundedIcon from '@mui/icons-material/CheckCircleRounded'

import { Guides } from '../content/guides'
import { StorageService } from '../services/storage.service'
import { BrandMark } from './BrandMark'

// Bloco "Lição do Bê-a-bá": explica a página em linguagem simples. O usuário pode recolher
// e a preferência fica salva no navegador.
export function PageGuide({ id }) {
  const guide = Guides[id]
  const [open, setOpen] = useState(() => !StorageService.isGuideDismissed(id))

  if (!guide)
    return null

  function toggle(value) {
    setOpen(value)
    StorageService.setGuideDismissed(id, !value)
  }

  return (
    <Box sx={{ mb: 3 }}>
      <Collapse in={!open} unmountOnExit>
        <Button size="small" color="secondary" variant="outlined" startIcon={<BrandMark size={20} />}
          onClick={() => toggle(true)} sx={{ color: 'text.secondary' }}>
          Lição do Bê-a-bá
        </Button>
      </Collapse>
      <Collapse in={open} unmountOnExit>
        <Paper variant="outlined" sx={theme => ({
          p: { xs: 2, sm: 2.5 },
          borderColor: 'secondary.main',
          borderLeftWidth: 6,
          backgroundColor: 'rgba(217, 154, 30, 0.07)',
          ...theme.applyStyles('dark', { backgroundColor: 'rgba(242, 193, 78, 0.07)' })
        })}>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 1.5 }}>
            <BrandMark size={26} />
            <Typography variant="overline" sx={{ color: 'secondary.main', lineHeight: 1.2 }}>Lição do Bê-a-bá</Typography>
          </Box>
          <Typography variant="h6" sx={{ mb: 1 }}>{guide.title}</Typography>
          <Box component="ul" sx={{ listStyle: 'none', p: 0, m: 0, display: 'grid', gap: 0.75 }}>
            {guide.points.map((p, i) =>
              <Box component="li" key={i} sx={{ display: 'flex', gap: 1, alignItems: 'flex-start' }}>
                <CheckCircleRoundedIcon sx={{ fontSize: 18, mt: '3px', color: 'primary.main' }} />
                <Typography variant="body2" sx={{ color: 'text.primary' }}>{p}</Typography>
              </Box>
            )}
          </Box>
          {guide.tip &&
            <Box sx={{ display: 'flex', gap: 1, mt: 2, p: 1.5, borderRadius: 2, bgcolor: 'background.paper' }}>
              <LightbulbRoundedIcon sx={{ color: 'secondary.main' }} />
              <Typography variant="body2"><b>Na prática:</b> {guide.tip}</Typography>
            </Box>
          }
          <Box sx={{ textAlign: 'right', mt: 1.5 }}>
            <Button size="small" onClick={() => toggle(false)}>Entendi, esconder</Button>
          </Box>
        </Paper>
      </Collapse>
    </Box>
  )
}
