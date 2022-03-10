import React, { useState, useEffect } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'

import {
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select
} from '@material-ui/core'

import { InstallmentList } from './InstallmentList/InstallmentList'
import { InstallmentSetBox } from './InstallmnetSetBox/InstallmentSetBox'
import { MainContainer, IconTextInput } from '../../../components/main'

import { toReal, toast, fromReal } from '../../../helpers'
import { paymentService, creditCardService } from '../../../services'
import { PaymentTypeBox } from './PaymentTypeBox/PaymentTypeBox'
import { ValueDateBox } from './ValueDateBox/ValueDateBox'
import { EditInstallmentModal } from './EditInstallmentModal/EditInstallmentModal'

export function EditPayment() {

  const [description, setDescription] = useState('')
  const [type, setType] = useState(2)
  const [qtdInstallments, setQtdInstallments] = useState(10)
  const [card, setCard] = useState(0)
  const [valueByInstallment, setValueByInstallment] = useState(false)
  const [valueText, setValueText] = useState('')
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

        const firstInstallment = (payment.installments || [])[0] || {}
        const qtdInstallments = (payment.installments || []).length || 1
        const values = (payment.installments || []).map(p => p.value)

        setDescription(payment.description || '')
        setType((payment.type || {}).id || 1)
        setCard(payment.creditCardId)
        setActive(params.id == 0 || payment.active)
        setValueByInstallment(false)
        setQtdInstallments(qtdInstallments)

        setValueText(toReal(values.length ? values.reduce((a, b) => a + b) : 0))

        setFirstPayment(firstInstallment.date ? new Date(firstInstallment.date) : null)
        setInstallments(payment.installments || [])
        if (payment.id)
          setInstallmentsUpdated(true)
      })
      .catch(ex => console.log(ex))
      .finally(() => setLoading(false))
  }, [])

  useEffect(() => {
    setFormIsValid(!!description && installmentsUpdated && firstPayment)
  }, [description, installmentsUpdated, firstPayment])

  useEffect(() => {
    if (!editInstallment)
      setInstallmentsUpdated(false)
  }, [valueByInstallment, firstPayment, qtdInstallments, valueText])

  function updateInstallments() {
    const installments = []
    let value = fromReal(valueText)
    if (value > 0 && qtdInstallments > 0 && qtdInstallments <= 72 && firstPayment) {
      let day = firstPayment.getDate()
      let month = firstPayment.getMonth() + 1
      let year = firstPayment.getFullYear()

      if (card) {
        const invoiceDay = cards.filter(p => p.id === card)[0].invoiceDay
        if (day > invoiceDay) {
          month++
          if (month > 12) {
            month = 1
            year++
          }
        }
        day = invoiceDay
      }

      let firstValue = value
      if (!valueByInstallment) {
        const total = value
        value = parseFloat(Number(parseInt((value / qtdInstallments) * 100) / 100).toFixed(2))
        const sum = parseFloat(Number(value * qtdInstallments).toFixed(2))
        firstValue = value + (total > sum ? total - sum : sum - total)
      }

      for (let i = 1; i <= qtdInstallments; i++) {
        if (month > 12) {
          month = 1
          year++
        }
        installments.push({
          number: i,
          value: value,
          date: new Date(`${month}/${day}/${year}`),
        })
        month++
      }
      installments[0].value = firstValue

      setInstallments(installments)
      setInstallmentsUpdated(true)
    }
  }

  function save() {

    const payment = {
      id,
      description: description,
      typeId: type,
      installments,
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
        e.paidValue = installment.paidValue
        e.paidDate = installment.paidDate
      }
      temp.push(e)
    })
    setInstallments(temp)
    setTimeout(() => setEditInstallment(null), 200)
  }

  return (
    <MainContainer title="Pagamento" loading={loading}>
      <div style={{ textAlign: 'start', fontSize: 14, color: '#666', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }} >

        <IconTextInput
          label="Descrição"
          value={description}
          onChange={e => setDescription(e.value)}
        />

        {types.length &&
          <PaymentTypeBox types={types} paymentType={type}
            paymentTypeChanged={e => setType(e)} />
        }

        <div style={{ marginTop: 10 }} hidden={!cards.length}>
          <FormControl>
            <InputLabel htmlFor="select-tipo">Cartão de Crédito</InputLabel>
            <Select style={{ width: '200px' }} value={card || ''}
              onChange={e => setCard(e.target.value)}>
              <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
              {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
            </Select>
          </FormControl>
        </div>

        <ValueDateBox value={valueText}
          valueChanged={e => setValueText(e)}
          date={firstPayment}
          dateChanged={e => setFirstPayment(e)}
        />

        <InstallmentSetBox valueByInstallment={valueByInstallment}
          qtdInstallments={qtdInstallments}
          valueByInstallmentChanged={checked => setValueByInstallment(checked)}
          qtdInstallmentsChanged={v => setQtdInstallments(v)}
        />

        <div>
          <Button disabled={installmentsUpdated} onClick={() => updateInstallments()} variant="contained" autoFocus>atualizar parcelas</Button>

          {editInstallment && <EditInstallmentModal installment={editInstallment} onCancel={() => setEditInstallment()} onSave={p => installmentChanged(p)} />}
        </div>

        <InstallmentList installments={installments}
          hide={!installments.length}
          onEdit={p => setEditInstallment(p)}
          onPay={p => installmentChanged(p)}
        />
      </div>

      <div style={{ display: 'flex', justifyContent: 'end' }}>
        <Link to="/payments">
          <Button variant="contained" autoFocus>Lista de Pagamentos</Button>
        </Link>

        <Button
          style={{ marginLeft: 10 }}
          onClick={() => save()}
          color="primary"
          disabled={loading || !formIsValid}
          variant="contained" autoFocus>salvar</Button>
      </div>

    </MainContainer>
  )
}