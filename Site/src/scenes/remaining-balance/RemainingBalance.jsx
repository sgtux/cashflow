import React, { useState, useEffect } from 'react'

import {
    Paper,
    List,
    ListItem,
    IconButton,
    ListItemText,
    Zoom,
    Button,
    Dialog,
    DialogTitle,
    DialogContent,
    FormControlLabel,
    Checkbox
} from '@mui/material'

import {
    EditOutlined as EditIcon,
    RefreshOutlined as RefreshIcon
} from '@mui/icons-material'

import { MainContainer, InputMoney, MoneySpan } from '../../components'
import { ConfirmModal } from '../../components/main'

import { remainingBalanceService } from '../../services'
import { toReal, fromReal } from '../../helpers'

export function RemainingBalances() {

    const [remainingBalances, setRemainingBalances] = useState([])
    const [remainingBalancesFiltered, setRemainingBalancesFiltered] = useState([])
    const [loading, setLoading] = useState(false)
    const [value, setValue] = useState('')
    const [selectedRemainingBalance, setSelectedRemainingBalance] = useState(null)
    const [recalculateItem, setRecalculateItem] = useState(null)
    const [debt, setDebt] = useState(false)
    const [showAllItems, setShowAllItems] = useState(false)

    useEffect(() => refresh(), [])

    useEffect(() => {
        if (showAllItems)
            setRemainingBalancesFiltered(remainingBalances)
        else
            setRemainingBalancesFiltered(remainingBalances.slice(0, 5))
    }, [showAllItems, remainingBalances])

    function refresh() {
        setSelectedRemainingBalance(null)
        setRecalculateItem(null)
        setLoading(true)
        remainingBalanceService.getAll()
            .then(res => setRemainingBalances(res))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function update() {
        setLoading(true)
        remainingBalanceService.update({
            month: selectedRemainingBalance.month,
            year: selectedRemainingBalance.year,
            value: debt ? (fromReal(value) * -1) : fromReal(value)
        })
            .then(() => refresh())
            .catch(err => {
                setLoading(false)
                console.log(err)
            })

    }

    function recalculate() {
        setLoading(true)
        remainingBalanceService.recalculate(recalculateItem)
            .then(() => refresh())
            .catch(err => {
                setLoading(false)
                console.log(err)
            })
    }

    function edit(p) {
        setSelectedRemainingBalance(p)
        setValue(toReal(p.value))
        setDebt(p.value < 0)
    }

    function showHide(){
        setShowAllItems(!showAllItems)
    }

    return (
        <MainContainer title="Saldo Remanescente" loading={loading}>
            {remainingBalancesFiltered.length ?
                <>
                    <List dense={true}>
                        {remainingBalancesFiltered.map(p =>
                            <Paper key={p.id} style={{ padding: 10, marginTop: 10, fontSize: 24, color: '#555' }}>
                                <ListItem secondaryAction={<div hidden={!p.id}>
                                    <IconButton color="primary" aria-label="Edit" onClick={() => edit(p)}>
                                        <EditIcon />
                                    </IconButton>
                                    <IconButton color="primary" aria-label="Refresh" onClick={() => setRecalculateItem(p)}>
                                        <RefreshIcon />
                                    </IconButton>
                                </div>
                                }>
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary=""
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={p.monthYearText}
                                    />
                                    <ListItemText
                                        style={{ width: '100px', textAlign: 'center' }}
                                        secondary={<MoneySpan $gain={p.value >= 0}>{toReal(p.value)}</MoneySpan>}
                                    />
                                </ListItem>
                            </Paper>
                        )}
                    </List>
                    <div style={{ textAlign: 'center' }}>
                        <Button size="large" color="primary" onClick={() => showHide()}>{showAllItems ? 'Minimizar' : 'Exibir Todos'}</Button>
                    </div>
                </>
                :
                <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                    <div style={{ marginBottom: 40 }}>
                        <span>Sem registros.</span>
                    </div>
                </div>
            }
            <Dialog
                open={!!selectedRemainingBalance}
                onClose={() => setSelectedRemainingBalance(null)}
                aria-labelledby="alert-dialog-title"
                aria-describedby="alert-dialog-description"
                transitionDuration={250}
                TransitionComponent={Zoom}>
                <DialogTitle id="alert-dialog-title" style={{ textAlign: 'center' }}>
                    <span>{(selectedRemainingBalance || {}).monthYearText}</span>
                </DialogTitle>
                <DialogContent>
                    <div style={{ textAlign: 'center', padding: 30 }}>
                        <span style={{ fontSize: 16 }}>Valor:</span>
                        <InputMoney
                            onChangeValue={(event, value, maskedValue) => setValue(value)}
                            value={value} />
                        <br />
                        <FormControlLabel label="Devedor"
                            control={<Checkbox
                                value={debt}
                                checked={debt}
                                onChange={(e, c) => setDebt(c)}
                                color="primary"
                            />} />
                    </div>
                </DialogContent>
                <div style={{ marginBottom: '20px', textAlign: 'center' }}>
                    <Button size="large" color="primary" onClick={() => setSelectedRemainingBalance(null)} autoFocus>cancelar</Button>
                    <Button size="large" color="primary" onClick={() => update()} variant="contained" autoFocus>salvar</Button>
                </div>
            </Dialog>
            <ConfirmModal show={!!recalculateItem}
                onClose={() => setRecalculateItem(null)}
                onConfirm={() => recalculate()}
                text={`Deseja realmente recalcular o valor para este mês? (${(recalculateItem || {}).monthYearText})`} />
        </MainContainer>
    )
}