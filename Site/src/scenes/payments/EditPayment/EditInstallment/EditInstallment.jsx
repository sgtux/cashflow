import React, { useState, useEffect } from 'react'
import { Button } from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { toReal, fromReal } from '../../../../helpers'
import { Container } from './styles'

import { InputMoney } from '../../../../components/inputs'

export function EditInstallment({ installment, onCancel, onSave }) {

    const [cost, setCost] = useState('')
    const [date, setDate] = useState('')
    const [paidDate, setPaidDate] = useState('')

    useEffect(() => {
        setDate(installment.date)
        setPaidDate(installment.paidDate)
        setCost(toReal(installment.cost))
    }, [installment])

    function save() {
        onSave({
            number: installment.number,
            cost: fromReal(cost),
            date,
            paidDate
        })
    }

    return (
        <Container>
            <span style={{ fontSize: 20 }}>Editar Parcela:</span><br />
            N°: <span>{installment.number}</span><br />
            Valor: <InputMoney
                onChangeText={e => costChanged(e)}
                kind="money"
                value={cost} /><br />
            Vencimento: <DatePicker onChange={e => setDate(e)}
                dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} /><br />
            Pago em: <DatePicker onChange={e => setPaidDate(e)}
                dateFormat="dd/MM/yyyy" locale={ptBr} selected={paidDate} /><br />
            <div>
                <Button onClick={() => { }} variant="contained" autoFocus>cancelar</Button>
                <Button
                    style={{ marginLeft: 10 }}
                    onClick={() => save()}
                    variant="contained" autoFocus>ok</Button>
            </div>
        </Container>
    )
}