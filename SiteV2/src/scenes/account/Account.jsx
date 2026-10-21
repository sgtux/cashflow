import React, { useState, useEffect } from 'react'
import { Box, Button, Card, Chip, Grid2 as Grid, LinearProgress, Typography } from '@mui/material'
import TuneRoundedIcon from '@mui/icons-material/TuneRounded'

import { Page, MoneyField, SectionCard } from '../../components'
import { authService } from '../../services'
import { toast, toThousandFormat } from '../../helpers'

const plans = [
  { id: 1, name: 'Free', price: 'R$ 0,00', records: 10000, image: './images/bronze-medal.svg' },
  { id: 2, name: 'Basic', price: 'R$ 5,99', records: 100000, image: './images/silver-medal.svg' },
  { id: 3, name: 'Premium', price: 'R$ 19,99', records: 1000000, image: './images/gold-medal.svg' }
]

export function Account() {
  const [loading, setLoading] = useState(false)
  const [email, setEmail] = useState('')
  const [expenseLimit, setExpenseLimit] = useState(0)
  const [fuelExpenseLimit, setFuelExpenseLimit] = useState(0)
  const [plan, setPlan] = useState(1)
  const [recordsUsed, setRecordsUsed] = useState(0)

  useEffect(() => {
    setLoading(true)
    authService.getAccount()
      .then(res => {
        setEmail(res.email)
        setExpenseLimit(res.expenseLimit || 0)
        setFuelExpenseLimit(res.fuelExpenseLimit || 0)
        setPlan(res.plan)
        setRecordsUsed(res.recordsUsed || 0)
      })
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }, [])

  function save() {
    setLoading(true)
    authService.update({ expenseLimit, fuelExpenseLimit })
      .then(() => toast.success('Limites salvos!'))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  // Mesma regra do Site: planos desconhecidos são tratados como Premium.
  const currentPlan = plans.find(p => p.id === plan) || plans[2]
  const usage = (recordsUsed * 100) / currentPlan.records

  return (
    <Page icon={<TuneRoundedIcon />} title="Minha conta e limites" subtitle={email} guide="account" loading={loading} maxWidth={960}>
      <Grid container spacing={2}>
        <Grid size={{ xs: 12, md: 7 }}>
          <SectionCard title="Limites de gastos por mês" help="limite">
            <Box sx={{ display: 'grid', gap: 2.5, mt: 1 }}>
              <MoneyField label="Despesas do dia a dia" value={expenseLimit} onChange={setExpenseLimit}
                helperText="Mercado, lanches, lazer, transporte... Quanto você aceita gastar no mês?" />
              <MoneyField label="Combustível" value={fuelExpenseLimit} onChange={setFuelExpenseLimit}
                helperText="Quanto pretende gastar abastecendo por mês. Deixe zerado se não tem veículo." />
            </Box>
            <Box sx={{ textAlign: 'right', mt: 2.5 }}>
              <Button variant="contained" onClick={save} disabled={loading}>Salvar limites</Button>
            </Box>
          </SectionCard>
        </Grid>
        <Grid size={{ xs: 12, md: 5 }}>
          <SectionCard title="Uso do seu plano">
            <Typography sx={{ fontWeight: 800, fontSize: 22 }}>{toThousandFormat(recordsUsed) || 0}</Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary' }}>de {toThousandFormat(currentPlan.records)} registros do plano {currentPlan.name}</Typography>
            <LinearProgress variant="determinate" value={Math.min(100, usage)} sx={{ height: 10, borderRadius: 99, mt: 1.5 }} />
            <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block', mt: 1 }}>
              Cada ganho, despesa, parcela ou abastecimento conta como um registro.
            </Typography>
          </SectionCard>
        </Grid>
        {plans.map(p =>
          <Grid key={p.id} size={{ xs: 12, sm: 4 }}>
            <Card sx={{ p: 2.5, textAlign: 'center', height: '100%', borderWidth: p.id === currentPlan.id ? 2 : 1, borderColor: p.id === currentPlan.id ? 'primary.main' : 'divider' }}>
              <img height={48} src={p.image} alt="" />
              <Typography variant="h6" sx={{ mt: 1 }}>{p.name}</Typography>
              <Typography sx={{ fontWeight: 800, fontSize: 22 }}>{p.price}</Typography>
              <Typography variant="body2" sx={{ color: 'text.secondary' }}>{toThousandFormat(p.records)} registros</Typography>
              {p.id === currentPlan.id && <Chip color="primary" label="Seu plano" size="small" sx={{ mt: 1.5 }} />}
            </Card>
          </Grid>
        )}
      </Grid>
    </Page>
  )
}
