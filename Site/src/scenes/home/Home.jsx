import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import { MainContainer } from '../../components'
import { InputMonth } from '../../components/inputs'

import { MonthExpensesChart } from './MonthExpensesChart/MonthExpensesChart'

import { homeService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'

export function Home() {

    const [selectedDate, setSelectedDate] = useState({ month: '', year: '' })
    const [homeChart, setHomeChart] = useState([])

    const dispatch = useDispatch()

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        refresh({ month, year })
    }, [])

    async function refresh(date) {
        if (date.month && date.year) {
            setSelectedDate(date)
            dispatch(showGlobalLoader())
            try {
                const res = await homeService.getChart(date.month, date.year)
                setHomeChart(res)
            } catch (err) {
                console.log(err)
            } finally {
                dispatch(hideGlobalLoader())
            }
        }
    }

    return (
        <MainContainer title="Home">
            <InputMonth
                startYear={(new Date().getFullYear()) - 1}
                selectedMonth={selectedDate.month}
                selectedYear={selectedDate.year}
                countYears={2}
                label="Mês Referência:"
                onChange={(month, year) => refresh({ month, year })} />
            <MonthExpensesChart data={homeChart} />
        </MainContainer>
    )
}