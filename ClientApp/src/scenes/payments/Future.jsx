import React from 'react'

import {
  Paper,
  List,
  ListItem,
  ListItemText,
  Typography
} from '@material-ui/core'

import CardMain from '../../components/main/CardMain'

import { paymentService } from '../../services/index'

import { toReal, isSameMonth } from '../../helpers/utils'

const styles = {
  noRecords: {
    textTransform: 'none',
    fontSize: '18px',
    textAlign: 'center'
  },
  divNewPayment: {
    textTransform: 'none',
    fontSize: '18px',
    textAlign: 'center',
    marginTop: '20px'
  },
  errorMessage: {
    color: 'red'
  }
}

export default class Payment extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      loading: true,
      payments: [],
      dates: []
    }
  }

  componentDidMount() {
    this.refresh()
  }

  refresh() {
    this.setState({ loading: true, errorMessage: '' })
    paymentService.get().then(res => {
      const arr = []
      res.map(p => {
        const date = new Date(p.firstPayment)
        return { date: date, month: date.getMonth() + 1, year: date.getFullYear() }
      }).forEach(v => {
        if (arr.filter(p => p.month === v.month && p.year === v.year).length === 0)
          arr.push(v)
      })
      setTimeout(() => {
        this.setState({ loading: false, payments: res, dates: arr })
      }, 300)
    })
  }

  render() {
    return (
      <CardMain title="Pagamentos" loading={this.state.loading}>
        {this.state.payments.length ?
          <Paper>
            <List dense={true}>
              {this.state.dates.map((d, i) =>
                <ListItem key={i}>
                  <ListItemText>
                    {`${d.month}-${d.year}`}
                    <br />
                    <List dense={true}>
                      {this.state.payments.filter(p => isSameMonth(d.date, p.firstPayment)).map((p, j) =>
                        <ListItem key={j}>
                          <ListItemText>
                            {`Dia ${new Date(p.firstPayment).getDate()}`}
                          </ListItemText>
                          <ListItemText>
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
                  </ListItemText>
                </ListItem>
              )}
            </List>
          </Paper>
          :
          <div style={styles.noRecords}>
            <span>Você pagamentos cadastrados.</span>
          </div>
        }
      </CardMain>
    )
  }
}