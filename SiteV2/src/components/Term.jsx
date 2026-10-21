import React from 'react'
import { Box, Tooltip, IconButton } from '@mui/material'
import HelpOutlineRoundedIcon from '@mui/icons-material/HelpOutlineRounded'

import { Glossary } from '../content/glossary'

function TermContent({ id, text }) {
  const item = Glossary[id]
  if (!item)
    return text || null
  return (
    <Box>
      <Box sx={{ fontWeight: 800, mb: 0.5 }}>{item.term}</Box>
      <Box>{item.short}</Box>
      {item.example && <Box sx={{ mt: 1, opacity: 0.85, fontStyle: 'italic' }}>Ex.: {item.example}</Box>}
    </Box>
  )
}

// Palavra sublinhada que explica o termo financeiro ao passar o mouse (ou tocar, no celular).
export function Term({ id, children }) {
  return (
    <Tooltip title={<TermContent id={id} />}>
      <Box component="span" tabIndex={0} sx={{
        borderBottom: '2px dotted',
        borderColor: 'secondary.main',
        cursor: 'help',
        outline: 'none'
      }}>
        {children}
      </Box>
    </Tooltip>
  )
}

// Ícone "?" com a explicação de um termo do glossário (id) ou de um texto livre (text).
export function HelpTip({ id, text, size = 'small' }) {
  return (
    <Tooltip title={<TermContent id={id} text={text} />}>
      <IconButton size={size} aria-label="O que é isso?" sx={{ p: 0.25, ml: 0.5, color: 'text.secondary', verticalAlign: 'middle' }}>
        <HelpOutlineRoundedIcon sx={{ fontSize: size === 'small' ? 18 : 22 }} />
      </IconButton>
    </Tooltip>
  )
}
