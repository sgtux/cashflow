import React from 'react'
import { Box, Card, Typography } from '@mui/material'

import { HelpTip } from './Term'

export function SectionCard({ title, help, helpText, icon, action, children, sx, contentSx }) {
  return (
    <Card sx={{ height: '100%', ...sx }}>
      {title &&
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, px: 2.5, pt: 2, pb: 1 }}>
          {icon}
          <Typography variant="h6" sx={{ flex: 1, fontSize: '1.05rem' }}>
            {title}
            {(help || helpText) && <HelpTip id={help} text={helpText} />}
          </Typography>
          {action}
        </Box>
      }
      <Box sx={{ px: 2.5, pb: 2.5, pt: title ? 0.5 : 2.5, ...contentSx }}>
        {children}
      </Box>
    </Card>
  )
}
