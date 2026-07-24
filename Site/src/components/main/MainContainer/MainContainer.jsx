import React from 'react'
import { Paper, Typography, CircularProgress } from '@mui/material';

import { ContainerLoader } from './styles'

export function MainContainer({ title, loading, children }) {
  return (
    <Paper variant="outlined" sx={{ mt: 2.5, mb: 6, mx: 2.5, p: 1.5, position: 'relative' }}>
      <Typography variant="overline" sx={{ color: 'text.secondary', fontFamily: 'GraphikMedium', display: 'block', mb: 1 }}>
        {title}
      </Typography>
      {
        loading &&
        <ContainerLoader>
          <div>
            <CircularProgress size={50} />
          </div>
        </ContainerLoader>
      }
      {children}
    </Paper>
  )
}
