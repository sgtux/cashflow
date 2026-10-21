import React from 'react'
import { Box } from '@mui/material'

import { toReal } from '../helpers'
import { MONEY_FONT } from '../theme'

const sizes = { xs: 12, sm: 14, md: 16, lg: 20, xl: 28, xxl: 36 }

// tone: 'in' (verde), 'out' (coral), 'neutral' ou 'auto' (cor pelo sinal do valor).
export function Money({ value, tone = 'neutral', size = 'sm', bold, sx, prefix }) {
  let color = 'text.primary'
  if (tone === 'in' || (tone === 'auto' && value >= 0))
    color = 'success.main'
  else if (tone === 'out' || (tone === 'auto' && value < 0))
    color = 'error.main'

  return (
    <Box component="span" sx={{
      fontFamily: MONEY_FONT,
      fontVariantNumeric: 'tabular-nums',
      fontWeight: bold ? 700 : 500,
      fontSize: sizes[size] || size,
      whiteSpace: 'nowrap',
      color,
      ...sx
    }}>
      {prefix}{toReal(value || 0)}
    </Box>
  )
}
