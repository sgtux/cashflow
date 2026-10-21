import React from 'react'
import { Avatar, Box, CircularProgress, Typography } from '@mui/material'

import { PageGuide } from './PageGuide'

// Estrutura padrão das telas: cabeçalho (ícone, título, explicação curta, ações), guia e conteúdo.
export function Page({ icon, title, subtitle, actions, guide, loading, children, maxWidth = 1100 }) {
  return (
    <Box sx={{ px: { xs: 2, sm: 3, md: 4 }, py: { xs: 2.5, md: 4 }, maxWidth, mx: 'auto', position: 'relative' }}>
      <Box sx={{ display: 'flex', flexWrap: 'wrap', alignItems: 'center', gap: 2, mb: 2.5 }}>
        {icon &&
          <Avatar variant="rounded" sx={{ bgcolor: 'primary.main', color: 'primary.contrastText', width: 52, height: 52, borderRadius: 3 }}>
            {icon}
          </Avatar>
        }
        <Box sx={{ flex: '1 1 240px', minWidth: 0 }}>
          <Typography variant="h4" component="h1">{title}</Typography>
          {subtitle && <Typography sx={{ color: 'text.secondary', mt: 0.25 }}>{subtitle}</Typography>}
        </Box>
        {actions && <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>{actions}</Box>}
      </Box>
      {guide && <PageGuide id={guide} />}
      {loading &&
        <Box sx={{ position: 'absolute', inset: 0, zIndex: 2, display: 'flex', justifyContent: 'center', pt: 20, bgcolor: 'rgba(0,0,0,0.04)' }}>
          <CircularProgress />
        </Box>
      }
      {children}
    </Box>
  )
}
