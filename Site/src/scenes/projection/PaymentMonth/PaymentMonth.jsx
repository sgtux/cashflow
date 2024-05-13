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
} from '@mui/material'

import { ExpandMore } from '@mui/icons-material'

import { toReal, getMonthYear } from '../../../helpers/utils'
import { Invoices, MoneySpan } from '../../../components'
import { PaidSpan, ContainerCosts, BoxCosts } from '../styles'

export function PaymentMonth({ paymentMonth }) {

  return (
    <ListItem>
      <ListItemText>
        <Accordion>
          <AccordionSummary expandIcon={<ExpandMore />}>
            <Typography>{getMonthYear(paymentMonth.monthYear)} - {<MoneySpan $bold={true} $gain={paymentMonth.accumulatedValue > 0}>{toReal(paymentMonth.accumulatedValue)}</MoneySpan>}</Typography>
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
                      </ImageListItem>
                      <ImageListItem cols={1}></ImageListItem>
                      <ImageListItem cols={1} style={{ textAlign: 'center' }}>
                        <span style={{ color: 'gray', fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>
                          {p.qtdInstallments ? `N°: ${p.number} - P: ${p.qtdPaidInstallments}/${p.qtdInstallments}` : ''}
                        </span>
                        {p.paidDate && <PaidSpan>PAGA</PaidSpan>}
                      </ImageListItem>
                      <ImageListItem cols={1} style={{ textAlign: 'end' }}>
                        <MoneySpan $gain={p.in}>{toReal(p.value)}</MoneySpan>
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
              <MoneySpan $gain>{toReal(paymentMonth.totalIn)}</MoneySpan>
            </BoxCosts>
            <BoxCosts>
              <span style={{ color: 'grey', marginRight: 20 }}>Líquido:</span>
              <MoneySpan $gain={paymentMonth.total >= 0}>{toReal(paymentMonth.total)}</MoneySpan>
            </BoxCosts>
          </ContainerCosts>
          <div style={{ textAlign: 'center', padding: 10 }}>
            <MoneySpan $bold $gain={paymentMonth.accumulatedValue >= 0}>{toReal(paymentMonth.accumulatedValue)}</MoneySpan>
          </div>
        </Accordion>
      </ListItemText>
    </ListItem>
  )
}