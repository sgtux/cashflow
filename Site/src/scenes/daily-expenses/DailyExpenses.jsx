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
    VisibilityRounded
} from '@material-ui/icons'

import { MainContainer, InputMonth } from '../../components'

import { dailyExpensesService } from '../../services'
import { toReal, dateToString } from '../../helpers'

import { DailyExpensesDetailModal } from './daily-expenses-detail-modal/DailyExpensesDetailModal'

export function DailyExpenses() {

    const [dailyExpenses, setDailyExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [dailyExpenseDetail, setDailyExpenseDetail] = useState({})
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
        dailyExpensesService.getAll(month, year)
            .then(res => setDailyExpenses(res))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function removeDailyExpense(id) {
        setLoading(true)
        dailyExpensesService.remove(id)
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
        <MainContainer title="Despesas Diárias" loading={loading}>
            <InputMonth selectedYear={selectedYear}
                selectedMonth={selectedMonth}
                onChange={(month, year) => monthYearChanged(month, year)}
                label="Filtro"
                startYear={new Date().getFullYear() - 4} />
            {dailyExpenses.length ?
                <div>
                    <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                        <Link to="/edit-daily-expenses/0">
                            <Button variant="contained" color="primary">Adicionar Despesa</Button>
                        </Link>
                    </div>
                    <Paper style={{ marginTop: '20px' }}>

                        <List dense={true}>
                            {dailyExpenses.map(p =>
                                <ListItem key={p.id}>
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={p.shopName}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={toReal(p.totalPrice)}
                                    />
                                    <ListItemText
                                        style={{ width: '100px' }}
                                        secondary={dateToString(p.date)}
                                    />
                                    <ListItemSecondaryAction>
                                        <Tooltip title="Visualizar">
                                            <IconButton color="primary" aria-label="Edit"
                                                onClick={() => setDailyExpenseDetail(p)}>
                                                <VisibilityRounded />
                                            </IconButton>
                                        </Tooltip>
                                        <Link to={`/edit-daily-expenses/${p.id}`}>
                                            <Tooltip title="Editar esta Despesa">
                                                <IconButton color="primary" aria-label="Edit">
                                                    <EditIcon />
                                                </IconButton>
                                            </Tooltip>
                                        </Link>
                                        <Tooltip title="Remover Despesa">
                                            <IconButton color="secondary" aria-label="Delete"
                                                onClick={() => removeDailyExpense(p.id)}>
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </ListItemSecondaryAction>
                                </ListItem>
                            )}
                        </List>
                    </Paper>
                    <DailyExpensesDetailModal
                        onClose={() => setDailyExpenseDetail({})}
                        dailyExpense={dailyExpenseDetail} />
                </div>
                :
                <div style={{ textTransform: 'none', fontSize: '18px', textAlign: 'center' }}>
                    <div style={{ marginBottom: 40 }}>
                        <span>Sem despesas para o filtro selecionado.</span>
                    </div>
                    <Link to="/edit-daily-expenses/0">
                        <Button variant="contained" color="primary">Adicionar Despesa</Button>
                    </Link>
                </div>
            }
        </MainContainer>
    )
}