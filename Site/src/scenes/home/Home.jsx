import React, { useState, useEffect } from 'react'

import { MainContainer } from '../../components'
import { InputMonth } from '../../components/inputs'

import { MonthExpensesChart } from './MonthExpensesChart/MonthExpensesChart'

import { homeService } from '../../services'

export function Home() {

    const [loading, setLoading] = useState(false)
    const [selectedDate, setSelectedDate] = useState({ month: '', year: '' })
    const [homeChart, setHomeChart] = useState([])

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        refresh({ month, year })
    }, [])

    function refresh(date) {
        if (date.month && date.year) {
            setSelectedDate(date)
            setLoading(true)
            homeService.getChart(date.month, date.year)
                .then(res => {
                    setHomeChart(res)
                    console.log(res)
                })
                .finally(() => setLoading(false))
        }
    }

    return (
        <MainContainer title="Home" loading={loading}>
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