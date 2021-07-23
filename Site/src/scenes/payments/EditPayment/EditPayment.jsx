import React, { useState, useEffect } from 'react'
import { useParams, Link, useHistory } from 'react-router-dom'
import {
  Button,
  CircularProgress,
  GridList,
  GridListTile
} from '@material-ui/core'

import { InstallmentList } from './InstallmentList/InstallmentList'
import { InstallmentSetBox } from './InstallmnetSetBox/InstallmentSetBox'
import { CreditCardBox } from './CreditCardBox/CreditCardBox'
import { MainContainer } from '../../../components/main'
import IconTextInput from '../../../components/main/IconTextInput'

import { toReal, toast, fromReal, PaymentCondition } from '../../../helpers'
import { paymentService, creditCardService } from '../../../services'
import { PaymentTypeBox } from './PaymentTypeBox/PaymentTypeBox'
import { CostDateConditionBox } from './CostDateConditionBox/CostDateConditionBox'
import { EditInstallment } from './EditInstallment/EditInstallment'

export function EditPayment() {

  const [description, setDescription] = useState('')
  const [type, setType] = useState(2)
  const [useCreditCard, setUseCreditCard] = useState(false)
  const [condition, setCondition] = useState(1)
  const [qtdInstallments, setQtdInstallments] = useState(10)
  const [card, setCard] = useState(0)
  const [costByInstallment, setCostByInstallment] = useState(false)
  const [costText, setCostText] = useState('')
  const [installments, setInstallments] = useState([])
  const [firstPayment, setFirstPayment] = useState('')
  const [loading, setLoading] = useState(false)
  const [invoice, setInvoice] = useState(false)
  const [types, setTypes] = useState([])
  const [id, setId] = useState(0)
  const [cards, setCards] = useState([])
  const [editInstallment, setEditInstallment] = useState()
  const [installmentsUpdated, setInstallmentsUpdated] = useState(false)
  const [formIsValid, setFormIsValid] = useState(false)

  const params = useParams()
  const history = useHistory()

  useEffect(() => {
    paymentService.getTypes().then(res => setTypes(res))
    creditCardService.get().then(res => setCards(res))

    paymentService.get(params.id)
      .then(res => {

        const payment = res || {}
        setId(payment.id)

        const firstInstallment = (payment.installments || [])[0] || {}
        const qtdInstallments = (payment.installments || []).length || 1
        const costs = (payment.installments || []).map(p => p.cost)

        setUseCreditCard(!!payment.creditCardId)
        setDescription(payment.description || '')
        setType((payment.type || {}).id || 1)
        setCard(payment.creditCardId)
        setInvoice(payment.invoice)
        setCostByInstallment(false)
        setQtdInstallments(qtdInstallments)
        setCostText(toReal(costs.length ? costs.reduce((a, b) => a + b) : 0))
        if (payment.condition)
          setCondition(payment.condition)
        setFirstPayment(firstInstallment.date ? new Date(firstInstallment.date) : null)
        setInstallments(payment.installments || [])
        if (payment.id)
          setInstallmentsUpdated(true)
      })
      .catch(ex => console.log(ex))
  }, [])

  useEffect(() => {
    const installmentValid = condition !== PaymentCondition.Installment || installmentsUpdated
    setFormIsValid(!!description && installmentValid && firstPayment)
  }, [description, condition, installmentsUpdated, firstPayment])

  useEffect(() => {
    if (cards.length)
      setCard(cards[0].id)
  }, [useCreditCard])

  useEffect(() => {
    if (condition !== PaymentCondition.Installment)
      updateInstallments()
  }, [costText, condition, firstPayment])

  useEffect(() => {
    if (condition === PaymentCondition.Installment)
      setInstallmentsUpdated(false)
  }, [costByInstallment, firstPayment, qtdInstallments, costText, condition])

  function updateInstallments() {
    const installments = []
    let cost = fromReal(costText)
    if (cost > 0 && qtdInstallments > 0 && qtdInstallments <= 72 && firstPayment) {
      let day = firstPayment.getDate()
      let month = firstPayment.getMonth() + 1
      let year = firstPayment.getFullYear()

      if (condition === PaymentCondition.Installment) {
        let firstCost = cost
        if (!costByInstallment) {
          const total = cost
          cost = parseFloat(Number(parseInt((cost / qtdInstallments) * 100) / 100).toFixed(2))
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
          })
          month++
        }
        installments[0].cost = firstCost
      }
      else
        installments.push({ number: 1, cost: cost, date: new Date(`${month}/${day}/${year}`) })

      setInstallments(installments)
      setInstallmentsUpdated(true)
    }
  }

  function save() {

    const payment = { id, description: description, typeId: type, installments, condition, invoice }

    if (useCreditCard)
      payment.creditCardId = card

    setLoading(true)

    paymentService.save(payment)
      .then(() => {
        toast.success('Salvo com sucesso.')
        if (!id)
          history.push('/payments')
      })
      .finally(() => setLoading(false))
  }

  function installmentChanged(installment) {
    const temp = []
    installments.forEach(e => {
      if (e.number === installment.number) {
        e.cost = installment.cost
        e.date = installment.date
        e.paidDate = installment.paidDate
      }
      temp.push(e)
    })
    setInstallments(temp)
    setEditInstallment(null)
  }

  return (
    <MainContainer title={'Pagamento' || description} loading={loading}>
      <div style={{ textAlign: 'start', fontSize: 14, color: '#666', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }} >

        <GridList cellHeight={380} cols={5}>
          <GridListTile cols={3}>

            <IconTextInput
              label="Descrição"
              value={description}
              onChange={e => setDescription(e.value)}
            />

            {types.length &&
              <PaymentTypeBox types={types} paymentType={type}
                paymentTypeChanged={e => setType(e)} />
            }

            <CostDateConditionBox cost={costText}
              costChanged={e => setCostText(e)}
              date={firstPayment}
              dateChanged={e => setFirstPayment(e)}
              condition={condition}
              conditionChanged={e => setCondition(Number(e))}
            />

            <InstallmentSetBox hide={condition !== PaymentCondition.Installment}
              costByInstallment={costByInstallment}
              qtdInstallments={qtdInstallments}
              costByInstallmentChanged={checked => setCostByInstallment(checked)}
              qtdInstallmentsChanged={v => setQtdInstallments(v)}
            />
            <br />
            <div hidden={condition !== PaymentCondition.Installment}>
              <Button disabled={installmentsUpdated} onClick={() => updateInstallments()} variant="contained" autoFocus>atualizar parcelas</Button>

              {editInstallment && <EditInstallment installment={editInstallment} onCancel={() => setEditInstallment()} onSave={p => installmentChanged(p)} />}
            </div>

            <CreditCardBox
              cards={cards}
              useCreditCard={useCreditCard}
              useCreditCardChanged={e => setUseCreditCard(e)}
              card={card}
              cardChanged={e => setCard(e)}
              invoice={invoice}
              invoiceChanged={c => setInvoice(c)}
            />

          </GridListTile>
          <GridListTile cols={2}>
            <InstallmentList installments={installments}
              hide={condition !== PaymentCondition.Installment || !installments.length}
              onEdit={p => setEditInstallment(p)}
            />
          </GridListTile>
        </GridList>
      </div>
      <div hidden={!loading}>
        <CircularProgress size={30} />
      </div>

      <div style={{ display: 'flex', justifyContent: 'end' }}>
        <Link to="/payments">
          <Button onClick={() => { }} variant="contained" autoFocus>Lista de Pagamentos</Button>
        </Link>

        <Button
          style={{ marginLeft: 10 }}
          disabled={loading}
          onClick={() => save()}
          color="primary"
          disabled={!formIsValid}
          variant="contained" autoFocus>salvar</Button>
      </div>

    </MainContainer>
  )
}