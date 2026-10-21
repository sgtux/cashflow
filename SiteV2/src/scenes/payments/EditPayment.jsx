import React, { useState, useEffect } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'
import {
  Alert, Box, Button, Card, Chip, MenuItem, Table, TableBody, TableCell, TableHead, TableRow, TextField, Typography
} from '@mui/material'
import ViewWeekRoundedIcon from '@mui/icons-material/ViewWeekRounded'
import ArrowBackRoundedIcon from '@mui/icons-material/ArrowBackRounded'
import AutorenewRoundedIcon from '@mui/icons-material/AutorenewRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import CheckRoundedIcon from '@mui/icons-material/CheckRounded'
import DoNotDisturbOnRoundedIcon from '@mui/icons-material/DoNotDisturbOnRounded'

import { Page, Money, MoneyField, DateField, SelectField, ActionIcon, StatusChip, HelpTip } from '../../components'
import { paymentService, creditCardService } from '../../services'
import { toast, dateToString, sum } from '../../helpers'
import { EditInstallmentDialog } from './EditInstallmentDialog'

function Step({ number, title, help, children }) {
  return (
    <Card sx={{ p: { xs: 2, sm: 3 } }}>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 2 }}>
        <Box sx={{ width: 30, height: 30, borderRadius: '50%', bgcolor: 'secondary.main', color: 'secondary.contrastText', fontWeight: 800, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          {number}
        </Box>
        <Typography variant="h6">{title}{help && <HelpTip id={help} />}</Typography>
      </Box>
      {children}
    </Card>
  )
}

function installmentStatus(p) {
  if (p.exempt)
    return <StatusChip status="exempt" label="Isenta" />
  if (p.paidDate)
    return <StatusChip status="done" label="Paga" />
  return <StatusChip status="pending" label="A pagar" />
}

export function EditPayment() {
  const params = useParams()
  const navigate = useNavigate()
  const paramId = Number(params.id) || 0

  const [id, setId] = useState(0)
  const [description, setDescription] = useState('')
  const [type, setType] = useState(1)
  const [types, setTypes] = useState([])
  const [card, setCard] = useState(0)
  const [cards, setCards] = useState([])
  const [value, setValue] = useState(0)
  const [purchaseDate, setPurchaseDate] = useState(null)
  const [qtdInstallments, setQtdInstallments] = useState(1)
  const [installments, setInstallments] = useState([])
  const [installmentsUpdated, setInstallmentsUpdated] = useState(true)
  const [editInstallment, setEditInstallment] = useState(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    paymentService.getTypes().then(res => setTypes(res || [])).catch(err => console.log(err))
    creditCardService.get().then(res => setCards(res || [])).catch(err => console.log(err))

    if (paramId > 0)
      paymentService.get(paramId)
        .then(res => fillPayment(res || {}))
        .catch(ex => console.log(ex))
        .finally(() => setLoading(false))
    else {
      fillPayment({})
      setLoading(false)
    }
  }, [paramId])

  function fillPayment(payment) {
    const list = payment.installments || []
    setId(payment.id || 0)
    setDescription(payment.description || '')
    setType(payment.type || 1)
    setCard(payment.creditCardId || 0)
    setQtdInstallments(list.length || 1)
    setValue(sum(list, p => p.value))
    const first = list[0] || {}
    setPurchaseDate((payment.date || first.date) ? new Date(payment.date || first.date) : null)
    setInstallments(list)
    setInstallmentsUpdated(!!payment.id)
  }

  // Qualquer mudança em valor, data, cartão ou quantidade exige gerar as parcelas de novo.
  const changed = setter => v => {
    if (loading)
      return
    setInstallmentsUpdated(false)
    setter(v)
  }

  function generateInstallments() {
    paymentService.generateInstallments({
      value,
      amount: qtdInstallments,
      date: purchaseDate,
      CreditCardId: card || undefined
    }).then(res => {
      setInstallments((res || []).map(({ number, value, date }) => ({ number, value, date })))
      setInstallmentsUpdated(true)
    }).catch(err => console.log(err))
  }

  function installmentChanged(installment) {
    setInstallments(installments.map(e => e.number === installment.number
      ? { ...e, paidValue: installment.paidValue, paidDate: installment.paidDate, exempt: installment.exempt }
      : e))
    setEditInstallment(null)
  }

  function save() {
    setLoading(true)
    paymentService.save({
      id,
      description,
      type,
      installments,
      creditCardId: card || null,
      date: purchaseDate
    })
      .then(() => {
        toast.success('Parcelamento salvo!')
        if (!id)
          navigate('/payments')
      })
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  const canGenerate = !installmentsUpdated && value > 0 && !!purchaseDate && qtdInstallments > 0
  const valid = !!description.trim() && installmentsUpdated && !!purchaseDate && installments.length > 0
  const perInstallment = qtdInstallments > 0 ? value / qtdInstallments : 0
  const selectedType = types.find(p => p.id === type)

  return (
    <Page icon={<ViewWeekRoundedIcon />} title={id ? 'Editar parcelamento' : 'Novo parcelamento'}
      subtitle="Cadastre uma vez e acompanhe cada parcela até quitar." loading={loading} maxWidth={900}>
      <Box sx={{ display: 'grid', gap: 2 }}>
        <Step number={1} title="O que foi comprado?">
          <Box sx={{ display: 'grid', gap: 2.5, gridTemplateColumns: { xs: '1fr', sm: '2fr 1fr' } }}>
            <TextField label="Descrição" placeholder="Ex.: Geladeira, Curso de inglês, Empréstimo do banco"
              value={description} onChange={e => setDescription(e.target.value)} autoFocus={!paramId} />
            {!!types.length &&
              <TextField select label="Tipo" value={type || ''} onChange={e => setType(e.target.value)}
                helperText={selectedType ? (selectedType.in ? 'Entra dinheiro para você' : 'Sai dinheiro do seu bolso') : ' '}>
                {types.map(p => <MenuItem key={p.id} value={p.id}>
                  <Box component="span" sx={{ color: p.in ? 'success.main' : 'text.primary', fontWeight: 700 }}>{p.description}</Box>
                </MenuItem>)}
              </TextField>
            }
          </Box>
        </Step>

        <Step number={2} title="Como foi pago?" help="parcelamento">
          <Box sx={{ display: 'grid', gap: 2.5, gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' } }}>
            <MoneyField label="Valor total da compra" value={value} onChange={changed(setValue)} helperText="Some todas as parcelas, se a loja cobrou juros." />
            <DateField label="Data da compra" value={purchaseDate} onChange={changed(setPurchaseDate)} />
            <TextField label="Em quantas vezes?" type="number" value={qtdInstallments}
              onChange={e => changed(setQtdInstallments)(Math.max(0, parseInt(e.target.value.replace(/\D/g, '') || '0', 10)))}
              slotProps={{ htmlInput: { min: 1, max: 480 } }}
              helperText={qtdInstallments > 0 && value > 0 ? <>Fica {qtdInstallments}x de <Money value={perInstallment} size="xs" bold /></> : 'Ex.: 10 para 10x'} />
            {!!cards.length &&
              <SelectField label="Foi no cartão de crédito?" help="fatura" value={card || 0} onChange={changed(setCard)}
                emptyLabel="Não (boleto, carnê, empréstimo)"
                options={cards.map(p => ({ value: p.id, label: p.name }))}
                helperText="No cartão, as parcelas caem nas faturas certas." />
            }
          </Box>
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, mt: 2.5, flexWrap: 'wrap' }}>
            <Button variant="contained" color="secondary" startIcon={<AutorenewRoundedIcon />} disabled={!canGenerate} onClick={generateInstallments}>
              Gerar parcelas
            </Button>
            {!installmentsUpdated && installments.length > 0 &&
              <Typography variant="body2" sx={{ color: 'warning.main', fontWeight: 700 }}>
                Você mudou os dados acima: gere as parcelas de novo antes de salvar.
              </Typography>
            }
          </Box>
        </Step>

        <Step number={3} title="Parcelas" help="parcela">
          {installments.length ?
            <>
              {!!id && !installmentsUpdated &&
                <Alert severity="warning" sx={{ mb: 2 }}>Ao gerar parcelas novamente, os pagamentos já marcados serão apagados.</Alert>}
              <Box sx={{ overflowX: 'auto' }}>
                <Table size="small" sx={{ minWidth: 620 }}>
                  <TableHead>
                    <TableRow>
                      <TableCell>Nº</TableCell>
                      <TableCell>Vencimento</TableCell>
                      <TableCell align="right">Valor</TableCell>
                      <TableCell>Situação</TableCell>
                      <TableCell align="right">Pago</TableCell>
                      <TableCell align="right">Ações</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {installments.map(p =>
                      <TableRow key={p.number} hover>
                        <TableCell sx={{ fontWeight: 700 }}>{p.number}</TableCell>
                        <TableCell>{dateToString(p.date)}</TableCell>
                        <TableCell align="right"><Money value={p.value} /></TableCell>
                        <TableCell>{installmentStatus(p)}</TableCell>
                        <TableCell align="right">
                          {p.paidDate && !p.exempt
                            ? <><Money value={p.paidValue} tone="in" /><Typography variant="caption" sx={{ display: 'block', color: 'text.secondary' }}>em {dateToString(p.paidDate)}</Typography></>
                            : '—'}
                        </TableCell>
                        <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                          {!p.paidDate && !p.exempt && <>
                            <Chip size="small" color="success" icon={<CheckRoundedIcon />} label="Paguei" clickable
                              onClick={() => installmentChanged({ ...p, paidDate: p.date, paidValue: p.value, exempt: false })} sx={{ mr: 0.5 }} />
                            <ActionIcon title="Isentar (não precisa pagar)" size="small"
                              onClick={() => installmentChanged({ ...p, paidDate: null, paidValue: null, exempt: true })}>
                              <DoNotDisturbOnRoundedIcon fontSize="small" />
                            </ActionIcon>
                          </>}
                          <ActionIcon title="Editar pagamento" size="small" color="primary" onClick={() => setEditInstallment(p)}><EditRoundedIcon fontSize="small" /></ActionIcon>
                        </TableCell>
                      </TableRow>
                    )}
                  </TableBody>
                </Table>
              </Box>
              <Typography variant="caption" sx={{ color: 'text.secondary', display: 'block', mt: 1 }}>
                "Paguei" considera o valor e a data de vencimento. Pagou diferente? Use o lápis para ajustar. As mudanças só valem depois de salvar.
              </Typography>
            </>
            :
            <Typography sx={{ color: 'text.secondary' }}>Preencha a etapa 2 e clique em <b>Gerar parcelas</b> para ver cada mês.</Typography>
          }
        </Step>

        <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end', flexWrap: 'wrap' }}>
          <Button component={Link} to="/payments" startIcon={<ArrowBackRoundedIcon />}>Voltar para parcelamentos</Button>
          <Button variant="contained" size="large" disabled={loading || !valid} onClick={save}>Salvar parcelamento</Button>
        </Box>
      </Box>

      <EditInstallmentDialog installment={editInstallment} onCancel={() => setEditInstallment(null)} onSave={installmentChanged} />
    </Page>
  )
}
