import React, { useState, useEffect } from 'react'

import { Paper, List } from '@material-ui/core'

import { MainContainer, MoneySpan } from '../../components'

import { homeService } from '../../services'

import { toReal } from '../../helpers/utils'
import { PaymentMonth } from './PaymentMonth/PaymentMonth'

export function Projection() {

  const [loading, setLoading] = useState('')
  const [allPayments, setAllPayments] = useState({})
  const [totalValue, setTotalValue] = useState(0)

  useEffect(() => refresh(), [])

  function refresh() {
    setLoading(true)
    homeService.getProjection()
      .then(res => {
        let total = 0

        for (let item of res) {
          total += item.total
          total += item.previousMonthBalanceValue
          item.payments.sort((a, b) => a.description > b.description ? 1 : a.description < b.description ? -1 : 0)
        }

        setTotalValue(total)
        setAllPayments(res)
      }).catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  return (
    <MainContainer title="Projeção" loading={loading}>
      <Paper>

        <List dense={true}>
          {!!allPayments.length && allPayments.map((p, i) => <PaymentMonth key={i} paymentMonth={p} />)}
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