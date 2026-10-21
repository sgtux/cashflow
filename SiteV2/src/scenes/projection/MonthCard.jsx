import React, { useMemo } from 'react'
import { Accordion, AccordionDetails, AccordionSummary, Box, Typography } from '@mui/material'
import ExpandMoreRoundedIcon from '@mui/icons-material/ExpandMoreRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'

import { Money } from '../../components'
import { getMonthYear, sum } from '../../helpers'

function PaymentLine({ p, tone }) {
  return (
    <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', gap: 1, px: 1.5, py: 0.75, borderRadius: 1.5, '&:nth-of-type(odd)': { bgcolor: 'action.hover' } }}>
      <Box sx={{ minWidth: 0 }}>
        <Typography variant="body2">{p.description}</Typography>
        {!!p.qtdInstallments &&
          <Typography variant="caption" sx={{ color: 'text.secondary' }}>
            parcela {p.number} de {p.qtdInstallments} · {p.qtdPaidInstallments} pagas
          </Typography>}
      </Box>
      <Money value={p.value} tone={tone || (p.in ? 'in' : 'out')} />
    </Box>
  )
}

export function MonthCard({ month, defaultExpanded }) {
  const { invoices, others } = useMemo(() => {
    const byCard = {}
    const rest = []
    month.payments.forEach(p => {
      if (p.creditCard && p.creditCard.id) {
        if (!byCard[p.creditCard.id])
          byCard[p.creditCard.id] = { name: p.creditCard.name, payments: [] }
        byCard[p.creditCard.id].payments.push(p)
      } else
        rest.push(p)
    })
    return { invoices: Object.values(byCard), others: rest }
  }, [month])

  const positive = month.accumulatedValue >= 0

  return (
    <Accordion defaultExpanded={defaultExpanded} disableGutters variant="outlined" sx={{ borderRadius: '14px !important', '&:before': { display: 'none' }, mb: 1.25 }}>
      <AccordionSummary expandIcon={<ExpandMoreRoundedIcon />}>
        <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr 1fr', sm: '1.2fr 1fr 1fr 1fr 1.1fr' }, gap: 1.5, width: '100%', alignItems: 'center', pr: 1 }}>
          <Typography sx={{ fontWeight: 800, gridColumn: { xs: '1 / -1', sm: 'auto' } }}>{getMonthYear(month.monthYear)}</Typography>
          <Box>
            <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block' }}>Ganhos</Typography>
            <Money value={month.totalIn} tone="in" />
          </Box>
          <Box>
            <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block' }}>Gastos</Typography>
            <Money value={month.totalOut} tone="out" />
          </Box>
          <Box>
            <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block' }}>Saldo do mês</Typography>
            <Money value={month.total} tone="auto" />
          </Box>
          <Box>
            <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block' }}>Acumulado</Typography>
            <Money value={month.accumulatedValue} tone="auto" bold size="md" />
            <Typography variant="caption" sx={{ display: 'block', color: positive ? 'success.main' : 'error.main', fontWeight: 700 }}>
              {positive ? '▲ sobrando' : '▼ faltando'}
            </Typography>
          </Box>
        </Box>
      </AccordionSummary>
      <AccordionDetails>
        {invoices.map((c, i) =>
          <Box key={i} sx={{ mb: 2 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 0.5 }}>
              <CreditCardRoundedIcon fontSize="small" sx={{ color: 'text.secondary' }} />
              <Typography sx={{ fontWeight: 800, flex: 1 }}>Fatura {c.name}</Typography>
              <Money value={sum(c.payments, p => p.value)} tone="out" bold />
            </Box>
            <Box sx={{ pl: { xs: 0, sm: 3 } }}>
              {c.payments.map((p, j) => <PaymentLine key={j} p={p} tone="out" />)}
            </Box>
          </Box>
        )}
        {!!others.length &&
          <Box>
            {!!invoices.length && <Typography sx={{ fontWeight: 800, mb: 0.5 }}>Outros lançamentos</Typography>}
            {others.map((p, j) => <PaymentLine key={j} p={p} />)}
          </Box>
        }
        {!month.payments.length && <Typography variant="body2" sx={{ color: 'text.secondary' }}>Nenhum lançamento previsto.</Typography>}
      </AccordionDetails>
    </Accordion>
  )
}
