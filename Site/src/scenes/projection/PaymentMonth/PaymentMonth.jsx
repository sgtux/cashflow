import React from 'react'

import {
  List,
  ListItem,
  ListItemText,
  ImageList,
  ImageListItem,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Typography
} from '@material-ui/core'

import { ExpandMore } from '@material-ui/icons'

import { toReal } from '../../../helpers/utils'
import { Invoices, MoneySpan } from '../../../components'
import { PaidSpan, ContainerCosts, BoxCosts } from '../styles'
import { PaymentCondition } from '../../../helpers'

export function PaymentMonth({ monthYear, paymentMonth, show }) {

  return (
    <ListItem>
      <ListItemText>
        <Accordion>
          <AccordionSummary expandIcon={<ExpandMore />}>
            <Typography>{monthYear} - {<MoneySpan bold gain={paymentMonth.accumulatedCost > 0}>{toReal(paymentMonth.accumulatedCost)}</MoneySpan>}</Typography>
          </AccordionSummary>

          <AccordionDetails>
            <div style={{ width: '100%' }}>
              <Invoices payments={paymentMonth.payments.filter(p => (p.creditCard || {}).id)} />
            </div>
          </AccordionDetails>
          <AccordionDetails>
            <List dense={true} style={{ width: '100%' }}>
              {paymentMonth.payments.filter(p => !(p.creditCard || {}).id).map((p, j) =>
                <ListItem style={{ backgroundColor: j % 2 == 0 ? '#ddd' : '#eee' }} key={j}>
                  <div style={{ width: '100%' }}>
                    <ImageList rowHeight={20} cols={6}>
                      <ImageListItem cols={3}>
                        <span style={{ color: '#666' }}>{p.description}</span>
                        <MoneySpan small gain={p.type.in}>({p.type.description})</MoneySpan>
                      </ImageListItem>
                      <ImageListItem cols={1}></ImageListItem>
                      <ImageListItem cols={1} style={{ textAlign: 'center' }}>
                        <span style={{ color: 'gray', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>
                          {p.condition === PaymentCondition.Installment ? `${p.number}/${p.qtdInstallments}` : ''}
                        </span>
                        {p.paidDate && <PaidSpan>PAGA</PaidSpan>}
                      </ImageListItem>
                      <ImageListItem cols={1} style={{ textAlign: 'end' }}>
                        <MoneySpan gain={p.type.in}>{toReal(p.cost)}</MoneySpan>
                      </ImageListItem>
                    </ImageList>
                  </div>
                </ListItem>
              )}
            </List>
          </AccordionDetails>
          <ContainerCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Gastos:</span>
              <MoneySpan>{toReal(paymentMonth.totalOut)}</MoneySpan>
            </BoxCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Ganhos:</span>
              <MoneySpan gain>{toReal(paymentMonth.totalIn)}</MoneySpan>
            </BoxCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Líquido:</span>
              <MoneySpan gain={paymentMonth.total > 0}>{toReal(paymentMonth.total)}</MoneySpan>
            </BoxCosts>
          </ContainerCosts>
          <div style={{ textAlign: 'center', padding: 10 }}>
            {show && <MoneySpan bold gain={paymentMonth.accumulatedCost > 0}>{toReal(paymentMonth.accumulatedCost)}</MoneySpan>}
          </div>
        </Accordion>
      </ListItemText>
    </ListItem>
  )
}