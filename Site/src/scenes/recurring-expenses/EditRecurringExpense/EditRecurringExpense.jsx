import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import {
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@mui/material'

import { InputMoney, DatePickerInput, DatePickerContainer } from '../../../components/inputs'

import { MainContainer, IconTextInput } from '../../../components/main'

import { recurringExpenseService, creditCardService } from '../../../services'
import { toast, fromReal, toReal } from '../../../helpers'
import { RecurringExpenseHistoryModal } from '../RecurringExpenseHistoryModal/RecurringExpenseHistoryModal'

export function EditRecurringExpense() {

    const [id, setId] = useState(0)
    const [description, setDescription] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [loading, setLoading] = useState(false)
    const [card, setCard] = useState(null)
    const [cards, setCards] = useState([])
    const [recurringExpense, setRecurringExpense] = useState(null)
    const [showModal, setShowModal] = useState(false)
    const [inactiveAt, setInactiveAt] = useState()

    const params = useParams()

    useEffect(() => {
        creditCardService.get()
            .then(res => setCards(res))
            .catch(err => console.log(err))
        refresh()
    }, [])

    function refresh() {
        if (params.id > 0) {
            setLoading(true)
            setId(params.id)
            recurringExpenseService.get(params.id)
                .then(res => {
                    if (res) {
                        setDescription(res.description)
                        setValue(toReal(res.value))
                        setCard((res.creditCard || {}).id)
                        setRecurringExpense(res)
                        if (res.inactiveAt)
                            setInactiveAt(new Date(res.inactiveAt))
                    }
                })
                .catch(err => console.log(err))
                .finally(() => setLoading(false))
        }
    }

    useEffect(() => {
        setFormIsValid(description && fromReal(value) > 0)
    }, [description, value])

    function save() {
        setLoading(true)
        recurringExpenseService.save({
            id,
            description,
            value: fromReal(value),
            creditCardId: card,
            inactiveAt
        }).then(() => toast.success('Salvo com sucesso.'))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Despesa Recorrente" loading={loading}>
            <div style={{ fontFamily: '"Helvetica Neue", Helvetica, Arial, sans-serif', fontSize: 14, color: '#666' }}>
                <IconTextInput
                    label="Descrição"
                    value={description}
                    onChange={e => setDescription(e.value)}
                />
                <br />
                <br />
                Valor: <InputMoney
                    onChangeText={e => setValue(e)}
                    kind="money"
                    value={value} />
                <br />
                {!!cards.length &&
                    <div>
                        <FormControl>
                            <InputLabel htmlFor="select-tipo">Cartão de Crédito</InputLabel>
                            <Select style={{ width: '200px' }} value={card || ''}
                                onChange={e => setCard(e.target.value)}>
                                <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
                                {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
                            </Select>
                        </FormControl>
                        <br />
                    </div>
                }
                {
                    !!id && <div style={{ marginBottom: 10 }}>
                        <DatePickerContainer style={{ color: '#666' }}>
                            <span>Data Inativação:</span>
                            <DatePicker customInput={<DatePickerInput style={{ width: 115 }} />} onChange={e => setInactiveAt(e)}
                                dateFormat="dd/MM/yyyy" locale={ptBr} selected={inactiveAt} />
                        </DatePickerContainer>
                    </div>
                }
                <div style={{ margin: '10px', textAlign: 'right' }}>
                    {
                        !!id &&
                        <Button onClick={() => setShowModal(true)}
                            color="primary"
                            style={{ marginLeft: 20, marginRight: 20 }}
                            autoFocus>Histórico</Button>
                    }
                    <Link to="/recurring-expenses">
                        <Button variant="contained" autoFocus>lista de despesas</Button>
                    </Link>
                    <Button onClick={() => save()}
                        disabled={!formIsValid}
                        variant="contained"
                        color="primary"
                        style={{ marginLeft: 20, marginRight: 20 }}
                        autoFocus>salvar</Button>
                </div>
                <RecurringExpenseHistoryModal
                    recurringExpense={recurringExpense}
                    show={showModal}
                    requestRefresh={() => refresh()}
                    onCancel={() => setShowModal(false)} />
            </div>
        </MainContainer >
    )
}