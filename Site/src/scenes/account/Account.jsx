import React, { useState, useEffect } from 'react'

import { Button } from '@mui/material'

import { MainContainer } from '../../components'

import { InputMoney } from '../../components/inputs'

import { authService } from '../../services'
import { toReal, fromReal } from '../../helpers'

export function Account() {

    const [loading, setLoading] = useState(false)
    const [spendingCeiling, setSpendingceiling] = useState(0)

    useEffect(() => {
        setLoading(true)
        authService.getAccount()
            .then(res => setSpendingceiling(toReal(res.spendingCeiling || 0)))
            .finally(() => setLoading(false))
    }, [])

    function save() {
        setLoading(true)
        authService.updateSpendingCeiling(fromReal(spendingCeiling))
            .then(() => { })
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Conta" loading={loading}>
            <div style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16 }}>Teto de Gastos:</span>
                <InputMoney
                    onChangeValue={(event, value, maskedValue) => setSpendingceiling(value)}
                    value={spendingCeiling} />
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