import React, { useState, useEffect } from 'react'

import { Paper, List } from '@material-ui/core'

import { MainContainer, MoneySpan } from '../../components'
import { InputMonth } from '../../components/inputs'

import { homeService } from '../../services'

import { toReal, getMonthYear } from '../../helpers/utils'
import { PaymentMonth } from './PaymentMonth/PaymentMonth'

export function Projection() {

  const [loading, setLoading] = useState('')
  const [allPayments, setAllPayments] = useState({})
  const [dates, setDates] = useState([])
  const [endDate, setEndDate] = useState({ month: '', year: '' })
  const [totalValue, setTotalValue] = useState(0)
  const [shownMonths, setShownMonths] = useState({})

  useEffect(() => {

    const now = new Date()
    let month = now.getMonth() + 12
    let year = now.getFullYear()

    if (month > 12) {
      month = month - 12
      year++
    }

    setEndDate({ month, year })
    refresh({ month, year })
  }, [])

  function refresh(date) {
    setLoading(true)
    setEndDate(date)
    homeService.getProjection(date.month, date.year)
      .then(res => {
        const dates = Object.keys(res)
        let total = 0
        dates.forEach(d => {
          res[d].payments.sort((a, b) => a.description > b.description ? 1 : a.description < b.description ? -1 : 0)
          total += res[d].total
          total += res[d].previousMonthBalanceValue
        })
        setTotalValue(total)
        setAllPayments(res)
        setDates(dates)
        if (dates.length)
          hideShowMonth(dates[0])
      }).catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  function hideShowMonth(month) {
    const temp = {}
    for (let m in shownMonths)
      temp[m] = shownMonths[m]
    temp[month] = !temp[month]
    setShownMonths(temp)
  }

  return (
    <MainContainer title="Projeção" loading={loading}>
      <Paper>
        <div style={{ margin: 20, paddingTop: 20 }}>
          <InputMonth
            startYear={new Date().getFullYear()}
            selectedMonth={endDate.month}
            selectedYear={endDate.year}
            label="Previsão até"
            onChange={(month, year) => refresh({ month, year })} />
        </div>

        <List dense={true}>
          {dates.map((d, i) => allPayments[d] && <PaymentMonth key={i} paymentMonth={allPayments[d]}
            hideShowMonth={() => hideShowMonth(d)}
            show={shownMonths[d]}
            monthYear={getMonthYear(d)} />)}
        </List>

        <div style={{
          textAlign: 'center',
          fontSize: '24px',
          marginBottom: '20px',
          fontWeight: 'bold',
          color: 'grey'
        }}>
          <span>Total Acumulado:</span><br />
          <MoneySpan bold bigger gain={totalValue >= 0}>{toReal(totalValue)}</MoneySpan>
        </div>
      </Paper>
    </MainContainer>
  )
}