import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'


import {
    Paper,
    List,
    ListItem,
    ListItemSecondaryAction,
    IconButton,
    ListItemText,
    Tooltip,
    Button
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    EditOutlined as EditIcon,
    Refresh as RefreshIcon
} from '@material-ui/icons'

import { MainContainer, InputMonth } from '../../components'

import { householdExpenseService } from '../../services'
import { toReal, dateToString, ellipsisText } from '../../helpers'

export function HouseholdExpenses() {

    const [householdExpenses, setHouseholdExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [selectedMonth, setSelectedMonth] = useState('')
    const [selectedYear, setSelectedYear] = useState('')
    const [total, setTotal] = useState(0)

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        setSelectedMonth(month)
        setSelectedYear(year)
        refresh(month, year)
    }, [])

    function refresh(month, year) {
        setLoading(true)
        householdExpenseService.getAll(month, year)
            .then(res => {
                setHouseholdExpenses(res)
                if (res.length)
                    setTotal(res.map(p => p.value).reduce((x, y) => x + y))
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
            <div style={{ fontSize: 16, marginTop: 20 }}>
                <span>Total: {toReal(total)}</span>
            </div>
            {householdExpenses.length ?
                <div>
                    <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                        <Link to="/edit-household-expense/0">
                            <Button variant="contained" color="primary">Adicionar Despesa</Button>
                        </Link>
                    </div>
                    <Paper style={{ marginTop: '20px' }}>

                        <List dense={true}>
                            {householdExpenses.map(p =>
                                <ListItem key={p.id}>
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={ellipsisText(p.description, 30)}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={`(${p.typeDescription})`}
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
                                        <Link to={`/edit-household-expense/${p.id}`}>
                                            <Tooltip title="Editar esta Despesa">
                                                <IconButton color="primary" aria-label="Edit">
                                                    <EditIcon />
                                                </IconButton>
                                            </Tooltip>
                                        </Link>
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
                    <Link to="/edit-household-expense/0">
                        <Button variant="contained" color="primary">Adicionar Despesa</Button>
                    </Link>
                </div>
            }
        </MainContainer>
    )
}