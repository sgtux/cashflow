import React, { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Box, Button, Card, IconButton, LinearProgress, Typography } from '@mui/material'
import CheckCircleRoundedIcon from '@mui/icons-material/CheckCircleRounded'
import RadioButtonUncheckedRoundedIcon from '@mui/icons-material/RadioButtonUncheckedRounded'
import CloseRoundedIcon from '@mui/icons-material/CloseRounded'
import FlagRoundedIcon from '@mui/icons-material/FlagRounded'

import { authService, householdExpenseService, recurringEarningService, recurringExpenseService, StorageService } from '../../services'

const GUIDE_KEY = 'first-steps'
export const PROJECTION_VISITED_KEY = 'projection-visited'

// Checklist de onboarding: mostra a ordem sugerida para quem está começando e some quando tudo foi feito.
export function FirstSteps() {
  const [hidden, setHidden] = useState(() => StorageService.isGuideDismissed(GUIDE_KEY))
  const [steps, setSteps] = useState(null)

  useEffect(() => {
    if (hidden)
      return
    const now = new Date()
    Promise.allSettled([
      recurringEarningService.getAll(false),
      recurringExpenseService.getAll(false),
      authService.getAccount(),
      householdExpenseService.getAll(now.getMonth() + 1, now.getFullYear())
    ]).then(([earnings, expenses, account, household]) => {
      const value = r => r.status === 'fulfilled' ? r.value : null
      const acc = value(account) || {}
      setSteps([
        { done: !!(value(earnings) || []).length, label: 'Cadastre seu salário (ou outro ganho que chega todo mês)', to: '/recurring-earnings', action: 'Cadastrar ganho fixo' },
        { done: !!(value(expenses) || []).length, label: 'Liste suas contas fixas: aluguel, internet, luz...', to: '/recurring-expenses', action: 'Cadastrar conta fixa' },
        { done: (acc.expenseLimit || 0) > 0, label: 'Defina quanto quer gastar por mês no dia a dia', to: '/account', action: 'Definir limite' },
        { done: !!(value(household) || []).length, label: 'Anote um gasto de hoje (mesmo que pequeno)', to: '/household-expenses', action: 'Anotar gasto' },
        { done: StorageService.isGuideDismissed(PROJECTION_VISITED_KEY), label: 'Veja como ficam os próximos meses', to: '/projection', action: 'Ver projeção' }
      ])
    })
  }, [hidden])

  if (hidden || !steps)
    return null

  const doneCount = steps.filter(p => p.done).length
  if (doneCount === steps.length)
    return null

  const next = steps.find(p => !p.done)

  function dismiss() {
    StorageService.setGuideDismissed(GUIDE_KEY, true)
    setHidden(true)
  }

  return (
    <Card sx={{ p: { xs: 2, sm: 3 }, mb: 3, borderColor: 'primary.main', borderWidth: 2 }}>
      <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 1.5 }}>
        <FlagRoundedIcon sx={{ color: 'primary.main', mt: 0.5 }} />
        <Box sx={{ flex: 1 }}>
          <Typography variant="h6">Primeiros passos</Typography>
          <Typography variant="body2" sx={{ color: 'text.secondary' }}>
            Siga esta ordem e em poucos minutos você terá uma visão real do seu dinheiro.
          </Typography>
        </Box>
        <IconButton size="small" aria-label="Esconder primeiros passos" onClick={dismiss}><CloseRoundedIcon /></IconButton>
      </Box>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mt: 2 }}>
        <LinearProgress variant="determinate" value={(doneCount * 100) / steps.length} sx={{ flex: 1, height: 10, borderRadius: 99 }} />
        <Typography variant="body2" sx={{ fontWeight: 800 }}>{doneCount} de {steps.length}</Typography>
      </Box>
      <Box component="ol" sx={{ listStyle: 'none', p: 0, m: 0, mt: 2, display: 'grid', gap: 1 }}>
        {steps.map((s, i) =>
          <Box component="li" key={i} sx={{
            display: 'flex', alignItems: 'center', gap: 1.5, p: 1.25, borderRadius: 2,
            bgcolor: s === next ? 'action.hover' : 'transparent'
          }}>
            {s.done ? <CheckCircleRoundedIcon color="success" /> : <RadioButtonUncheckedRoundedIcon sx={{ color: 'text.disabled' }} />}
            <Typography sx={{ flex: 1, textDecoration: s.done ? 'line-through' : 'none', color: s.done ? 'text.secondary' : 'text.primary' }}>
              <b>{i + 1}.</b> {s.label}
            </Typography>
            {s === next && <Button size="small" variant="contained" component={Link} to={s.to}>{s.action}</Button>}
          </Box>
        )}
      </Box>
    </Card>
  )
}
