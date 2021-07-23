import React from 'react'
import { Link } from 'react-router-dom'

import {
  Paper,
  List,
  ListItem,
  ListItemAvatar,
  Avatar,
  ListItemSecondaryAction,
  IconButton,
  ListItemText,
  Tooltip,
  Button,
  Typography
} from '@material-ui/core'

import {
  Delete as DeleteIcon,
  CreditCardOutlined as CardIcon,
  EditOutlined as EditIcon
} from '@material-ui/icons'

import { MainContainer } from '../../components/main'
import { paymentService, creditCardService } from '../../services'
import { toReal, dateToString, PaymentCondition } from '../../helpers'
import IconTextInput from '../../components/main/IconTextInput'

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

const CreditCardComponent = props => {
  if (!props.card)
    return null
  return (
    <span>
      <br />
      <CardIcon />
      <span style={{ marginTop: '-20px' }}>{props.card.name}</span>
    </span>
  )
}

export default class Payments extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      loading: true,
      cards: [],
      payment: {},
      payments: [],
      paymentType: 2,
      useCreditCard: false,
      fixedPayment: false,
      card: null,
      showModal: false,
      filtro: ''
    }
  }

  componentDidMount() {
    this.refresh()
    creditCardService.get().then(res => this.setState({ cards: res }))
  }

  refresh() {
    this.setState({ loading: true, errorMessage: '' })
    paymentService.getAll().then(res => {
      setTimeout(() => {
        this.setState({
          loading: false,
          payments: res,
          filtro: '',
          filteredPayments: res
        })
      }, 300)
    })
  }

  removePayment(id) {
    paymentService.remove(id)
      .then(() => this.refresh())
      .catch(err => this.setState({ errorMessage: err.error }))
  }

  openEditNew(p) {
    const { description, firstPayment, fixedPayment, cost, type, creditCardId } = p || {}
    this.setState({
      payment: p || {},
      description: description || '',
      firstPayment: dateToString(firstPayment),
      cost: cost ? cost.toString() : '0',
      paymentType: type || 2,
      card: creditCardId,
      useCreditCard: creditCardId ? true : false,
      fixedPayment: fixedPayment ? true : false,
      showModal: true
    })
  }

  render() {
    const { payments, filtro } = this.state
    return (
      <MainContainer title="Pagamentos" loading={this.state.loading}>
        {payments.length ?
          <div>
            <div style={styles.divNewPayment}>
              <Link to="/edit-payment/0">
                <Button variant="contained" color="primary">Adicionar Pagamento</Button>
              </Link>
            </div>
            <Paper style={{ marginTop: '20px' }}>

              <div style={{ textAlign: 'center' }}>
                <IconTextInput
                  label="Filtro"
                  onChange={e => this.setState({ filtro: e.value.toUpperCase() })}
                />
              </div>

              <List dense={true}>
                {payments.filter(p => !filtro || !p.description || p.description.toUpperCase().indexOf(filtro) !== -1).map(p =>
                  <ListItem key={p.id}>
                    <ListItemAvatar>
                      <Avatar>
                        <CardIcon />
                      </Avatar>
                    </ListItemAvatar>
                    <ListItemText
                      primary={p.description}
                      style={{ width: '200px' }}
                      secondary={
                        <React.Fragment>
                          <Typography component="span" color={p.type.in ? 'primary' : 'secondary'}>
                            {p.type.description}
                          </Typography>
                        </React.Fragment>
                      }
                    />
                    <ListItemText
                      style={{ width: '100px' }}
                      secondary={p.firstPaymentFormatted}
                    />
                    <ListItemText
                      style={{ width: '100px' }}
                      secondary={toReal(p.total)}
                    />
                    <ListItemText
                      style={{ width: '100px' }}
                      secondary={p.conditionText}
                    />
                    <ListItemText
                      style={{ width: '100px' }}
                      secondary={p.creditCardText}
                    />
                    <ListItemText
                      style={{ width: '100px' }}
                      secondary={p.condition === PaymentCondition.Installment ? `${p.paidInstallments}/${p.installments.length}` : ''}
                    />
                    <ListItemSecondaryAction>
                      <Link to={`/edit-payment/${p.id}`}>
                        <Tooltip title="Editar este pagamento">
                          <IconButton color="primary" aria-label="Edit">
                            <EditIcon />
                          </IconButton>
                        </Tooltip>
                      </Link>
                      <Tooltip title="Remover este pagamento">
                        <IconButton color="secondary" aria-label="Delete"
                          onClick={() => this.removePayment(p.id)}>
                          <DeleteIcon />
                        </IconButton>
                      </Tooltip>
                    </ListItemSecondaryAction>
                  </ListItem>
                )}
              </List>
            </Paper>
          </div>
          :
          <div style={styles.noRecords}>
            <div style={{ marginBottom: 40 }}>
              <span>Você não possui pagamentos cadastrados.</span>
            </div>
            <Link to="/edit-payment/0">
              <Button variant="contained" color="primary">Adicionar Pagamento</Button>
            </Link>
          </div>
        }
      </MainContainer>
    )
  }
}