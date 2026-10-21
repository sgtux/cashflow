import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Box, Button, Card, Collapse, Grid2 as Grid, Typography } from '@mui/material'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import ExpandMoreRoundedIcon from '@mui/icons-material/ExpandMoreRounded'
import CheckRoundedIcon from '@mui/icons-material/CheckRounded'

import { Page, Money, MoneyField, EmptyState, ConfirmDialog, ActionIcon, StatusChip, FormDialog, HelpTip, Term } from '../../components'
import { creditCardService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { toast } from '../../helpers'
import { Palette } from '../../theme'
import { CreditCardDialog } from './CreditCardDialog'

const gradients = [
  [Palette.greenDark, Palette.green],
  ['#3A3F7A', '#5D63B8'],
  ['#7A3A4E', '#B85D79'],
  ['#2F4F6F', '#4F7FA8'],
  ['#5A4A1E', '#A7832F']
]

function CardFace({ card, index }) {
  const [from, to] = gradients[index % gradients.length]
  return (
    <Box sx={{
      p: 2.5, borderRadius: 3, color: '#fff', minHeight: 150,
      background: `linear-gradient(135deg, ${from}, ${to})`,
      display: 'flex', flexDirection: 'column', justifyContent: 'space-between'
    }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography sx={{ fontWeight: 800, fontSize: 18 }}>{card.name}</Typography>
        <CreditCardRoundedIcon sx={{ opacity: 0.8 }} />
      </Box>
      <Box>
        <Typography variant="caption" sx={{ opacity: 0.85 }}>Fatura atual</Typography>
        <Box><Money value={card.currentMonthDebt} size="xl" bold sx={{ color: '#fff' }} /></Box>
      </Box>
      <Box sx={{ display: 'flex', gap: 3, fontSize: 13, opacity: 0.9 }}>
        <span>Fecha dia <b>{card.invoiceClosingDay}</b></span>
        <span>Vence dia <b>{card.invoiceDueDay}</b></span>
      </Box>
    </Box>
  )
}

export function CreditCards() {
  const [cards, setCards] = useState([])
  const [card, setCard] = useState(null)
  const [open, setOpen] = useState({})
  const [removeItem, setRemoveItem] = useState(null)
  const [paymentToPay, setPaymentToPay] = useState(null)
  const [valueToPay, setValueToPay] = useState(0)
  const [loaded, setLoaded] = useState(false)

  const dispatch = useDispatch()

  useEffect(() => { refresh() }, [])

  async function run(action) {
    dispatch(showGlobalLoader())
    try {
      await action()
    } catch (err) {
      console.log(err)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  function refresh() {
    return run(async () => {
      setCards(await creditCardService.get() || [])
      setLoaded(true)
    })
  }

  function saveCard(c) {
    return run(async () => {
      if (c.id)
        await creditCardService.update(c)
      else
        await creditCardService.create(c)
      toast.success('Cartão salvo!')
      setCard(null)
      await refresh()
    })
  }

  function removeCard() {
    return run(async () => {
      await creditCardService.remove(removeItem.id)
      toast.success('Cartão removido.')
      setRemoveItem(null)
      await refresh()
    })
  }

  function startPayment(item, c) {
    setPaymentToPay({ description: item.description, itemId: item.id, type: item.type, creditCardId: c.id })
    setValueToPay(item.currentMonthDebt || 0)
  }

  function savePayment() {
    return run(async () => {
      await creditCardService.payCurrentInvoicePayment({ ...paymentToPay, paidValue: valueToPay })
      toast.success('Pagamento registrado!')
      setPaymentToPay(null)
      await refresh()
    })
  }

  return (
    <Page
      icon={<CreditCardRoundedIcon />}
      title="Cartões e faturas"
      subtitle="Veja quanto vai pagar em cada cartão neste mês e o que ainda falta quitar."
      guide="creditCards"
      actions={<Button variant="contained" startIcon={<AddRoundedIcon />} onClick={() => setCard({})}>Adicionar cartão</Button>}>

      {cards.length ?
        <Grid container spacing={2}>
          {cards.map((c, i) =>
            <Grid key={c.id} size={{ xs: 12, md: 6 }}>
              <Card sx={{ p: 2 }}>
                <CardFace card={c} index={i} />
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2, mt: 2 }}>
                  <Box>
                    <Typography variant="caption" sx={{ color: 'text.secondary', fontWeight: 700 }}>
                      Fatura deste mês<HelpTip id="fatura" />
                    </Typography>
                    <Box sx={{ mt: 0.5 }}>
                      {c.isCurrentMonthDebtPaid
                        ? <StatusChip status="done" label="Paga" />
                        : <StatusChip status="pending" label="Em aberto" />}
                    </Box>
                  </Box>
                  <Box>
                    <Typography variant="caption" sx={{ color: 'text.secondary', fontWeight: 700 }}>
                      Crédito devedor<HelpTip id="creditoDevedor" />
                    </Typography>
                    <Box><Money value={c.outstandingDebtTotal} tone={c.outstandingDebt <= 0 ? 'in' : 'out'} bold size="md" /></Box>
                  </Box>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', mt: 1.5 }}>
                  <Button size="small" onClick={() => setOpen({ ...open, [c.id]: !open[c.id] })}
                    endIcon={<ExpandMoreRoundedIcon sx={{ transition: '200ms', transform: open[c.id] ? 'rotate(180deg)' : 'none' }} />}>
                    {open[c.id] ? 'Esconder compras' : `Ver compras (${(c.items || []).length})`}
                  </Button>
                  <Box sx={{ flex: 1 }} />
                  <ActionIcon title="Editar cartão" color="primary" onClick={() => setCard(c)}><EditRoundedIcon /></ActionIcon>
                  <ActionIcon title="Remover cartão" color="error" onClick={() => setRemoveItem(c)}><DeleteOutlineRoundedIcon /></ActionIcon>
                </Box>
                <Collapse in={!!open[c.id]} unmountOnExit>
                  <Box sx={{ display: 'grid', gap: 1, mt: 1 }}>
                    {(c.items || []).length === 0 &&
                      <Typography variant="body2" sx={{ color: 'text.secondary', p: 1 }}>Nenhuma compra neste cartão.</Typography>}
                    {(c.items || []).map((h, j) =>
                      <Box key={j} sx={{ display: 'flex', alignItems: 'center', gap: 1.5, p: 1.25, borderRadius: 2, bgcolor: 'action.hover', flexWrap: 'wrap' }}>
                        <Box sx={{ flex: '1 1 160px', minWidth: 0 }}>
                          <Typography variant="body2" sx={{ fontWeight: 700 }}>{h.description}</Typography>
                          <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                            {h.plots ? <>Parcelas: {h.plots}</> : 'Compra à vista ou conta fixa'}
                            {h.total ? <> · total <Money value={h.total} size="xs" /></> : null}
                          </Typography>
                        </Box>
                        <Box sx={{ textAlign: 'right' }}>
                          <Typography variant="caption" sx={{ color: 'text.secondary' }}>falta pagar</Typography>
                          <Box><Money value={h.outstandingDebt} tone="out" size="sm" bold /></Box>
                        </Box>
                        {h.isCurrentMonthDebtPaid
                          ? <StatusChip status="done" label="Mês pago" />
                          : <Button size="small" variant="contained" startIcon={<CheckRoundedIcon />} onClick={() => startPayment(h, c)}>Pagar</Button>}
                      </Box>
                    )}
                  </Box>
                </Collapse>
              </Card>
            </Grid>
          )}
        </Grid>
        : loaded &&
        <EmptyState
          icon={<CreditCardRoundedIcon />}
          title="Nenhum cartão cadastrado"
          text="Cadastrar seus cartões ajuda a lembrar que cada compra no crédito é um dinheiro que sairá do seu bolso no mês seguinte."
          actionLabel="Adicionar meu cartão"
          onAction={() => setCard({})} />
      }

      <CreditCardDialog card={card} onClose={() => setCard(null)} onSave={saveCard} />
      <ConfirmDialog open={!!removeItem} danger
        title="Remover cartão?"
        text={`O cartão "${(removeItem || {}).name || ''}" será removido.`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={removeCard} />
      <FormDialog open={!!paymentToPay} maxWidth="xs"
        title="Registrar pagamento"
        subtitle={(paymentToPay || {}).description}
        saveLabel="Confirmar pagamento"
        saveDisabled={!(valueToPay > 0)}
        onClose={() => setPaymentToPay(null)}
        onSave={savePayment}>
        <MoneyField label="Quanto você pagou deste item na fatura?" value={valueToPay} onChange={setValueToPay}
          helperText="Já vem preenchido com o valor deste mês." />
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
          Pagar a <Term id="fatura">fatura</Term> inteira evita juros, que no cartão estão entre os mais altos do mercado.
        </Typography>
      </FormDialog>
    </Page>
  )
}
