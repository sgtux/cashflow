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
    EditOutlined as EditIcon
} from '@material-ui/icons'

import { MainContainer, InputMonth } from '../../components'

import { householdExpenseService } from '../../services'
import { toReal, dateToString, ellipsisText } from '../../helpers'

export function HouseholdExpenses() {

    const [householdExpenses, setHouseholdExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [selectedMonth, setSelectedMonth] = useState('')
    const [selectedYear, setSelectedYear] = useState('')

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
            .then(res => setHouseholdExpenses(res))
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
        refresh(month, year)
    }

    return (
        <MainContainer title="Despesas Domésticas" loading={loading}>
            <InputMonth selectedYear={selectedYear}
                selectedMonth={selectedMonth}
                onChange={(month, year) => monthYearChanged(month, year)}
                label="Filtro"
                startYear={new Date().getFullYear() - 4} />
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