import React, { useState, useEffect } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'

import {
  Button,
  FormControl,
  InputLabel,
  MenuItem,
  Select
} from '@mui/material'

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
  const [type, setType] = useState(1)
  const [qtdInstallments, setQtdInstallments] = useState(10)
  const [card, setCard] = useState(0)
  const [valueText, setValueText] = useState('')
  const [installments, setInstallments] = useState([])
  const [purchaseDate, setPurchaseDate] = useState('')
  const [loading, setLoading] = useState(true)
  const [types, setTypes] = useState([])
  const [id, setId] = useState(0)
  const [cards, setCards] = useState([])
  const [editInstallment, setEditInstallment] = useState()
  const [installmentsUpdated, setInstallmentsUpdated] = useState(true)
  const [formIsValid, setFormIsValid] = useState(false)


  const params = useParams()
  const navigate = useNavigate()

  useEffect(() => {
    paymentService.getTypes().then(res => setTypes(res))
    creditCardService.get().then(res => setCards(res))

    if (params.id > 0)
      paymentService.get(params.id)
        .then(res => fillPayment(res || {}))
        .catch(ex => console.log(ex))
        .finally(() => setLoading(false))
    else {
      fillPayment({})
      setLoading(false)
    }
  }, [])

  useEffect(() => {
    if (!loading)
      setFormIsValid(!!description && installmentsUpdated && purchaseDate)
  }, [description, installmentsUpdated, purchaseDate])

  function fillPayment(payment) {
    setId(payment.id)

    const firstInstallment = (payment.installments || [])[0] || {}
    const qtdInstallments = (payment.installments || []).length || 1
    const values = (payment.installments || []).map(p => p.value)

    setDescription(payment.description || '')
    setType(payment.type || 1)
    setCard(payment.creditCardId)
    setQtdInstallments(qtdInstallments)

    setValueText(toReal(values.length ? values.reduce((a, b) => a + b) : 0))

    setPurchaseDate((payment.date || firstInstallment.date) ? new Date(payment.date || firstInstallment.date) : null)
    setInstallments(payment.installments || [])
    if (payment.id)
      setInstallmentsUpdated(true)
  }

  function updateInstallments() {

    paymentService.generateInstallments({
      value: fromReal(valueText),
      amount: qtdInstallments,
      date: purchaseDate,
      CreditCardId: card
    }).then(res => {
      setInstallments(res.map(({ number, value, date }) => ({ number, value, date })))
      setInstallmentsUpdated(true)
    })
      .catch(err => console.log(err))
  }

  function save() {

    const payment = {
      id,
      description,
      type,
      installments,
      creditCardId: card,
      date: purchaseDate
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
        e.exempt = installment.exempt
      }
      temp.push(e)
    })
    setInstallments(temp)
    setTimeout(() => setEditInstallment(null), 200)
  }

  function creditCardHasChanged(card) {
    if (!loading) {
      setInstallmentsUpdated(false)
      setCard(card)
    }
  }

  function purchaseDateHasChanged(date) {
    if (!loading) {
      setInstallmentsUpdated(false)
      setPurchaseDate(date)
    }
  }

  function purchaseValueHasChanged(value) {
    if (!loading) {
      setInstallmentsUpdated(false)
      setValueText(value)
    }
  }

  function qtdInstallmentsHasChanged(qtd) {
    if (!loading) {
      setInstallmentsUpdated(false)
      setQtdInstallments(qtd)
    }
  }

  return (
    <MainContainer title="Pagamento" loading={loading}>
      <div style={{ textAlign: 'start', fontSize: 14, color: '#666', fontFamily: 'GraphikRegular' }} >

        <IconTextInput
          label="Descrição"
          value={description}
          onChange={e => setDescription(e.value)}
        />

        {!!types.length &&
          <PaymentTypeBox types={types} paymentType={type}
            paymentTypeChanged={e => setType(e)} />
        }

        <div style={{ marginTop: 10 }} hidden={!cards.length}>
          <FormControl>
            <InputLabel htmlFor="select-tipo">Cartão de Crédito</InputLabel>
            <Select style={{ width: '200px' }} value={card || ''}
              onChange={e => creditCardHasChanged(e.target.value)}>
              <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
              {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
            </Select>
          </FormControl>
        </div>

        <ValueDateBox value={valueText}
          valueChanged={e => purchaseValueHasChanged(e)}
          date={purchaseDate}
          dateChanged={e => purchaseDateHasChanged(e)}
        />

        <InstallmentSetBox
          qtdInstallments={qtdInstallments}
          qtdInstallmentsChanged={v => qtdInstallmentsHasChanged(v)} />

        <div>
          <Button disabled={installmentsUpdated} onClick={() => updateInstallments()} variant="contained" autoFocus>gerar parcelas</Button>

          {editInstallment && <EditInstallmentModal installment={editInstallment} onCancel={() => setEditInstallment()} onSave={p => installmentChanged(p)} />}
        </div>

        <InstallmentList installments={installments}
          hide={!installments.length}
          onEdit={p => setEditInstallment(p)}
          onPay={p => installmentChanged(p)}
          onExempt={p => installmentChanged(p)}
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