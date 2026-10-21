import React from 'react'
import { Box, Tooltip, Typography } from '@mui/material'

import { Money } from '../../components'
import { Months } from '../../helpers'

// Polaridade (sobra x falta) em par divergente frio/quente validado para daltonismo,
// com a posição acima/abaixo do zero como codificação secundária.
const colorSx = positive => theme => ({
  bgcolor: positive ? '#2F6FB2' : '#D9661E',
  ...theme.applyStyles('dark', { bgcolor: positive ? '#4A8AD4' : '#D9732E' })
})

const HEIGHT = 180
// Espaço reservado acima e abaixo das barras para os rótulos diretos não serem cortados.
const PAD = 22

export function BalanceChart({ months }) {
  if (!months.length)
    return null

  const values = months.map(p => p.accumulatedValue)
  const max = Math.max(0, ...values)
  const min = Math.min(0, ...values)
  const range = (max - min) || 1
  const zeroTop = PAD + (max / range) * HEIGHT

  const firstNegative = months.findIndex(p => p.accumulatedValue < 0)
  const last = months.length - 1

  return (
    <Box>
      <Box sx={{ display: 'flex', gap: 2, mb: 1.5, flexWrap: 'wrap' }}>
        {[[true, 'Sobra acumulada'], [false, 'Falta acumulada']].map(([positive, label]) =>
          <Box key={label} sx={{ display: 'flex', alignItems: 'center', gap: 0.75 }}>
            <Box sx={[{ width: 12, height: 12, borderRadius: 0.5 }, colorSx(positive)]} />
            <Typography variant="caption" sx={{ color: 'text.secondary', fontWeight: 700 }}>{label}</Typography>
          </Box>
        )}
      </Box>
      <Box sx={{ overflowX: 'auto', pb: 1 }}>
        <Box sx={{ position: 'relative', height: HEIGHT + PAD * 2 + 20, minWidth: months.length * 36 }}>
          <Box sx={{ position: 'absolute', left: 0, right: 0, top: zeroTop, borderTop: 1, borderColor: 'text.disabled' }} />
          <Box sx={{ position: 'absolute', inset: 0, display: 'flex', gap: '2px' }}>
            {months.map((p, i) => {
              const positive = p.accumulatedValue >= 0
              const h = Math.max(2, (Math.abs(p.accumulatedValue) / range) * HEIGHT)
              const [m, y] = (p.monthYear || '').split('/')
              const labelled = i === firstNegative || i === last
              return (
                <Tooltip key={i} placement="top" title={
                  <Box>
                    <Box sx={{ fontWeight: 800 }}>{Months[Number(m) - 1]} de {y}</Box>
                    <Box>Saldo do mês: <Money value={p.total} sx={{ color: 'inherit' }} /></Box>
                    <Box>Acumulado: <Money value={p.accumulatedValue} bold sx={{ color: 'inherit' }} /></Box>
                  </Box>
                }>
                  <Box sx={{ flex: 1, position: 'relative', cursor: 'pointer', '&:hover .bar': { opacity: 0.8 } }}>
                    <Box className="bar" sx={[{
                      position: 'absolute', left: '15%', right: '15%',
                      top: positive ? zeroTop - h : zeroTop,
                      height: h,
                      borderRadius: positive ? '4px 4px 0 0' : '0 0 4px 4px'
                    }, colorSx(positive)]} />
                    {labelled &&
                      <Typography variant="caption" sx={{
                        position: 'absolute', whiteSpace: 'nowrap', fontWeight: 700,
                        // Rótulos nas pontas do gráfico ficam ancorados para dentro, para não serem cortados.
                        ...(i === last ? { right: 0 } : i === 0 ? { left: 0 } : { left: '50%', transform: 'translateX(-50%)' }),
                        top: positive ? zeroTop - h - 20 : zeroTop + h + 2, color: 'text.secondary'
                      }}>
                        <Money value={p.accumulatedValue} size="xs" sx={{ color: 'inherit' }} />
                      </Typography>}
                    <Typography variant="caption" sx={{ position: 'absolute', bottom: 0, left: 0, right: 0, textAlign: 'center', color: 'text.secondary', fontSize: 11 }}>
                      {(Months[Number(m) - 1] || '').slice(0, 3)}{Number(m) === 1 ? `/${String(y).slice(2)}` : ''}
                    </Typography>
                  </Box>
                </Tooltip>
              )
            })}
          </Box>
        </Box>
      </Box>
    </Box>
  )
}
