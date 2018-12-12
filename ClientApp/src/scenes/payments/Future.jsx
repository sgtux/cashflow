import React from 'react'

import {
  Paper,
  List,
  ListItem,
  ListItemText,
  Typography,
  TextField
} from '@material-ui/core'

import CardMain from '../../components/main/CardMain'

import { paymentService } from '../../services/index'

import { toReal, getMonthYear } from '../../helpers/utils'

export default class Payment extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      loading: true,
      payments: [],
      dates: [],
      totalCost: 0
    }
  }

  componentDidMount() {
    this.refresh()
  }

  refresh(forecastDate) {
    this.setState({ loading: true, errorMessage: '' })

    paymentService.getFuture(forecastDate || this.state.forecastDate)
      .then(res => {
        const dates = Object.keys(res)
        let total = 0
        dates.forEach(d => total += res[d].cost)
        setTimeout(() => {
          this.setState({ totalCost: total, loading: false, payments: res, dates: dates })
        }, 300)
      })
  }

  forecastChanged(value) {
    this.setState({ forecastDate: value })
    this.refresh(value)
  }

  render() {
    return (
      <CardMain title="Pagamentos" loading={this.state.loading}>
        <Paper>
          <div>
            <TextField style={{ marginLeft: '10px', marginTop: '10px' }}
              id="date"
              label="Previsão até"
              type="date"
              onChange={(e) => this.forecastChanged(e.target.value)}
              defaultValue={this.state.forecastDate}
              InputLabelProps={{
                shrink: true,
              }}
            />
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
                        <ListItemText>
                          {`Dia ${p.day}`}
                        </ListItemText>
                        <ListItemText style={{ width: '300px', textAlign: 'left' }}>
                          {p.description}
                        </ListItemText>
                        <ListItemText>
                          <Typography component="span" color={p.type === 1 ? 'primary' : 'secondary'}>
                            {toReal(p.cost)}
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
                    {/* {toReal(this.state.payments[d].cost)} */}
                  </ListItemText>
                </ListItemText>
              </ListItem>
            )}
          </List>
          <Typography component="span"
            color={this.state.totalCost < 0 ? 'secondary' : 'primary'}>
            {toReal(this.state.totalCost)}
          </Typography>
        </Paper>
      </CardMain>
    )
  }
}