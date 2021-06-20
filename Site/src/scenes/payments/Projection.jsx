import React from 'react'

import {
  Paper,
  List,
  ListItem,
  ListItemText,
  Typography,
  GridList,
  GridListTile
} from '@material-ui/core'
import CardIcon from '@material-ui/icons/CreditCardOutlined'

import { CardMain, Invoices } from '../../components'
import InputMonth from '../../components/inputs/InputMonth'

import { paymentService } from '../../services/index'

import { toReal, getMonthYear } from '../../helpers/utils'
import { Colors } from '../../helpers/themes'

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
      <CardMain title="Projeção" loading={this.state.loading}>
        <Paper>
          <div style={{ marginLeft: '20px', marginBottom: '20px' }}>
            <InputMonth
              month={this.state.endDate.month}
              year={this.state.endDate.year}
              label="Previsão até"
              onChange={v => this.refresh(v)} />
          </div>
          <List dense={true}>
            {this.state.dates.map((d, i) => {
              const { payments, costExpense, costIncome, accumulatedCost, total } = this.state.payments[d]
              return (<ListItem key={i}>
                <ListItemText>
                  <hr />
                  <span onClick={() => this.hideShowMonth(d)}
                    style={{
                      cursor: 'pointer',
                      fontSize: '16px',
                      fontWeight: 'bold',
                      color: '#666'
                    }}>{getMonthYear(d)}</span>

                  <div hidden={!this.state.hiddenMonths[d]}>
                    <Invoices payments={payments.filter(p => p.invoice)} />
                    <List dense={true}>
                      {payments.filter(p => !p.invoice).map((p, j) =>
                        <ListItem key={j}>
                          <GridList cellHeight={20} cols={6} style={{ width: '100%' }}>
                            <GridListTile cols={3}>
                              <span style={{ color: '#666', fontWeight: 'bold' }}>{p.description}</span>
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
                              <span style={{ fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>{p.fixedPayment ? '' : `${p.number}/${p.qtdInstallments}`}</span>
                            </GridListTile>
                            <GridListTile cols={1} style={{ textAlign: 'end' }}>
                              <span style={{
                                color: p.type === 2 ? Colors.AppRed : Colors.AppGreen,
                                fontSize: '14px',
                                marginRight: '10px'
                              }}>
                                {toReal(p.cost)}
                              </span>
                            </GridListTile>
                          </GridList>
                        </ListItem>
                      )}
                    </List>
                    <ListItemText style={{ textAlign: 'end', marginTop: '20px' }}>
                      <Typography component="span"
                        color={'secondary'}>
                        {`${toReal(costExpense)}`}
                      </Typography>
                      <Typography component="span"
                        color={'primary'}>
                        {`${toReal(costIncome)}`}
                      </Typography>
                      <span style={{
                        color: total < 0 ? Colors.AppRed : Colors.AppGreen,
                        marginTop: '6px',
                        padding: '3px',
                        fontSize: '14px',
                        fontWeight: 'bold'
                      }}>
                        {toReal(total)}
                      </span>
                    </ListItemText>
                  </div>
                  <div style={{ textAlign: 'center' }}>
                    <span style={{
                      color: accumulatedCost < 0 ? Colors.AppRed : Colors.AppGreen,
                      marginTop: '6px',
                      padding: '3px',
                      fontWeight: 'bold',
                      fontSize: '14px'
                    }}>
                      {toReal(accumulatedCost)}
                    </span>
                  </div>
                  <hr />
                </ListItemText>
              </ListItem>)
            }
            )}
          </List>
          <div style={{
            textAlign: 'center',
            fontSize: '20px',
            marginBottom: '20px',
            fontWeight: 'bold'
          }}>
            <span>Total:</span>
            <Typography component="span" style={{ fontSize: '30px', fontWeight: 'bold' }}
              color={this.state.totalCost < 0 ? 'secondary' : 'primary'}>
              {toReal(this.state.totalCost)}
            </Typography>
          </div>
        </Paper>
      </CardMain>
    )
  }
}