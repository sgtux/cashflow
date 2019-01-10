import React from 'react'

import {
  Paper,
  List,
  ListItem,
  ListItemText,
  Typography,
  Button
} from '@material-ui/core'

import CardIcon from '@material-ui/icons/CreditCardOutlined'

import CardMain from '../../components/main/CardMain'
import InputMonth from '../../components/inputs/InputMonth'

import { paymentService } from '../../services/index'

import { toReal, getMonthYear, Months } from '../../helpers/utils'

export default class Payment extends React.Component {

  constructor(props) {
    super(props)
    const now = new Date()
    let month = now.getMonth() + 3
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
      forecastDate: { month, year },
      i: -1,
      j: -1
    }
  }

  componentDidMount() {
    this.refresh(this.state.forecastDate)
  }

  refresh(forecastDate) {
    this.setState({ loading: true, errorMessage: '', forecastDate })
    paymentService.getFuture(`${forecastDate.month}/01/${forecastDate.year}`)
      .then(res => {
        const dates = Object.keys(res)
        let total = 0
        dates.forEach(d => total += res[d].cost)
        setTimeout(() => {
          this.setState({ totalCost: total, loading: false, payments: res, dates })
        }, 300)
      }).catch(err => this.setState({ loading: false, errorMessage: err.message, forecastDate }))
  }

  showItems(i, j) {
    if (this.state.i === i && this.state.j === j)
      this.setState({ i: -1, j: -1 })
    else
      this.setState({ i: i, j: j })
  }

  render() {
    return (
      <CardMain title="Pagamentos" loading={this.state.loading}>
        <Paper>
          <div style={{ marginLeft: '20px', marginBottom: '20px' }}>
            <InputMonth
              month={this.state.forecastDate.month}
              year={this.state.forecastDate.year}
              label="Previsão até"
              onChange={v => this.refresh(v)} />
          </div>
          <List dense={true}>
            {this.state.dates.map((d, i) =>
              <ListItem key={i}>
                <ListItemText>
                  <hr />
                  <span style={{ fontSize: '16px', fontWeight: 'bold', color: '#666' }}>{getMonthYear(d)}</span>
                  <List dense={true}>
                    {this.state.payments[d].payments.map((p, j) =>
                      <ListItem key={j}>
                        <ListItemText style={{ width: '300px', textAlign: 'left' }}>
                          {p.isCreditCard ? <span style={{ fontWeight: 'bold' }}>Fatura: </span> : null}
                          {p.description}

                          <div hidden={this.state.i !== i || this.state.j !== j}>
                            <List dense={true}>
                              {p.items.map((g, k) =>
                                <ListItem key={k}>
                                  <ListItemText style={{ width: '100px', textAlign: 'right' }}>
                                    {g.description}
                                    <span style={{ marginLeft: 10, color: '#bb2222' }}>{toReal(g.cost)}</span>
                                  </ListItemText>
                                </ListItem>
                              )}
                            </List>
                          </div>

                        </ListItemText>
                        <ListItemText>
                          <Typography component="span" color={p.type === 1 ? 'primary' : 'secondary'}>
                            {toReal(p.cost)}
                            {p.isCreditCard ? <span onClick={() => this.showItems(i, j)} style={{ marginLeft: 20, color: 'gray', cursor: 'pointer' }}>Itens</span> : null}
                          </Typography>
                        </ListItemText>
                      </ListItem>
                    )}
                  </List>
                  <ListItemText style={{ textAlign: 'end' }}>
                    <Typography component="span"
                      color={this.state.payments[d].cost < 0 ? 'secondary' : 'primary'}>
                      {toReal(this.state.payments[d].cost)}
                    </Typography>
                  </ListItemText>
                  <hr />
                </ListItemText>
              </ListItem>
            )}
          </List>
          <div style={{ marginBottom: '20px' }}>
            <Typography component="span" style={{ textAlign: 'center' }}
              color={this.state.totalCost < 0 ? 'secondary' : 'primary'}>
              {toReal(this.state.totalCost)}
            </Typography>
          </div>
        </Paper>
      </CardMain>
    )
  }
}