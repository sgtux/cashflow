import React, { useState, useEffect } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'
import {
  Button,
  ImageList,
  ImageListItem,
  FormControlLabel,
  Checkbox
} from '@material-ui/core'

import { InstallmentList } from './InstallmentList/InstallmentList'
import { InstallmentSetBox } from './InstallmnetSetBox/InstallmentSetBox'
import { ConditionCreditCardBox } from './ConditionCreditCardBox/ConditionCreditCardBox'
import { MainContainer } from '../../../components/main'
import IconTextInput from '../../../components/main/IconTextInput'
import { DatePickerInput, DatePickerContainer } from '../../../components/inputs'

import { toReal, toast, fromReal, PaymentCondition } from '../../../helpers'
import { paymentService, creditCardService } from '../../../services'
import { PaymentTypeBox } from './PaymentTypeBox/PaymentTypeBox'
import { CostDateBox } from './CostDateBox/CostDateBox'
import { EditInstallmentModal } from './EditInstallmentModal/EditInstallmentModal'

export function EditPayment() {

  const [description, setDescription] = useState('')
  const [type, setType] = useState(2)
  const [condition, setCondition] = useState(1)
  const [qtdInstallments, setQtdInstallments] = useState(10)
  const [card, setCard] = useState(0)
  const [costByInstallment, setCostByInstallment] = useState(false)
  const [costText, setCostText] = useState('')
  const [installments, setInstallments] = useState([])
  const [firstPayment, setFirstPayment] = useState('')
  const [loading, setLoading] = useState(false)
  const [types, setTypes] = useState([])
  const [id, setId] = useState(0)
  const [cards, setCards] = useState([])
  const [editInstallment, setEditInstallment] = useState()
  const [installmentsUpdated, setInstallmentsUpdated] = useState(false)
  const [formIsValid, setFormIsValid] = useState(false)
  const [active, setActive] = useState(true)
  const [inactiveAt, setInactiveAt] = useState()


  const params = useParams()
  const navigate = useNavigate()

  useEffect(() => {
    setLoading(true)
    paymentService.getTypes().then(res => setTypes(res))
    creditCardService.get().then(res => setCards(res))

    paymentService.get(params.id)
      .then(res => {

        const payment = res || {}
        setId(payment.id)

        if (payment.id && payment.inactiveAt) {
          setInactiveAt(new Date(payment.inactiveAt))
        }

        const firstInstallment = (payment.installments || [])[0] || {}
        const qtdInstallments = (payment.installments || []).length || 1
        const costs = (payment.installments || []).map(p => p.cost)

        setDescription(payment.description || '')
        setType((payment.type || {}).id || 1)
        setCard(payment.creditCardId)
        setActive(params.id == 0 || payment.active)
        setCostByInstallment(false)
        setQtdInstallments(qtdInstallments)

        if (payment.condition === PaymentCondition.Installment)
          setCostText(toReal(costs.length ? costs.reduce((a, b) => a + b) : 0))
        else
          setCostText(toReal(costs[0] || 0))

        if (payment.condition)
          setCondition(payment.condition)

        setFirstPayment(firstInstallment.date ? new Date(firstInstallment.date) : null)
        setInstallments(payment.installments || [])
        if (payment.id)
          setInstallmentsUpdated(true)
      })
      .catch(ex => console.log(ex))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    const installmentValid = condition !== PaymentCondition.Installment || installmentsUpdated
    setFormIsValid(!!description && installmentValid && firstPayment)
  }, [description, condition, installmentsUpdated, firstPayment])

  useEffect(() => {
    if (condition !== PaymentCondition.Installment)
      updateInstallments()
  }, [costText, condition, firstPayment])

  useEffect(() => {
    if (!editInstallment && condition === PaymentCondition.Installment)
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

    const payment = {
      id,
      description: description,
      typeId: type,
      baseCost: fromReal(costText),
      installments,
      inactiveAt,
      condition,
      creditCardId: card,
      active
    }

    setLoading(true)

    paymentService.save(payment)
      .then(() => {
        toast.success('Salvo com sucesso.')
        if (!id)
          navigate('/payments')
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
    const costs = (installments || []).map(p => p.cost)
    setCostText(toReal(costs.length ? costs.reduce((a, b) => a + b) : 0))
    setTimeout(() => setEditInstallment(null), 200)
  }

  return (
    <MainContainer title="Pagamento" loading={loading}>
      <div style={{ textAlign: 'start', fontSize: 14, color: '#666', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }} >

        <ImageList rowHeight={380} cols={5}>
          <ImageListItem cols={3}>

            <IconTextInput
              label="Descrição"
              value={description}
              onChange={e => setDescription(e.value)}
            />

            {types.length &&
              <PaymentTypeBox types={types} paymentType={type}
                paymentTypeChanged={e => setType(e)} />
            }

            <ConditionCreditCardBox
              cards={cards}
              condition={condition}
              conditionChanged={e => setCondition(Number(e))}
              card={card}
              cardChanged={e => setCard(e)}
            />

            <CostDateBox cost={costText}
              costChanged={e => setCostText(e)}
              date={firstPayment}
              dateChanged={e => setFirstPayment(e)}
            />

            <InstallmentSetBox hide={condition !== PaymentCondition.Installment}
              costByInstallment={costByInstallment}
              qtdInstallments={qtdInstallments}
              costByInstallmentChanged={checked => setCostByInstallment(checked)}
              qtdInstallmentsChanged={v => setQtdInstallments(v)}
            />

            {id && <div style={{ marginBottom: 10 }}>
              <DatePickerContainer style={{ color: '#666' }}>
                <span>Data Inativação:</span>
                <DatePicker customInput={<DatePickerInput style={{ width: 115 }} />} onChange={e => setInactiveAt(e)}
                  dateFormat="dd/MM/yyyy" locale={ptBr} selected={inactiveAt} />
              </DatePickerContainer>
            </div>
            }

            <div hidden={condition !== PaymentCondition.Installment}>
              <Button disabled={installmentsUpdated} onClick={() => updateInstallments()} variant="contained" autoFocus>atualizar parcelas</Button>

              {editInstallment && <EditInstallmentModal installment={editInstallment} onCancel={() => setEditInstallment()} onSave={p => installmentChanged(p)} />}
            </div>

          </ImageListItem>
          <ImageListItem cols={2}>
            <InstallmentList installments={installments}
              hide={condition !== PaymentCondition.Installment || !installments.length}
              onEdit={p => setEditInstallment(p)}
            />
          </ImageListItem>
        </ImageList>
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