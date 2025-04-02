import React, { useState, useEffect } from 'react'

import { Button, Grid2 as Grid, Card, Divider } from '@mui/material'

import { MainContainer } from '../../components'
import { InputMoney } from '../../components/inputs'

import { authService } from '../../services'
import { toReal, fromReal } from '../../helpers'

import { EmailSpan, BenefitsCard, BenefitsContainer, PlanTitle, PlanCost } from './styles'


export function Account() {

    const [loading, setLoading] = useState(false)
    const [expenseLimit, setExpenseLimit] = useState(0)
    const [fuelExpenseLimit, setFuelExpenseLimit] = useState(0)
    const [email, setEmail] = useState('')
    const [plan, setPlan] = useState(0)
    const [total, setTotal] = useState(0)
    const [recordsUsed, setRecordsUsed] = useState(0)

    useEffect(() => {
        setLoading(true)
        authService.getAccount()
            .then(res => {
                setEmail(res.email)
                setExpenseLimit(toReal(res.expenseLimit || 0))
                setFuelExpenseLimit(toReal(res.fuelExpenseLimit || 0))
                setPlan(res.plan)
                setRecordsUsed(res.recordsUsed)
                if (res.plan === 1) {
                    setTotal(10000)
                } else if (res.plan === 2) {
                    setTotal(100000)
                } else
                    setTotal(1000000)
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
            <Card style={{ padding: 20, width: 350 }}>
                <div>
                    <span style={{ fontSize: 16 }}>Email:</span>
                    <EmailSpan>{email}</EmailSpan>
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
                <div style={{ display: 'flex', justifyContent: 'end', marginTop: 20 }}>
                    <Button
                        onClick={() => save()}
                        color="primary"
                        variant="contained" autoFocus>salvar</Button>
                </div>
            </Card>
            <Divider style={{ marginTop: 50 }} />
            <div style={{ marginTop: 20, fontSize: 16 }}>
                {recordsUsed} registros utilizados de {total}.
            </div>
            <div style={{ marginTop: 20 }}>
                <Grid container rowSpacing={1} columnSpacing={{ xs: 2, sm: 2, md: 2 }}>
                    <Grid size={4}>
                        <BenefitsCard selected={plan === 1}>
                            <img height={50} src="./icons/bronze-medal.svg" />
                            <PlanTitle>Free</PlanTitle>
                            <PlanCost>R$ 0,00</PlanCost>
                            <BenefitsContainer>
                                <span>10.000 registros disponíveis.</span>
                            </BenefitsContainer>
                        </BenefitsCard>
                    </Grid>
                    <Grid size={4}>
                        <BenefitsCard selected={plan === 2}>
                            <img height={50} src="./icons/silver-medal.svg" />
                            <PlanTitle>Basic</PlanTitle>
                            <PlanCost>R$ 5,99</PlanCost>
                            <BenefitsContainer><span>100.000 registros disponíveis.</span></BenefitsContainer>
                        </BenefitsCard>
                    </Grid>
                    <Grid size={4}>
                        <BenefitsCard selected={plan === 3}>
                            <img height={50} src="./icons/gold-medal.svg" />
                            <PlanTitle>Premium</PlanTitle>
                            <PlanCost>R$ 19,99</PlanCost>
                            <BenefitsContainer><span>1.000.000 registros disponíveis.</span></BenefitsContainer>
                        </BenefitsCard>
                    </Grid>
                </Grid>
            </div>
        </MainContainer>
    )
}