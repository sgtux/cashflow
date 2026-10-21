import React from 'react'
import { Box, Card, Typography } from '@mui/material'

import { Money } from './Money'
import { HelpTip } from './Term'

const accents = { in: 'success.main', out: 'error.main', neutral: 'info.main', warn: 'warning.main' }

export function StatCard({ label, value, tone = 'neutral', help, caption, icon }) {
  const moneyTone = tone === 'warn' ? 'neutral' : tone
  return (
    <Card sx={{ p: 2.5, height: '100%', borderTop: 4, borderTopColor: accents[tone] }}>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, color: 'text.secondary' }}>
        {icon}
        <Typography variant="body2" sx={{ fontWeight: 700 }}>
          {label}
          {help && <HelpTip id={help} />}
        </Typography>
      </Box>
      <Box sx={{ mt: 1 }}>
        <Money value={value} tone={moneyTone} size="xl" bold />
      </Box>
      {caption && <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block', mt: 0.5 }}>{caption}</Typography>}
    </Card>
  )
}
