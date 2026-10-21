import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Link, useNavigate } from 'react-router-dom'
import { Box, Button, Card, Chip, InputAdornment, LinearProgress, TextField, ToggleButton, ToggleButtonGroup, Typography } from '@mui/material'
import ViewWeekRoundedIcon from '@mui/icons-material/ViewWeekRounded'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import SearchRoundedIcon from '@mui/icons-material/SearchRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'

import { Page, Money, EmptyState, ConfirmDialog, ActionIcon, StatusChip, DateField, Term } from '../../components'
import { paymentService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { toast } from '../../helpers'

// status -> parâmetro "done" da API: em andamento = false, concluídos = true, todos = sem filtro.
const doneByStatus = { inProgress: false, done: true, all: undefined }

export function Payments() {
  const [payments, setPayments] = useState([])
  const [description, setDescription] = useState('')
  const [status, setStatus] = useState('inProgress')
  const [startDate, setStartDate] = useState(null)
  const [endDate, setEndDate] = useState(null)
  const [removeItem, setRemoveItem] = useState(null)
  const [loaded, setLoaded] = useState(false)

  const dispatch = useDispatch()
  const navigate = useNavigate()

  useEffect(() => {
    // Espera a pessoa parar de digitar antes de buscar.
    const timer = setTimeout(refresh, description ? 600 : 0)
    return () => clearTimeout(timer)
  }, [description, status, startDate, endDate])

  async function refresh() {
    try {
      dispatch(showGlobalLoader())
      setPayments(await paymentService.getAll({ description, done: doneByStatus[status], startDate, endDate }) || [])
    } catch (err) {
      console.log(err)
    } finally {
      setLoaded(true)
      dispatch(hideGlobalLoader())
    }
  }

  async function remove() {
    dispatch(showGlobalLoader())
    try {
      await paymentService.remove(removeItem.id)
      toast.success('Parcelamento removido.')
      setRemoveItem(null)
      await refresh()
    } catch (err) {
      console.log(err)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  function clearFilters() {
    setDescription('')
    setStatus('inProgress')
    setStartDate(null)
    setEndDate(null)
  }

  const hasFilters = description || status !== 'inProgress' || startDate || endDate

  return (
    <Page
      icon={<ViewWeekRoundedIcon />}
      title="Parcelamentos"
      subtitle="Compras e empréstimos divididos em parcelas mensais."
      guide="payments"
      actions={<Button variant="contained" startIcon={<AddRoundedIcon />} component={Link} to="/edit-payment/0">Novo parcelamento</Button>}>

      <Card sx={{ p: 2, mb: 2.5, display: 'grid', gap: 2, gridTemplateColumns: { xs: '1fr', md: '2fr 1fr 1fr' } }}>
        <TextField size="small" label="Buscar pelo nome" value={description} onChange={e => setDescription(e.target.value)}
          slotProps={{ input: { startAdornment: <InputAdornment position="start"><SearchRoundedIcon /></InputAdornment> } }} />
        <DateField label="Comprado desde" value={startDate} onChange={setStartDate} clearable />
        <DateField label="Até" value={endDate} onChange={setEndDate} clearable />
        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', flexWrap: 'wrap', gridColumn: '1 / -1' }}>
          <ToggleButtonGroup size="small" exclusive value={status} onChange={(e, v) => v && setStatus(v)} color="primary">
            <ToggleButton value="inProgress">Em andamento</ToggleButton>
            <ToggleButton value="done">Já quitados</ToggleButton>
            <ToggleButton value="all">Todos</ToggleButton>
          </ToggleButtonGroup>
          <Box sx={{ flex: 1 }} />
          {hasFilters && <Button onClick={clearFilters}>Limpar filtros</Button>}
        </Box>
      </Card>

      {payments.length ?
        <Box sx={{ display: 'grid', gap: 1.25 }}>
          {payments.map(p => {
            const count = (p.installments || []).length
            const progress = count ? (p.paidInstallments * 100) / count : 0
            const left = count - p.paidInstallments
            return (
              <Card key={p.id} sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', gap: 2, alignItems: 'flex-start', flexWrap: 'wrap' }}>
                  <Box sx={{ flex: '1 1 240px', minWidth: 0 }}>
                    <Typography sx={{ fontWeight: 800 }}>{p.description}</Typography>
                    <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap', mt: 0.5, alignItems: 'center' }}>
                      <Chip size="small" label={(p.type || {}).description} color={(p.type || {}).in ? 'success' : 'default'} variant="outlined" />
                      {p.creditCardText && <Chip size="small" icon={<CreditCardRoundedIcon />} label={p.creditCardText} variant="outlined" />}
                      {p.done
                        ? <StatusChip status="done" label="Quitado" />
                        : p.currentMonthPaid ? <StatusChip status="done" label="Parcela do mês paga" /> : null}
                    </Box>
                  </Box>
                  <Box sx={{ textAlign: 'right' }}>
                    <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                      {count}x de <Money value={p.installmentValue} bold />
                    </Typography>
                    <Typography variant="caption" sx={{ color: 'text.secondary' }}>total <Money value={p.total} size="xs" /></Typography>
                  </Box>
                  <Box>
                    <ActionIcon title="Ver e editar parcelas" color="primary" onClick={() => navigate(`/edit-payment/${p.id}`)}><EditRoundedIcon /></ActionIcon>
                    <ActionIcon title="Remover" color="error" onClick={() => setRemoveItem(p)}><DeleteOutlineRoundedIcon /></ActionIcon>
                  </Box>
                </Box>
                <Box sx={{ mt: 1.5 }}>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', mb: 0.5, flexWrap: 'wrap', gap: 1 }}>
                    <Typography variant="caption" sx={{ fontWeight: 700 }}>
                      {p.paidInstallments} de {count} <Term id="parcela">parcelas</Term> pagas
                      {!p.done && left > 0 && <Box component="span" sx={{ color: 'text.secondary', fontWeight: 600 }}> · faltam {left}</Box>}
                    </Typography>
                    <Typography variant="caption" sx={{ color: 'text.secondary' }}>{p.firstPaymentDate} → {p.lastPaymentDate}</Typography>
                  </Box>
                  <LinearProgress variant="determinate" value={progress} color={p.done ? 'success' : 'primary'} sx={{ height: 8, borderRadius: 99, bgcolor: 'action.hover' }} />
                </Box>
              </Card>
            )
          })}
        </Box>
        : loaded &&
        <EmptyState
          icon={<ViewWeekRoundedIcon />}
          title={hasFilters ? 'Nada encontrado com esses filtros' : 'Nenhum parcelamento em andamento'}
          text={hasFilters ? 'Tente limpar os filtros ou buscar por outro nome.' : 'Comprou algo em várias vezes ou fez um empréstimo? Cadastre aqui e veja quando cada parcela vence.'}
          actionLabel={hasFilters ? 'Limpar filtros' : 'Cadastrar parcelamento'}
          onAction={hasFilters ? clearFilters : () => navigate('/edit-payment/0')} />
      }

      <ConfirmDialog open={!!removeItem} danger
        title="Remover parcelamento?"
        text={`"${(removeItem || {}).description || ''}" e todas as suas parcelas serão apagados.`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
    </Page>
  )
}
