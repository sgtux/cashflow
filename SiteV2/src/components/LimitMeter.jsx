import React from 'react'
import { Box, LinearProgress, Typography } from '@mui/material'

import { Money } from './Money'

// Mesmas faixas do Site: verde < 65%, amarelo < 80%, vermelho < 95%, vermelho-escuro acima.
function getLevel(percent) {
  if (percent < 65)
    return { color: 'success', text: 'Tranquilo: ainda há bastante espaço no orçamento.' }
  if (percent < 80)
    return { color: 'warning', text: 'Atenção: você já usou boa parte do limite.' }
  if (percent < 95)
    return { color: 'error', text: 'Cuidado: o limite está quase no fim.' }
  return { color: 'error', text: 'Limite atingido. Tente segurar os gastos até o mês virar.', critical: true }
}

export function LimitMeter({ description, spent, limit }) {
  const percent = limit > 0 ? (spent * 100) / limit : (spent > 0 ? 100 : 0)
  const level = getLevel(percent)
  const left = (limit || 0) - (spent || 0)

  return (
    <Box sx={{ py: 1.25 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'baseline', gap: 1, flexWrap: 'wrap' }}>
        <Typography sx={{ fontWeight: 700 }}>{description}</Typography>
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
          <Money value={spent} size="sm" bold /> de <Money value={limit} size="sm" />
        </Typography>
      </Box>
      <LinearProgress
        variant="determinate"
        color={level.color}
        value={Math.min(100, percent)}
        sx={{ height: 12, borderRadius: 99, mt: 1, bgcolor: 'action.hover', ...(level.critical && { '& .MuiLinearProgress-bar': { bgcolor: 'error.dark' } }) }}
      />
      <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 1, mt: 0.5, flexWrap: 'wrap' }}>
        <Typography variant="caption" sx={{ color: `${level.color}.main`, fontWeight: 700 }}>{level.text}</Typography>
        <Typography variant="caption" sx={{ color: 'text.secondary' }}>
          {limit > 0 ? (left >= 0 ? <>Ainda pode gastar <Money value={left} size="xs" bold /></> : <>Passou <Money value={-left} size="xs" bold tone="out" /> do limite</>) : 'Sem limite definido'}
        </Typography>
      </Box>
    </Box>
  )
}
