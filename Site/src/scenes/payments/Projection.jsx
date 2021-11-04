import React from 'react'

import {
  Paper,
  List,
  ListItem,
  ListItemText,
  GridList,
  GridListTile
} from '@material-ui/core'
import CardIcon from '@material-ui/icons/CreditCardOutlined'

import { MainContainer, Invoices, MoneySpan } from '../../components'
import { InputMonth } from '../../components/inputs'

import { paymentService } from '../../services/index'

import { toReal, getMonthYear } from '../../helpers/utils'
import { PaymentCondition } from '../../helpers'
import { ArrowUp, ArrowDown, ContainerCosts, BoxCosts, PaidSpan } from './styles'

export default class Projection extends React.Component {

  constructor(props) {
    super(props)
    const now = new Date()
    let month = now.getMonth() + 12
    let year = now.getFullYear()
    if (month > 12) {
      month = month - 12
      year++
    }
    this.state = {
      loading: true,
      payments: [],
      dates: [],
      totalCost: 0,
      forecastDate: {},
      i: -1,
      j: -1,
      hiddenMonths: {},
      startDate: `${now.getMonth() + 1}/${now.getFullYear()}`,
      endDate: { month, year },
      cards: []
    }
  }

  componentDidMount() {
    this.refresh(this.state.endDate)
  }

  refresh(endDate) {
    this.setState({ loading: true, errorMessage: '', endDate })
    paymentService.getFuture(this.state.startDate, `${endDate.month}/${endDate.year}`)
      .then(res => {
        const dates = Object.keys(res)
        let total = 0
        dates.forEach(d => {
          res[d].payments.sort((a, b) => a.description > b.description ? 1 : a.description < b.description ? -1 : 0)
          total += res[d].total
        })
        setTimeout(() => {
          this.setState({ totalCost: total, loading: false, payments: res, dates })
        }, 300)
        if (dates.length) {
          this.hideShowMonth(dates[0])
        }
      }).catch(err => this.setState({ loading: false, errorMessage: err.message, endDate }))
  }

  showItems(i, j) {
    if (this.state.i === i && this.state.j === j)
      this.setState({ i: -1, j: -1 })
    else
      this.setState({ i: i, j: j })
  }

  hideShowMonth(month) {
    const { hiddenMonths } = this.state
    hiddenMonths[month] = !hiddenMonths[month]
    this.setState({ hiddenMonths: hiddenMonths })
  }

  render() {
    return (
      <MainContainer title="Projeção" loading={this.state.loading}>
        <Paper>
          <div style={{ margin: 20, paddingTop: 20 }}>
            <InputMonth
              selectedMonth={this.state.endDate.month}
              selectedYear={this.state.endDate.year}
              label="Previsão até"
              onChange={v => this.refresh(v)} />
          </div>
          <List dense={true}>
            {this.state.dates.map((d, i) => {
              const { payments, costExpense, costGain, accumulatedCost, total } = this.state.payments[d]
              return (<ListItem key={i}>
                <ListItemText>
                  <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <span onClick={() => this.hideShowMonth(d)}
                      style={{
                        cursor: 'pointer',
                        fontSize: '16px',
                        fontWeight: 'bold',
                        color: '#666'
                      }}>{getMonthYear(d)}</span>
                    {!this.state.hiddenMonths[d] && <MoneySpan bold gain={accumulatedCost > 0}>{toReal(accumulatedCost)}</MoneySpan>}
                    {this.state.hiddenMonths[d] ? <ArrowUp onClick={() => this.hideShowMonth(d)} /> : <ArrowDown onClick={() => this.hideShowMonth(d)} />}
                  </div>

                  <div hidden={!this.state.hiddenMonths[d]}>
                    <Invoices payments={payments.filter(p => p.invoice)} />
                    <List dense={true}>
                      {payments.filter(p => !p.invoice).map((p, j) =>
                        <ListItem key={j}>
                          <GridList cellHeight={20} cols={6} style={{ width: '100%' }}>
                            <GridListTile cols={3}>
                              <span style={{ color: '#666' }}>{p.description}</span>
                              <MoneySpan small gain={p.type.in}>({p.type.description})</MoneySpan>
                            </GridListTile>
                            <GridListTile cols={1}>
                              {
                                p.creditCard ?
                                  <div style={{ display: 'flex' }}>
                                    <CardIcon style={{ color: '#666' }} />
                                    <span style={{ fontSize: '10px', color: '#666', marginLeft: '6px', fontWeight: 'bold', marginTop: '4px' }}>{p.creditCard.name}</span>
                                  </div>
                                  : null
                              }
                            </GridListTile>
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
                        <MoneySpan>{toReal(costExpense)}</MoneySpan>
                      </BoxCosts>
                      <BoxCosts>
                        <span style={{ color: 'grey', marginRight: 20 }}>Ganhos:</span>
                        <MoneySpan gain>{toReal(costGain)}</MoneySpan>
                      </BoxCosts>
                      <BoxCosts>
                        <span style={{ color: 'grey', marginRight: 20 }}>Líquido:</span>
                        <MoneySpan gain={total > 0}>{toReal(total)}</MoneySpan>
                      </BoxCosts>
                    </ContainerCosts>
                    <div style={{ textAlign: 'center' }}>
                      {this.state.hiddenMonths[d] && <MoneySpan bold gain={accumulatedCost > 0}>{toReal(accumulatedCost)}</MoneySpan>}
                    </div>
                  </div>
                  <hr />
                </ListItemText>
              </ListItem>)
            }
            )}
          </List>
          <div style={{
            textAlign: 'center',
            fontSize: '24px',
            marginBottom: '20px',
            fontWeight: 'bold',
            color: 'grey'
          }}>
            <span>Total Acumulado:</span><br />
            <MoneySpan bold bigger gain={this.state.totalCost > 0}>{toReal(this.state.totalCost)}</MoneySpan>
          </div>
        </Paper>
      </MainContainer>
    )
  }
}