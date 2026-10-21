import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Alert, Box, Card, Typography } from '@mui/material'
import QueryStatsRoundedIcon from '@mui/icons-material/QueryStatsRounded'

import { Page, Money, SectionCard, EmptyState, HelpTip, Term } from '../../components'
import { homeService, StorageService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { getMonthYear } from '../../helpers'
import { PROJECTION_VISITED_KEY } from '../home/FirstSteps'
import { BalanceChart } from './BalanceChart'
import { MonthCard } from './MonthCard'

export function Projection() {
  const [months, setMonths] = useState([])
  const [loaded, setLoaded] = useState(false)

  const dispatch = useDispatch()

  useEffect(() => {
    StorageService.setGuideDismissed(PROJECTION_VISITED_KEY, true)
    dispatch(showGlobalLoader())
    homeService.getProjection()
      .then(res => {
        const list = res || []
        list.forEach(item => item.payments.sort((a, b) => (a.description || '').localeCompare(b.description || '')))
        setMonths(list)
      })
      .catch(err => console.log(err))
      .finally(() => {
        setLoaded(true)
        dispatch(hideGlobalLoader())
      })
  }, [])

  // Mesmo cálculo do Site: soma dos saldos mensais mais o saldo trazido do mês anterior.
  const total = months.reduce((acc, p) => acc + p.total + (p.previousMonthBalanceValue || 0), 0)
  const firstNegative = months.find(p => p.accumulatedValue < 0)
  const lastMonth = months[months.length - 1]

  return (
    <Page
      icon={<QueryStatsRoundedIcon />}
      title="Projeção dos próximos meses"
      subtitle="Uma previsão feita a partir de tudo que você cadastrou: ganhos, contas fixas, parcelas e limites."
      guide="projection">

      {months.length ?
        <>
          {firstNegative
            ? <Alert severity="warning" sx={{ mb: 2 }}>
              Atenção: em <b>{getMonthYear(firstNegative.monthYear)}</b> o <Term id="acumulado">saldo acumulado</Term> fica negativo.
              Guardar um pouco a cada mês a partir de agora ajuda a cobrir essa diferença.
            </Alert>
            : <Alert severity="success" sx={{ mb: 2 }}>
              Boa notícia: pelo que está cadastrado, seu saldo acumulado se mantém positivo em todos os meses.
            </Alert>
          }

          <Box sx={{ display: 'grid', gap: 2, gridTemplateColumns: { xs: '1fr', md: '2fr 1fr' }, mb: 3 }}>
            <SectionCard title="Como seu saldo acumulado evolui" help="acumulado">
              <BalanceChart months={months} />
            </SectionCard>
            <Card sx={{ p: 2.5, display: 'flex', flexDirection: 'column', justifyContent: 'center', borderTop: 4, borderTopColor: total >= 0 ? 'success.main' : 'error.main' }}>
              <Typography variant="body2" sx={{ fontWeight: 700, color: 'text.secondary' }}>
                Total acumulado até {lastMonth ? getMonthYear(lastMonth.monthYear) : ''}<HelpTip id="projecao" />
              </Typography>
              <Box sx={{ my: 1 }}><Money value={total} tone="auto" size="xxl" bold /></Box>
              <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                {total >= 0
                  ? 'É quanto deve sobrar se tudo correr como planejado. Que tal transformar parte disso em reserva de emergência?'
                  : 'É quanto deve faltar no fim do período. Revise gastos variáveis e evite novas parcelas.'}
              </Typography>
            </Card>
          </Box>

          <Typography variant="h6" sx={{ mb: 1 }}>Mês a mês</Typography>
          <Typography variant="body2" sx={{ color: 'text.secondary', mb: 2 }}>
            Abra um mês para ver cada lançamento. Gastos ainda não lançados usam seu limite como estimativa (<Term id="residual">valor residual</Term>).
          </Typography>
          {months.map((p, i) => <MonthCard key={p.monthYear || i} month={p} defaultExpanded={i === 0} />)}
        </>
        : loaded &&
        <EmptyState icon={<QueryStatsRoundedIcon />} title="Ainda não há o que projetar"
          text="Cadastre seus ganhos fixos e contas fixas para ver como ficam os próximos meses." />
      }
    </Page>
  )
}
