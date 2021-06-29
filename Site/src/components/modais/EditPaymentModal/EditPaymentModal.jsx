import React, { useState, useEffect } from 'react'
import {
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Zoom,
  CircularProgress,
  GridList,
  GridListTile
} from '@material-ui/core'

import { InstallmentList } from './InstallmentList/InstallmentList'
import { InstallmentSetBox } from './InstallmnetSetBox/InstallmentSetBox'
import { CreditCardBox } from './CreditCardBox/CreditCardBox'

import IconTextInput from '../../main/IconTextInput'

import { dateToString, toReal, toast, fromReal, debounce } from '../../../helpers'
import { paymentService } from '../../../services'
import { PaymentTypeBox } from './PaymentTypeBox/PaymentTypeBox'
import { CostDateFixedBox } from './CostDateFixedBox/CostDateFixedBox'

export function EditPaymentModal({ onFinish, cards, open, selectedPayment, onClose }) {

  const [description, setDescription] = useState('')
  const [type, setType] = useState(2)
  const [useCreditCard, setUseCreditCard] = useState(false)
  const [fixedPayment, setFixedPayment] = useState(false)
  const [qtdInstallments, setQtdInstallments] = useState(10)
  const [card, setCard] = useState(0)
  const [costByInstallment, setCostByInstallment] = useState(false)
  const [costText, setCostText] = useState('')
  const [installments, setInstallments] = useState([])
  const [firstPayment, setFirstPayment] = useState('')
  const [loading, setLoading] = useState(false)
  const [paidInstallments, setPaidInstallments] = useState([])
  const [invoice, setInvoice] = useState(false)
  const [types, setTypes] = useState([])

  useEffect(() => {

    const payment = selectedPayment || {}

    paymentService.getTypes().then(res => setTypes(res))

    const firstInstallment = (payment.installments || [])[0] || {}
    const qtdInstallments = (payment.installments || []).length || 1
    const costs = (payment.installments || []).map(p => p.cost)
    const paidInstallments = (payment.installments || []).filter(p => p.paid).map(p => p.number)

    setUseCreditCard(!!payment.creditCardId)
    setDescription(payment.description || '')
    setType(payment.type || 2)
    setCard(payment.creditCardId)
    setPaidInstallments(payment.paidInstallments)
    setInvoice(payment.invoice)
    setCostByInstallment(false)
    setQtdInstallments(qtdInstallments)
    setCostText(toReal(costs.length ? costs.reduce((a, b) => a + b) : 0))
    setFixedPayment(payment.fixedPayment)
    setPaidInstallments(paidInstallments)
    setFirstPayment(dateToString(firstInstallment.date ? new Date(firstInstallment.date) : null))
  }, [])

  useEffect(() => {
    updateInstallments()
  }, [paidInstallments, costByInstallment, qtdInstallments, costText, firstPayment, fixedPayment])

  useEffect(() => {
    if (cards.length)
      setCard(cards[0].id)
  }, [useCreditCard])

  function updateInstallments() {
    const installments = []
    let cost = fromReal(costText)
    if (cost > 0 && qtdInstallments > 0 && qtdInstallments <= 72 && /^[0-9]{2}\/[0-9]{2}\/[0-9]{4}$/.test(firstPayment)) {
      let day = Number(firstPayment.substr(0, 2))
      let month = Number(firstPayment.substr(3, 2))
      let year = Number(firstPayment.substr(6, 4))

      if (!fixedPayment) {
        let firstCost = cost
        if (!costByInstallment) {
          const total = cost
          cost = parseFloat(Number(cost / qtdInstallments).toFixed(2))
          const sum = parseFloat(Number(cost * qtdInstallments).toFixed(2))
          firstCost = cost + (total > sum ? total - sum : sum - total)
        }

        for (let i = 1; i <= qtdInstallments; i++) {
          if (month > 12) {
            month = 1
            year++
          }
          installments.push({
            number: i,
            cost: cost,
            date: new Date(`${month}/${day}/${year}`),
            paid: paidInstallments.indexOf(i) !== -1
          })
          month++
        }
        installments[0].cost = firstCost
      }
      else
        installments.push({ number: 1, cost: cost, date: new Date(`${month}/${day}/${year}`) })

      setInstallments(installments)
    }
  }

  function paidChanged(installment, paid) {
    if (paid && !paidInstallments.includes(installment.number))
      setPaidInstallments(paidInstallments.concat([installment.number]))
    else if (!paid)
      setPaidInstallments(paidInstallments.filter(p => p.number != installment.number))
  }

  function save() {

    const payment = { id: selectedPayment.id, description: description, type, installments, fixedPayment, invoice }

    if (!description || !installments.length) {
      toast.error('Preencha corretamente os campos.')
      return
    }



    if (useCreditCard)
      payment.creditCardId = card

    setLoading(true)

    paymentService.save(payment)
      .then(() => {
        toast.success('Salvo com sucesso.')
        onFinish()
      })
      .finally(() => setLoading(false))
  }

  return (
    <Dialog
      open={open}
      onClose={() => onClose()}
      transitionDuration={300}
      TransitionComponent={Zoom}
      maxWidth="lg"
      fullWidth>
      <DialogTitle style={{ textAlign: 'center' }}>
        {selectedPayment.id > 0 ? 'Editar Pagamento' : 'Novo Pagamento'}
      </DialogTitle>
      <DialogContent>
        <div style={{ textAlign: 'start', color: '#666', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }} >

          <GridList cellHeight={350} cols={5}>
            <GridListTile cols={3}>

              <IconTextInput
                label="Descrição"
                value={description}
                onChange={e => setDescription(e.value)}
              />

              <PaymentTypeBox paymentType={type || 2}
                paymentTypeChanged={e => setType(e)} />

              <CostDateFixedBox cost={costText}
                costChanged={e => setCostText(e)}
                date={firstPayment}
                dateChanged={e => setFirstPayment(e)}
                fixedPayment={fixedPayment}
                fixedPaymentChanged={e => setFixedPayment(e)}
              />

              <CreditCardBox
                cards={cards}
                useCreditCard={useCreditCard}
                useCreditCardChanged={e => setUseCreditCard(e)}
                card={card}
                cardChanged={e => setCard(e)}
                invoice={invoice}
                invoiceChanged={c => setInvoice(c)}
              />

              <InstallmentSetBox hide={fixedPayment}
                costByInstallment={costByInstallment}
                qtdInstallments={qtdInstallments}
                costByInstallmentChanged={checked => setCostByInstallment(checked)}
                qtdInstallmentsChanged={v => setQtdInstallments(v)}
              />

            </GridListTile>
            <GridListTile cols={2}>
              <InstallmentList installments={installments}
                paidChanged={(p, checked) => paidChanged(p, checked)}
                hide={!installments.length || fixedPayment} />
            </GridListTile>
          </GridList>
        </div>
      </DialogContent>
      <DialogActions>
        <div hidden={!loading}>
          <CircularProgress size={30} />
        </div>
        <Button onClick={() => onClose()} variant="contained" autoFocus>cancelar</Button>
        <Button
          disabled={loading}
          onClick={() => save()}
          color="primary"
          variant="contained" autoFocus>salvar</Button>
      </DialogActions>
    </Dialog>
  )
}