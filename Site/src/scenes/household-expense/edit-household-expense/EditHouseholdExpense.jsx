import React, { useState, useEffect } from 'react'
import DatePicker, { setDefaultLocale } from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import { Link, useParams, useNavigate } from 'react-router-dom'

import {
    TextField,
    Button
} from '@material-ui/core'

import { MainContainer } from '../../../components/main'
import { InputMoney, DatePickerContainer, DatePickerInput } from '../../../components/inputs'
import { toReal, fromReal } from '../../../helpers'

import { householdExpenseService } from '../../../services'

export function EditHouseholdExpense() {

    const [id, setId] = useState(0)
    const [loading, setLoading] = useState(false)
    const [description, setDescription] = useState('')
    const [date, setDate] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)

    const params = useParams()
    const navigate = useNavigate()

    useEffect(() => {
        if (Number(params.id) > 0) {
            setLoading(true)
            householdExpenseService.get(params.id)
                .then(res => {
                    if (!res) {
                        navigate('/household-expense')
                        return
                    }
                    setId(res.id)
                    setDescription(res.description)
                    setDate(new Date(res.date))
                    setValue(toReal(res.value))
                })
                .catch(err => console.log(err))
                .finally(() => setLoading(false))
        }
    }, [])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0)
    }, [description, date, value])

    function save() {
        setLoading(true)
        householdExpenseService.save({ id, description, date, value: fromReal(value) })
            .then(() => navigate('/household-expenses'))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Despesas Domésticas" loading={loading}>
            <TextField
                label="Descrição"
                placeholder="Placeholder"
                multiline
                onChange={e => setDescription(e.target.value)}
                value={description}
            />
            <DatePickerContainer style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16, marginRight: 10 }}>Data:</span>
                <DatePicker onChange={e => setDate(e)} customInput={<DatePickerInput />}
                    dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
            </DatePickerContainer>
            <div style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16 }}>Valor:</span>
                <InputMoney
                    onChangeText={e => setValue(e)}
                    kind="money"
                    value={value} />
            </div>
            <div style={{ display: 'flex', justifyContent: 'end' }}>
                <Link to="/household-expenses">
                    <Button onClick={() => { }} variant="contained" autoFocus>Lista de Despesas</Button>
                </Link>

                <Button
                    style={{ marginLeft: 10 }}
                    disabled={loading || !formIsValid}
                    onClick={() => save()}
                    color="primary"
                    variant="contained" autoFocus>salvar</Button>
            </div>
        </MainContainer>
    )
}