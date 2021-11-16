import React from 'react'

import {
  List,
  ListItem,
  ListItemText,
  GridList,
  GridListTile
} from '@material-ui/core'

import { toReal } from '../../../helpers/utils'
import { Invoices, MoneySpan } from '../../../components'
import { ArrowUp, ArrowDown, PaidSpan, ContainerCosts, BoxCosts } from '../styles'
import { PaymentCondition } from '../../../helpers'

export function PaymentMonth({ monthYear, paymentMonth, hideShowMonth, show }) {

  return (
    <ListItem>
      <ListItemText>
        <div style={{ display: 'flex', justifyContent: 'space-between' }}>
          <span onClick={() => hideShowMonth()}
            style={{
              cursor: 'pointer',
              fontSize: '16px',
              fontWeight: 'bold',
              color: '#666'
            }}>{monthYear}</span>
          {show && <MoneySpan bold gain={paymentMonth.accumulatedCost > 0}>{toReal(paymentMonth.accumulatedCost)}</MoneySpan>}
          {show ? <ArrowUp onClick={() => hideShowMonth()} /> : <ArrowDown onClick={() => hideShowMonth()} />}
        </div>

        <div hidden={!show}>
          <Invoices payments={paymentMonth.payments.filter(p => (p.creditCard || {}).id)} />
          <List dense={true}>
            {paymentMonth.payments.filter(p => !(p.creditCard || {}).id).map((p, j) =>
              <ListItem key={j}>
                <GridList cellHeight={20} cols={6} style={{ width: '100%' }}>
                  <GridListTile cols={3}>
                    <span style={{ color: '#666' }}>{p.description}</span>
                    <MoneySpan small gain={p.type.in}>({p.type.description})</MoneySpan>
                  </GridListTile>
                  <GridListTile cols={1}></GridListTile>
                  <GridListTile cols={1} style={{ textAlign: 'center' }}>
                    <span style={{ fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>
                      {p.condition === PaymentCondition.Installment ? `${p.number}/${p.qtdInstallments}` : ''}
                    </span>
                    {p.paidDate && <PaidSpan>PAGA</PaidSpan>}
                  </GridListTile>
                  <GridListTile cols={1} style={{ textAlign: 'end' }}>
                    <MoneySpan gain={p.type.in}>{toReal(p.cost)}</MoneySpan>
                  </GridListTile>
                </GridList>
              </ListItem>
            )}
          </List>
          <ContainerCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Gastos:</span>
              <MoneySpan>{toReal(paymentMonth.costExpense)}</MoneySpan>
            </BoxCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Ganhos:</span>
              <MoneySpan gain>{toReal(paymentMonth.costGain)}</MoneySpan>
            </BoxCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Líquido:</span>
              <MoneySpan gain={paymentMonth.total > 0}>{toReal(paymentMonth.total)}</MoneySpan>
            </BoxCosts>
          </ContainerCosts>
          <div style={{ textAlign: 'center' }}>
            {show && <MoneySpan bold gain={paymentMonth.accumulatedCost > 0}>{toReal(paymentMonth.accumulatedCost)}</MoneySpan>}
          </div>
        </div>
        <hr />
      </ListItemText>
    </ListItem>
  )
}