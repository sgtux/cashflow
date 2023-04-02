import React, { useState, useEffect } from 'react'

import {
    Paper,
    List,
    ListItem,
    ListItemSecondaryAction,
    IconButton,
    ListItemText,
    Tooltip
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    EditOutlined as EditIcon,
    Refresh as RefreshIcon,
    AddCircle as AddCircleIcon
} from '@material-ui/icons'

import { MainContainer, InputMonth } from '../../components'

import { householdExpenseService } from '../../services'
import { toReal, dateToString, ellipsisText } from '../../helpers'
import { EditHouseholdExpenseModal } from './edit-household-expense-modal/EditHouseholdExpenseModal'

export function HouseholdExpenses() {

    const [householdExpenses, setHouseholdExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [selectedMonth, setSelectedMonth] = useState('')
    const [selectedYear, setSelectedYear] = useState('')
    const [totals, setTotals] = useState([])
    const [total, setTotal] = useState(0)
    const [editHouseholdExpense, setEditHouseholdExpense] = useState()

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        setSelectedMonth(month)
        setSelectedYear(year)
        refresh(month, year)
    }, [])

    useEffect(() => {
        if (householdExpenses.length) {
            const keyValue = {}
            householdExpenses.forEach(e => {
                keyValue[e.typeDescription] = (keyValue[e.typeDescription] || 0) + e.value
            })
            const temp = []
            for (const key in keyValue)
                temp.push({ description: key, value: keyValue[key] })
            setTotals(temp)
        }
    }, [householdExpenses])

    function refresh(month, year) {
        setLoading(true)
        householdExpenseService.getAll(month, year)
            .then(res => {
                setHouseholdExpenses(res)
                if (res.length)
                    setTotal(res.map(p => p.value).reduce((x, y) => x + y))
                else
                    setTotal(0)
            })
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function removeHouseholdExpense(id) {
        setLoading(true)
        householdExpenseService.remove(id)
            .then(() => refresh(selectedMonth, selectedYear))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function monthYearChanged(month, year) {
        setSelectedMonth(month)
        setSelectedYear(year)
    }

    return (
        <MainContainer title="Despesas" loading={loading}>
            <InputMonth selectedYear={selectedYear}
                selectedMonth={selectedMonth}
                onChange={(month, year) => monthYearChanged(month, year)}
                label="Filtro"
                startYear={new Date().getFullYear() - 4} />
            <IconButton style={{ marginLeft: 10 }} color="primary" aria-label="Refresh"
                onClick={() => refresh(selectedMonth, selectedYear)}>
                <RefreshIcon />
            </IconButton>
            {
                totals.map((e, i) => <div key={i} style={{ fontSize: 12 }}>
                    <span>{e.description}: {toReal(e.value)}</span>
                </div>)
            }
            <div style={{ fontSize: 16, marginTop: 20 }}>
                <span style={{ fontWeight: 'bold' }}>Total: {toReal(total)}</span>
            </div>
            {householdExpenses.length ?
                <div>
                    <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                        <IconButton onClick={() => setEditHouseholdExpense({})} variant="contained" color="primary">
                            <AddCircleIcon />
                        </IconButton>
                    </div>
                    <Paper style={{ marginTop: '20px' }}>

                        <List dense={true}>
                            {householdExpenses.map(p =>
                                <ListItem key={p.id}>
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={p.typeDescription}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={ellipsisText(p.description, 30)}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={p.creditCardText ? `(${p.creditCardText})` : ''}
                                    />
                                    <ListItemText
                                        style={{ width: '100px', textAlign: 'center' }}
                                        secondary={toReal(p.value)}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={dateToString(p.date)}
                                    />
                                    <ListItemSecondaryAction>
                                        <Tooltip title="Editar esta Despesa">
                                            <IconButton onClick={() => setEditHouseholdExpense(p)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Remover Despesa">
                                            <IconButton color="secondary" aria-label="Delete"
                                                onClick={() => removeHouseholdExpense(p.id)}>
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </ListItemSecondaryAction>
                                </ListItem>
                            )}
                        </List>
                    </Paper>
                </div>
                :
                <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                    <div style={{ marginBottom: 40 }}>
                        <span>Sem despesas para o filtro selecionado.</span>
                    </div>
                    <IconButton onClick={() => setEditHouseholdExpense({})} variant="contained" color="primary">
                        <AddCircleIcon />
                    </IconButton>
                </div>
            }
            <EditHouseholdExpenseModal onClose={() => setEditHouseholdExpense(null)}
                editHouseholdExpense={editHouseholdExpense}
                onSave={() => { setEditHouseholdExpense(null); refresh(selectedMonth, selectedYear) }}
            />
        </MainContainer>
    )
}