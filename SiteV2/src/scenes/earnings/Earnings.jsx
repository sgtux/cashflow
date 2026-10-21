import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Link } from 'react-router-dom'
import { Alert, Box, Button, Card, MenuItem, TextField, Typography } from '@mui/material'
import TrendingUpRoundedIcon from '@mui/icons-material/TrendingUpRounded'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import ContentCopyRoundedIcon from '@mui/icons-material/ContentCopyRounded'

import { Page, Money, EmptyState, ConfirmDialog, ActionIcon } from '../../components'
import { earningService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { Months, toast, dateToString, isSameMonth, sum } from '../../helpers'
import { EditEarningDialog } from './EditEarningDialog'

// Últimos 12 meses, do mais antigo para o atual (mesmo filtro "Desde" do Site).
function buildMonthOptions() {
  const now = new Date()
  const list = []
  for (let i = 11; i >= 0; i--) {
    const d = new Date(now.getFullYear(), now.getMonth() - i, 1)
    list.push({ value: `${d.getMonth() + 1}/${d.getFullYear()}`, label: `${Months[d.getMonth()]} de ${d.getFullYear()}`, date: d })
  }
  return list
}

const monthOptions = buildMonthOptions()

export function Earnings() {
  const [earnings, setEarnings] = useState([])
  const [since, setSince] = useState(monthOptions[monthOptions.length - 1].value)
  const [editEarning, setEditEarning] = useState(null)
  const [removeItem, setRemoveItem] = useState(null)

  const dispatch = useDispatch()

  useEffect(() => { refresh() }, [since])

  async function refresh() {
    const option = monthOptions.find(p => p.value === since)
    try {
      dispatch(showGlobalLoader())
      setEarnings(await earningService.getAll({ fromDate: option.date }) || [])
    } catch (ex) {
      console.log(ex)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  async function remove() {
    dispatch(showGlobalLoader())
    try {
      await earningService.remove(removeItem.id)
      toast.success('Ganho removido.')
      setRemoveItem(null)
      refresh()
    } catch (ex) {
      console.log(ex)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  async function copyToCurrentMonth(earning) {
    const original = new Date(earning.date)
    const now = new Date()
    const date = new Date(now.getFullYear(), now.getMonth(), original.getDate())
    dispatch(showGlobalLoader())
    try {
      await earningService.save({ ...earning, id: 0, date })
      toast.success('Ganho copiado para este mês.')
      await refresh()
    } catch (ex) {
      console.log(ex)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  const total = sum(earnings, p => p.value)

  return (
    <Page
      icon={<TrendingUpRoundedIcon />}
      title="Ganhos"
      subtitle="Dinheiro que entrou de forma avulsa: freelas, vendas, bônus, presentes."
      guide="earnings"
      actions={<Button variant="contained" startIcon={<AddRoundedIcon />} onClick={() => setEditEarning({})}>Registrar ganho</Button>}>

      <Card sx={{ p: 2, mb: 2, display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap' }}>
        <TextField select label="Mostrar ganhos desde" value={since} onChange={e => setSince(e.target.value)} sx={{ maxWidth: 260 }} size="small">
          {monthOptions.map(p => <MenuItem key={p.value} value={p.value}>{p.label}</MenuItem>)}
        </TextField>
        <Box sx={{ flex: 1 }} />
        <Box sx={{ textAlign: 'right' }}>
          <Typography variant="caption" sx={{ color: 'text.secondary' }}>Total no período</Typography>
          <Box><Money value={total} tone="in" size="lg" bold /></Box>
        </Box>
      </Card>

      <Alert severity="info" sx={{ mb: 2 }}>
        O salário entra todo mês? Cadastre em <Link to="/recurring-earnings">Ganhos fixos</Link> para não precisar lançar de novo.
      </Alert>

      {earnings.length ?
        <Box sx={{ display: 'grid', gap: 1.25 }}>
          {earnings.map(p =>
            <Card key={p.id} sx={{ p: 1.5, pl: 2, display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
              <Box sx={{ flex: '1 1 220px', minWidth: 0 }}>
                <Typography sx={{ fontWeight: 700 }}>{p.description}</Typography>
                <Typography variant="body2" sx={{ color: 'text.secondary' }}>Recebido em {dateToString(p.date)}</Typography>
              </Box>
              <Money value={p.value} tone="in" size="md" bold />
              <Box>
                {!isSameMonth(new Date(), p.date) &&
                  <ActionIcon title="Repetir este ganho no mês atual" color="primary" onClick={() => copyToCurrentMonth(p)}>
                    <ContentCopyRoundedIcon />
                  </ActionIcon>
                }
                <ActionIcon title="Editar" color="primary" onClick={() => setEditEarning(p)}><EditRoundedIcon /></ActionIcon>
                <ActionIcon title="Remover" color="error" onClick={() => setRemoveItem(p)}><DeleteOutlineRoundedIcon /></ActionIcon>
              </Box>
            </Card>
          )}
        </Box>
        :
        <EmptyState
          icon={<TrendingUpRoundedIcon />}
          title="Nenhum ganho neste período"
          text="Fez um trabalho extra, vendeu algo ou ganhou um bônus? Registre aqui para saber exatamente quanto entrou."
          actionLabel="Registrar meu primeiro ganho"
          onAction={() => setEditEarning({})} />
      }

      <ConfirmDialog open={!!removeItem} danger
        title="Remover ganho?"
        text={`"${(removeItem || {}).description}" deixará de contar nas suas entradas.`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
      <EditEarningDialog earning={editEarning}
        onSave={() => { setEditEarning(null); refresh() }}
        onClose={() => setEditEarning(null)} />
    </Page>
  )
}
