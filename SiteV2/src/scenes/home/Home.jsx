import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Link } from 'react-router-dom'
import { Box, Button, Card, Chip, Grid2 as Grid, LinearProgress, Typography } from '@mui/material'
import HomeRoundedIcon from '@mui/icons-material/HomeRounded'
import ArrowDownwardRoundedIcon from '@mui/icons-material/ArrowDownwardRounded'
import ArrowUpwardRoundedIcon from '@mui/icons-material/ArrowUpwardRounded'
import AccountBalanceWalletRoundedIcon from '@mui/icons-material/AccountBalanceWalletRounded'
import WarningAmberRoundedIcon from '@mui/icons-material/WarningAmberRounded'
import CelebrationRoundedIcon from '@mui/icons-material/CelebrationRounded'
import SentimentDissatisfiedRoundedIcon from '@mui/icons-material/SentimentDissatisfiedRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'
import TaskAltRoundedIcon from '@mui/icons-material/TaskAltRounded'
import DirectionsRunRoundedIcon from '@mui/icons-material/DirectionsRunRounded'

import { Page, MonthStepper, StatCard, SectionCard, Money, LimitMeter, Term } from '../../components'
import { homeService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { Months, sum } from '../../helpers'
import { FirstSteps } from './FirstSteps'

function Verdict({ inflows, outflows, monthName }) {
  const balance = inflows - outflows
  const usage = inflows > 0 ? Math.round((outflows * 100) / inflows) : null
  const positive = balance >= 0

  return (
    <Card sx={{
      p: { xs: 2.5, sm: 3 }, mb: 3,
      borderColor: positive ? 'success.main' : 'error.main',
      borderWidth: 2
    }}>
      <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap' }}>
        <Box sx={{ color: positive ? 'success.main' : 'error.main', '& svg': { fontSize: 44 } }}>
          {positive ? <CelebrationRoundedIcon /> : <SentimentDissatisfiedRoundedIcon />}
        </Box>
        <Box sx={{ flex: '1 1 300px' }}>
          <Typography sx={{ fontSize: { xs: 17, sm: 20 }, fontWeight: 700, lineHeight: 1.4 }}>
            Em {monthName}, entram <Money value={inflows} tone="in" size="inherit" bold /> e
            saem <Money value={outflows} tone="out" size="inherit" bold />.
          </Typography>
          <Typography sx={{ fontSize: { xs: 17, sm: 20 }, fontWeight: 800, color: positive ? 'success.main' : 'error.main' }}>
            {positive
              ? <>Sobram <Money value={balance} tone="in" size="inherit" bold />. Muito bem!</>
              : <>Faltam <Money value={-balance} tone="out" size="inherit" bold />. Hora de rever alguns gastos.</>}
          </Typography>
        </Box>
      </Box>
      {usage !== null &&
        <Box sx={{ mt: 2.5 }}>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 0.5 }}>
            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
              Você está comprometendo <b>{usage}%</b> do que ganha
            </Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary', display: { xs: 'none', sm: 'block' } }}>
              ideal: até 80–90%, para sobrar algo para guardar
            </Typography>
          </Box>
          <LinearProgress variant="determinate" value={Math.min(usage, 100)}
            color={usage <= 80 ? 'success' : usage <= 100 ? 'warning' : 'error'}
            sx={{ height: 12, borderRadius: 99, bgcolor: 'action.hover' }} />
        </Box>
      }
    </Card>
  )
}

function RatRace({ passiveInflows, livingCosts, coverage, outOfRatRace }) {
  const percent = Math.round(coverage * 100)
  const gap = livingCosts - passiveInflows

  return (
    <SectionCard title="Corrida dos ratos" help="corridaDosRatos"
      icon={<DirectionsRunRoundedIcon sx={{ color: outOfRatRace ? 'success.main' : 'warning.main' }} />}
      action={<Typography sx={{ fontWeight: 800, fontSize: 20, color: outOfRatRace ? 'success.main' : 'text.primary' }}>{percent}%</Typography>}>
      <Typography sx={{ mb: 1.5 }}>
        {outOfRatRace
          ? <>Sua <Term id="rendaPassiva">renda passiva</Term> já paga todo o seu custo de vida do mês. Você saiu da corrida dos ratos!</>
          : passiveInflows > 0
            ? <>Sua <Term id="rendaPassiva">renda passiva</Term> de <Money value={passiveInflows} tone="in" size="inherit" bold /> cobre {percent}% do seu custo de vida (as saídas, sem contar os aportes).
              Faltam <Money value={gap} tone="out" size="inherit" bold /> por mês para sair da corrida.</>
            : <>Nenhuma <Term id="rendaPassiva">renda passiva</Term> neste mês. Aluguéis, dividendos e juros que você recebe aparecem aqui.</>}
      </Typography>
      <LinearProgress variant="determinate" value={Math.min(percent, 100)}
        color={outOfRatRace ? 'success' : 'warning'}
        sx={{ height: 12, borderRadius: 99, bgcolor: 'action.hover' }} />
    </SectionCard>
  )
}

function ShareList({ items, total, tone, empty }) {
  if (!items.length)
    return <Typography sx={{ color: 'text.secondary', py: 2 }}>{empty}</Typography>
  return (
    <Box sx={{ display: 'grid', gap: 1.5 }}>
      {items.map((p, i) => {
        const share = total > 0 ? (p.value * 100) / total : 0
        return (
          <Box key={i}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', gap: 1 }}>
              <Typography sx={{ fontWeight: 600, minWidth: 0, overflow: 'hidden', textOverflow: 'ellipsis' }}>{p.description}</Typography>
              <Money value={p.value} tone={tone} bold />
            </Box>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mt: 0.5 }}>
              <LinearProgress variant="determinate" value={share} color={tone === 'in' ? 'success' : 'error'}
                sx={{ flex: 1, height: 6, borderRadius: 99, bgcolor: 'action.hover' }} />
              <Typography variant="caption" sx={{ color: 'text.secondary', width: 38, textAlign: 'right' }}>{Math.round(share)}%</Typography>
            </Box>
          </Box>
        )
      })}
    </Box>
  )
}

export function Home() {
  const now = new Date()
  const [date, setDate] = useState({ month: now.getMonth() + 1, year: now.getFullYear() })
  const [data, setData] = useState({ pendingPayments: [], inflows: [], outflows: [], limitValues: [], totalInflows: 0, totalOutflows: 0, totalPassiveInflows: 0, totalLivingCosts: 0, passiveIncomeCoverage: 0, outOfRatRace: false })

  const dispatch = useDispatch()

  useEffect(() => { refresh() }, [date])

  async function refresh() {
    dispatch(showGlobalLoader())
    try {
      const res = await homeService.getChart(date.month, date.year)
      setData({
        pendingPayments: res.pendingPayments || [],
        inflows: res.inflows || [],
        outflows: res.outflows || [],
        limitValues: res.limitValues || [],
        totalInflows: res.totalInflows || 0,
        totalOutflows: res.totalOutflows || 0,
        totalPassiveInflows: res.totalPassiveInflows || 0,
        totalLivingCosts: res.totalLivingCosts || 0,
        passiveIncomeCoverage: res.passiveIncomeCoverage || 0,
        outOfRatRace: !!res.outOfRatRace
      })
    } catch (err) {
      console.log(err)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  const pendingTotal = sum(data.pendingPayments, p => p.value)
  const monthName = Months[date.month - 1].toLowerCase()
  const hasLimits = data.limitValues.some(p => p.limit > 0)

  return (
    <Page
      icon={<HomeRoundedIcon />}
      title="Seu mês em resumo"
      subtitle="Quanto entra, quanto sai e o que ainda falta pagar."
      guide="home"
      actions={<MonthStepper month={date.month} year={date.year} minYear={now.getFullYear() - 1} maxYear={now.getFullYear() + 1}
        onChange={(month, year) => setDate({ month, year })} />}>

      <FirstSteps />

      <Verdict inflows={data.totalInflows} outflows={data.totalOutflows} monthName={monthName} />

      <Grid container spacing={2} sx={{ mb: 3 }}>
        <Grid size={{ xs: 12, sm: 4 }}>
          <StatCard label="Entradas" help="entradas" tone="in" value={data.totalInflows}
            icon={<ArrowDownwardRoundedIcon fontSize="small" />} caption="Tudo que entra no mês" />
        </Grid>
        <Grid size={{ xs: 12, sm: 4 }}>
          <StatCard label="Saídas" help="saidas" tone="out" value={data.totalOutflows}
            icon={<ArrowUpwardRoundedIcon fontSize="small" />} caption="Tudo que sai no mês" />
        </Grid>
        <Grid size={{ xs: 12, sm: 4 }}>
          <StatCard label="Saldo do mês" help="liquido" tone={data.totalInflows - data.totalOutflows >= 0 ? 'in' : 'out'}
            value={data.totalInflows - data.totalOutflows}
            icon={<AccountBalanceWalletRoundedIcon fontSize="small" />} caption="Entradas − saídas" />
        </Grid>
      </Grid>

      <Box sx={{ mb: 3 }}>
        <RatRace passiveInflows={data.totalPassiveInflows} livingCosts={data.totalLivingCosts}
          coverage={data.passiveIncomeCoverage} outOfRatRace={data.outOfRatRace} />
      </Box>

      <Grid container spacing={2}>
        <Grid size={{ xs: 12, md: 6 }}>
          <SectionCard title="De onde vem o dinheiro" help="entradas">
            <ShareList items={data.inflows} total={data.totalInflows} tone="in"
              empty={<>Nenhum ganho neste mês. <Link to="/recurring-earnings">Cadastre seu salário</Link> ou <Link to="/earnings">um ganho avulso</Link>.</>} />
          </SectionCard>
        </Grid>
        <Grid size={{ xs: 12, md: 6 }}>
          <SectionCard title="Para onde vai o dinheiro" help="saidas">
            <ShareList items={data.outflows} total={data.totalOutflows} tone="out"
              empty="Nenhuma saída neste mês ainda." />
          </SectionCard>
        </Grid>

        <Grid size={{ xs: 12, md: 6 }}>
          <SectionCard
            title="Contas a pagar este mês"
            help="pendencia"
            icon={<WarningAmberRoundedIcon sx={{ color: data.pendingPayments.length ? 'warning.main' : 'success.main' }} />}
            action={!!data.pendingPayments.length && <Money value={pendingTotal} tone="out" bold size="md" />}>
            {data.pendingPayments.length === 0 ?
              <Box sx={{ display: 'flex', gap: 1, alignItems: 'center', color: 'success.main', py: 2 }}>
                <TaskAltRoundedIcon />
                <Typography sx={{ fontWeight: 700 }}>Tudo em dia! Nenhuma conta pendente.</Typography>
              </Box>
              :
              <Box sx={{ display: 'grid', gap: 1 }}>
                {data.pendingPayments.map((p, i) =>
                  <Box key={i} sx={{ display: 'flex', alignItems: 'center', gap: 1.5, p: 1.25, borderRadius: 2, bgcolor: 'action.hover' }}>
                    <WarningAmberRoundedIcon sx={{ color: 'warning.main' }} />
                    <Box sx={{ flex: 1, minWidth: 0 }}>
                      <Typography sx={{ fontWeight: 600 }}>{p.description}</Typography>
                      {(p.creditCard || p.creditCardName) &&
                        <Chip size="small" icon={<CreditCardRoundedIcon />} variant="outlined" sx={{ mt: 0.5 }}
                          label={(p.creditCard || {}).name || p.creditCardName || 'Cartão'} />}
                    </Box>
                    <Money value={p.value} tone="out" bold />
                  </Box>
                )}
                <Typography variant="caption" sx={{ color: 'text.secondary', mt: 0.5 }}>
                  Já pagou alguma? Marque como paga em <Link to="/recurring-expenses">Contas fixas</Link> ou <Link to="/payments">Parcelamentos</Link>.
                </Typography>
              </Box>
            }
          </SectionCard>
        </Grid>
        <Grid size={{ xs: 12, md: 6 }}>
          <SectionCard title="Seus limites do mês" help="limite">
            {data.limitValues.map((p, i) =>
              <LimitMeter key={i} description={p.description} spent={p.spent} limit={p.limit} />
            )}
            {!hasLimits &&
              <Box sx={{ mt: 1, p: 2, borderRadius: 2, bgcolor: 'action.hover' }}>
                <Typography variant="body2">
                  Um <Term id="limite">limite</Term> é o teto que você define para gastar no mês. Ele ajuda a perceber cedo quando os gastos estão passando do ponto.
                </Typography>
                <Button component={Link} to="/account" size="small" variant="outlined" sx={{ mt: 1.5 }}>Definir meus limites</Button>
              </Box>
            }
          </SectionCard>
        </Grid>
      </Grid>
    </Page>
  )
}
