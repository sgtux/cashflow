import React, { useState, useEffect } from 'react'

import { Button } from '@mui/material'

import { MainContainer } from '../../components'
import { IconTextInput } from '../../components/main'

import { InputMoney } from '../../components/inputs'

import { authService } from '../../services'
import { toReal, fromReal } from '../../helpers'

export function Account() {

    const [loading, setLoading] = useState(false)
    const [expenseLimit, setExpenseLimit] = useState(0)
    const [fuelExpenseLimit, setFuelExpenseLimit] = useState(0)
    const [email, setEmail] = useState('')

    useEffect(() => {
        setLoading(true)
        authService.getAccount()
            .then(res => {
                setEmail(res.email)
                setExpenseLimit(toReal(res.expenseLimit || 0))
                setFuelExpenseLimit(toReal(res.fuelExpenseLimit || 0))
            }).finally(() => setLoading(false))
    }, [])

    function save() {
        setLoading(true)
        authService.update({
            expenseLimit: fromReal(expenseLimit),
            fuelExpenseLimit: fromReal(fuelExpenseLimit)
        }).finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Conta" loading={loading}>
            <div>
                <IconTextInput
                    label="Email"
                    value={email}
                    disabled
                />
            </div>
            <div style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16 }}>Limite para Despesas:</span>
                <InputMoney
                    onChangeValue={(event, value, maskedValue) => setExpenseLimit(value)}
                    value={expenseLimit} />
            </div>
            <div style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16 }}>Limite para Combustível:</span>
                <InputMoney
                    onChangeValue={(event, value, maskedValue) => setFuelExpenseLimit(value)}
                    value={fuelExpenseLimit} />
            </div>
            <div style={{ display: 'flex', justifyContent: 'end', marginTop: 100, marginBottom: 10, width: 300 }}>
                <Button
                    onClick={() => save()}
                    color="primary"
                    variant="contained" autoFocus>salvar</Button>
            </div>
        </MainContainer>
    )
}