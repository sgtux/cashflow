import React, { useState, useEffect, useMemo } from 'react'
import { useDispatch } from 'react-redux'
import { Box, Button, Card, Chip, Tooltip, Typography } from '@mui/material'
import ShoppingBasketRoundedIcon from '@mui/icons-material/ShoppingBasketRounded'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'

import { Page, MonthStepper, Money, EmptyState, ConfirmDialog, ActionIcon, SectionCard, ExpenseTypeAvatar, getExpenseTypeColor } from '../../components'
import { householdExpenseService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { Months, toast, sum } from '../../helpers'
import { EditHouseholdExpenseDialog } from './EditHouseholdExpenseDialog'

const weekDays = ['domingo', 'segunda', 'terça', 'quarta', 'quinta', 'sexta', 'sábado']

function CategoryBreakdown({ expenses, total }) {
  const categories = useMemo(() => {
    const map = {}
    expenses.forEach(e => {
      const key = e.type || 0
      if (!map[key])
        map[key] = { type: e.type, description: e.typeDescription || 'Outros', value: 0 }
      map[key].value += e.value
    })
    return Object.values(map).sort((a, b) => b.value - a.value)
  }, [expenses])

  if (!categories.length)
    return null

  const top = categories[0]

  return (
    <SectionCard title="Em que você mais gastou" sx={{ mb: 2.5 }}
      action={<Box sx={{ textAlign: 'right' }}>
        <Typography variant="caption" sx={{ color: 'text.secondary' }}>Total do mês</Typography>
        <Box><Money value={total} tone="out" bold size="lg" /></Box>
      </Box>}>
      <Box sx={{ display: 'flex', gap: '2px', height: 18, borderRadius: 99, overflow: 'hidden', bgcolor: 'action.hover' }}>
        {categories.map(c =>
          <Tooltip key={c.type || 0} title={`${c.description}: ${Math.round((c.value * 100) / total)}%`}>
            <Box sx={{ width: `${(c.value * 100) / total}%`, bgcolor: getExpenseTypeColor(c.type) }} />
          </Tooltip>
        )}
      </Box>
      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr', md: '1fr 1fr 1fr' }, gap: 1, mt: 2 }}>
        {categories.map(c =>
          <Box key={c.type || 0} sx={{ display: 'flex', alignItems: 'center', gap: 1.25 }}>
            <ExpenseTypeAvatar type={c.type} size={30} />
            <Box sx={{ flex: 1, minWidth: 0 }}>
              <Typography variant="body2" sx={{ fontWeight: 700 }}>{c.description}</Typography>
              <Typography variant="caption" sx={{ color: 'text.secondary' }}>{Math.round((c.value * 100) / total)}% do total</Typography>
            </Box>
            <Money value={c.value} size="sm" bold />
          </Box>
        )}
      </Box>
      <Typography variant="body2" sx={{ mt: 2, p: 1.5, borderRadius: 2, bgcolor: 'action.hover' }}>
        💡 <b>{top.description}</b> foi o que mais pesou este mês ({Math.round((top.value * 100) / total)}%).
        Se quiser economizar, é um bom lugar para começar.
      </Typography>
    </SectionCard>
  )
}

export function HouseholdExpenses() {
  const now = new Date()
  const [date, setDate] = useState({ month: now.getMonth() + 1, year: now.getFullYear() })
  const [expenses, setExpenses] = useState([])
  const [editExpense, setEditExpense] = useState(null)
  const [removeItem, setRemoveItem] = useState(null)
  const [loaded, setLoaded] = useState(false)

  const dispatch = useDispatch()

  useEffect(() => { refresh() }, [date])

  function refresh() {
    dispatch(showGlobalLoader())
    householdExpenseService.getAll(date.month, date.year)
      .then(res => setExpenses(res || []))
      .catch(err => console.log(err))
      .finally(() => {
        setLoaded(true)
        dispatch(hideGlobalLoader())
      })
  }

  function remove() {
    dispatch(showGlobalLoader())
    householdExpenseService.remove(removeItem.id)
      .then(() => {
        toast.success('Despesa removida.')
        setRemoveItem(null)
        refresh()
      })
      .catch(err => console.log(err))
      .finally(() => dispatch(hideGlobalLoader()))
  }

  const total = sum(expenses, p => p.value)

  const days = useMemo(() => {
    const map = {}
    expenses.forEach(p => {
      const day = new Date(p.date).getDate()
      if (!map[day])
        map[day] = []
      map[day].push(p)
    })
    return Object.keys(map).map(Number).sort((a, b) => b - a).map(day => ({ day, items: map[day] }))
  }, [expenses])

  return (
    <Page
      icon={<ShoppingBasketRoundedIcon />}
      title="Despesas do dia a dia"
      subtitle="Mercado, lanche, transporte, farmácia, lazer... tudo que é gasto variável."
      guide="expenses"
      actions={<>
        <MonthStepper month={date.month} year={date.year} minYear={now.getFullYear() - 4} maxYear={now.getFullYear()}
          onChange={(month, year) => setDate({ month, year })} />
        <Button variant="contained" startIcon={<AddRoundedIcon />} onClick={() => setEditExpense({})}>Anotar despesa</Button>
      </>}>

      <CategoryBreakdown expenses={expenses} total={total} />

      {days.length ?
        <Box sx={{ display: 'grid', gap: 2 }}>
          {days.map(({ day, items }) => {
            const weekDay = weekDays[new Date(date.year, date.month - 1, day).getDay()]
            return (
              <Box key={day}>
                <Box sx={{ display: 'flex', alignItems: 'baseline', justifyContent: 'space-between', px: 0.5, mb: 1 }}>
                  <Typography sx={{ fontWeight: 800 }}>
                    {day} de {Months[date.month - 1].toLowerCase()} <Box component="span" sx={{ color: 'text.secondary', fontWeight: 600 }}>· {weekDay}</Box>
                  </Typography>
                  <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                    no dia: <Money value={sum(items, p => p.value)} size="sm" bold />
                  </Typography>
                </Box>
                <Card>
                  {items.map((x, i) =>
                    <Box key={x.id} sx={{ display: 'flex', alignItems: 'center', gap: 1.5, px: 2, py: 1.25, borderTop: i ? 1 : 0, borderColor: 'divider', flexWrap: 'wrap' }}>
                      <ExpenseTypeAvatar type={x.type} />
                      <Box sx={{ flex: '1 1 180px', minWidth: 0 }}>
                        <Typography sx={{ fontWeight: 700 }}>{x.description}</Typography>
                        <Box sx={{ display: 'flex', gap: 1, alignItems: 'center', flexWrap: 'wrap' }}>
                          <Typography variant="caption" sx={{ color: 'text.secondary' }}>{x.typeDescription}</Typography>
                          {!!x.creditCardId &&
                            <Chip size="small" variant="outlined" icon={<CreditCardRoundedIcon />} label={x.creditCardName || 'Cartão'} sx={{ height: 20, fontSize: 11 }} />}
                        </Box>
                      </Box>
                      <Money value={x.value} tone="out" bold size="md" />
                      <Box>
                        <ActionIcon title="Editar" color="primary" onClick={() => setEditExpense(x)}><EditRoundedIcon /></ActionIcon>
                        <ActionIcon title="Remover" color="error" onClick={() => setRemoveItem(x)}><DeleteOutlineRoundedIcon /></ActionIcon>
                      </Box>
                    </Box>
                  )}
                </Card>
              </Box>
            )
          })}
        </Box>
        : loaded &&
        <EmptyState
          icon={<ShoppingBasketRoundedIcon />}
          title={`Nenhuma despesa anotada em ${Months[date.month - 1].toLowerCase()}`}
          text="Comece anotando o gasto mais recente que você lembra. Não precisa ser perfeito: o hábito vale mais que a precisão."
          actionLabel="Anotar uma despesa"
          onAction={() => setEditExpense({})} />
      }

      <ConfirmDialog open={!!removeItem} danger
        title="Remover despesa?"
        text={`"${(removeItem || {}).description || ''}" será apagada.`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
      <EditHouseholdExpenseDialog expense={editExpense}
        onClose={() => setEditExpense(null)}
        onSave={() => { setEditExpense(null); refresh() }} />
    </Page>
  )
}
