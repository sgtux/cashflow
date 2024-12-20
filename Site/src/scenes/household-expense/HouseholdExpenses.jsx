import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import {
    Paper,
    IconButton,
    Tooltip,
    Fab
} from '@mui/material'

import {
    Delete as DeleteIcon,
    EditOutlined as EditIcon,
    CreditCardOutlined as CardIcon
} from '@mui/icons-material'

import { InputMonth, AddFloatingButton } from '../../components'

import { householdExpenseService } from '../../services'
import { toReal, getMonthName } from '../../helpers'
import { EditHouseholdExpenseModal } from './edit-household-expense-modal/EditHouseholdExpenseModal'
import { getFabIconByExpenseType } from '../../components/icons'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'

export function HouseholdExpenses() {

    const [householdExpenses, setHouseholdExpenses] = useState([])
    const [selectedMonth, setSelectedMonth] = useState('')
    const [selectedMonthName, setSelectedMonthName] = useState('')
    const [selectedYear, setSelectedYear] = useState('')
    const [totals, setTotals] = useState([])
    const [total, setTotal] = useState(0)
    const [editHouseholdExpense, setEditHouseholdExpense] = useState()
    const [householdExpensesByDay, setHouseholdExpensesByDay] = useState({})
    const [days, setDays] = useState([])

    const dispatch = useDispatch()

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        setSelectedMonth(month)
        setSelectedYear(year)
        refresh(month, year)
    }, [])

    useEffect(() => setSelectedMonthName(getMonthName(selectedMonth - 1)), [selectedMonth])

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
        } else {
            setTotals([])
        }
        updateDayMonth(householdExpenses)

    }, [householdExpenses])

    function refresh(month, year) {
        dispatch(showGlobalLoader())
        householdExpenseService.getAll(month, year)
            .then(res => {
                setHouseholdExpenses(res)
                if (res.length)
                    setTotal(res.map(p => p.value).reduce((x, y) => x + y))
                else
                    setTotal(0)
            })
            .catch(err => console.log(err))
            .finally(() => dispatch(hideGlobalLoader()))
    }

    function updateDayMonth(householdExpenses) {
        const householdExpensesByDay = {}
        if (Array.isArray(householdExpenses)) {
            householdExpenses.forEach(p => {
                const day = parseInt(new Date(p.date).getDate())
                if (!householdExpensesByDay[day])
                    householdExpensesByDay[day] = []
                householdExpensesByDay[day].push(p)
            })
            setDays(Object.keys(householdExpensesByDay).reverse())
            setHouseholdExpensesByDay(householdExpensesByDay)
        }
    }

    function removeHouseholdExpense(id) {
        dispatch(showGlobalLoader())
        householdExpenseService.remove(id)
            .then(() => refresh(selectedMonth, selectedYear))
            .catch(err => console.log(err))
            .finally(() => dispatch(hideGlobalLoader()))
    }

    function monthYearChanged(month, year) {
        setSelectedMonth(month)
        setSelectedYear(year)
        refresh(month, year)
    }

    return (
        <div style={{ padding: 50 }}>
            <Paper style={{ padding: 20, color: '#555', display: 'flex', flexDirection: 'column', textAlign: 'center' }}>
                <InputMonth selectedYear={selectedYear}
                    selectedMonth={selectedMonth}
                    onChange={(month, year) => monthYearChanged(month, year)}
                    startYear={new Date().getFullYear() - 4} />
                <div style={{ textAlign: 'center', marginTop: 20 }}>
                    {
                        totals.map((e, i) => <div key={i} style={{ fontSize: 12 }}>
                            <span>{e.description}: {toReal(e.value)}</span>
                        </div>)
                    }
                    <div style={{ fontSize: 18, marginTop: 10 }}>
                        <span style={{ fontWeight: 'bold' }}>Total: {toReal(total)}</span>
                    </div>
                </div>
            </Paper>

            {days.length ?
                <div>
                    {days.map((p, q) =>
                        <Paper key={q} style={{ textAlign: 'center', padding: 20, marginTop: 20, color: '#555' }}>
                            <span style={{ fontSize: 16, fontWeight: 'bold' }}>{`${p} - ${selectedMonthName}`}</span>
                            {
                                householdExpensesByDay[p].map((x, i) => <Paper key={i} style={{ padding: 10, marginTop: 10, fontSize: 18, color: '#555' }}>
                                    {getFabIconByExpenseType(x.type)}
                                    {
                                        !!x.creditCardId &&
                                        <Tooltip title={x.creditCardName}>
                                            <Fab size='small'><CardIcon /></Fab>
                                        </Tooltip>
                                    }
                                    <span>{` ${toReal(x.value)} - ${x.description} `}</span>
                                    <Tooltip title="Editar esta Despesa">
                                        <IconButton onClick={() => setEditHouseholdExpense(x)} color="primary" aria-label="Edit">
                                            <EditIcon />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Remover Despesa">
                                        <IconButton color="secondary" aria-label="Delete"
                                            onClick={() => removeHouseholdExpense(x.id)}>
                                            <DeleteIcon />
                                        </IconButton>
                                    </Tooltip>
                                </Paper>)
                            }
                        </Paper>)
                    }
                </div>
                :
                <Paper style={{ marginTop: 20, textTransform: 'none', fontSize: '18px', textAlign: 'center', padding: 10 }}>
                    <span>Sem despesas para o filtro selecionado.</span>
                </Paper>
            }
            <EditHouseholdExpenseModal onClose={() => setEditHouseholdExpense(null)}
                editHouseholdExpense={editHouseholdExpense}
                onSave={() => { setEditHouseholdExpense(null); refresh(selectedMonth, selectedYear) }}
            />
            <AddFloatingButton onClick={() => setEditHouseholdExpense({})} />
        </div>
    )
}