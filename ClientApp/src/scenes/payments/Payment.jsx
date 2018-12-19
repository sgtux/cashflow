import React from 'react'

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
  Divider,
  InputLabel,
  Select,
  FormControl,
  MenuItem,
  Checkbox,
  FormControlLabel,
  TextField,
  Typography
} from '@material-ui/core'

import DeleteIcon from '@material-ui/icons/Delete'
import CardIcon from '@material-ui/icons/CreditCardOutlined'
import { DatePicker, MuiPickersUtilsProvider } from 'material-ui-pickers'
import DateFnsUtilsIo from '@date-io/date-fns'

import CardMain from '../../components/main/CardMain'
import IconTextInput from '../../components/main/IconTextInput'
import Grid from '@material-ui/core/Grid'

import NumberFormat from 'react-number-format'

import { creditCardService, paymentService } from '../../services/index'

import { toReal, getDateStringEg, getDateFromStringEg } from '../../helpers/utils'

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

const CreditCardComponent = (props) => {
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

function NumberFormatCustom(props) {
  const { inputRef, onChange, ...other } = props;

  return (
    <NumberFormat
      {...other}
      getInputRef={inputRef}
      onValueChange={values => {
        onChange({
          target: {
            value: values.value,
          },
        });
      }}
      thousandSeparator="."
      decimalSeparator=","
      fixedDecimalScale={2}
      decimalScale={2}
      prefix="R$"
    />
  );
}

export default class Payment extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      loading: true,
      cards: [],
      payment: null,
      payments: [],
      paymentType: 2,
      useCreditCard: false,
      fixedPayment: false,
      card: null,
      firstPayment: getDateStringEg(new Date())
    }
  }

  componentDidMount() {
    this.refresh()
  }

  refresh() {
    this.setState({ payment: null, loading: true, errorMessage: '' })
    creditCardService.get().then(res => this.setState({ cards: res, card: res[0] ? res[0].id : null }))
    paymentService.get().then(res => {
      setTimeout(() => {
        this.setState({ loading: false, payments: res })
      }, 300)
    })
  }

  removePayment(id) {
    paymentService.remove(id)
      .then(() => this.refresh())
      .catch(err => this.setState({ errorMessage: err.error }))
  }

  save() {
    const { fixedPayment, description, firstPayment, cost, paymentType, plots, card, useCreditCard, plotsPaid } = this.state

    const payment = {}
    payment.id = this.state.payment.id
    payment.description = description
    payment.firstPayment = getDateFromStringEg(firstPayment)
    payment.cost = cost ? Number(cost) : 0
    payment.fixedPayment = fixedPayment
    payment.type = paymentType
    payment.plots = plots && !fixedPayment ? Number(plots) : 0
    payment.plotsPaid = plotsPaid && !fixedPayment ? Number(plotsPaid) : 0

    if (useCreditCard)
      payment.creditCardId = card

    if (payment.id)
      paymentService.update(payment)
        .then(() => this.refresh())
        .catch(err => this.setState({ errorMessage: err.error }))
    else
      paymentService.create(payment)
        .then(() => this.refresh())
        .catch(err => this.setState({ errorMessage: err.error }))
  }

  openEditNew(p) {
    const { description, firstPayment, fixedPayment, cost, type, plots, creditCardId, plotsPaid } = p || {}
    this.setState({
      payment: p || {},
      description: description || '',
      firstPayment: getDateStringEg(firstPayment ? new Date(firstPayment) : new Date()),
      cost: cost ? cost.toString() : '0',
      paymentType: type || 2,
      plots: plots ? plots.toString() : '1',
      card: creditCardId,
      useCreditCard: creditCardId ? true : false,
      plotsPaid: plotsPaid ? plotsPaid.toString() : '0',
      fixedPayment: fixedPayment ? true : false
    })
  }

  render() {
    return (
      <CardMain title="Pagamentos" loading={this.state.loading}>
        {this.state.payments.length ?
          <Paper>
            <List dense={true}>
              {this.state.payments.map(p =>
                <ListItem button key={p.id}
                  onClick={() => this.openEditNew(p)}>
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
                        <Typography component="span" color={p.type === 1 ? 'primary' : 'secondary'}>
                          {p.type === 1 ? 'Renda' : 'Despesa'}
                        </Typography>
                        {p.firstPaymentFormatted}
                      </React.Fragment>
                    }
                  />
                  <ListItemText
                    primary={toReal(p.cost)}
                    style={{ width: '200px' }}
                    secondary={
                      <React.Fragment>
                        {p.fixedPayment ? 'Fixo Mensal' : `${p.plotsPaid}/${p.plots}`}
                        <CreditCardComponent card={p.creditCard} />
                      </React.Fragment>
                    }
                  />
                  <ListItemSecondaryAction>
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
          :
          <div style={styles.noRecords}>
            <span>Você não possui pagamentos cadastrados.</span>
          </div>
        }
        <div style={styles.divNewPayment}>
          <Button variant="raised" color="primary" onClick={() => this.openEditNew()}>
            Adicionar Pagamento
          </Button>
          <Divider style={{ marginTop: '20px' }} />
          <div style={{ marginTop: '50px' }} hidden={this.state.payment === null}>

            <IconTextInput
              label="Descrição"
              value={this.state.description}
              onChange={(e) => this.setState({ description: e.value, errorMessage: '' })}
            />
            <FormControl style={{ marginLeft: '20px', marginTop: '10px' }}>
              <InputLabel htmlFor="select-tipo">Tipo</InputLabel>
              <Select
                value={this.state.paymentType}
                onChange={(e) => this.setState({ paymentType: e.target.value })}>
                <MenuItem key={1} value={1}>Renda</MenuItem>
                <MenuItem key={2} value={2}>Despesa</MenuItem>
              </Select>
            </FormControl>
            <br />
            <FormControlLabel
              control={
                <Checkbox
                  checked={this.state.fixedPayment}
                  onChange={(e, c) => this.setState({ fixedPayment: c })}
                  color="primary"
                />
              }
              label="Fixo mensal ?"
            />
            <br />
            <div hidden={this.state.fixedPayment}>
              <IconTextInput
                label="Quantidade de Parcelas"
                value={this.state.plots}
                type="number"
                onChange={(e) => this.setState({ plots: e.value.replace('.', ''), errorMessage: '' })}
              />
              <IconTextInput style={{ marginLeft: '10px' }}
                label="Parcelas Pagas"
                value={this.state.plotsPaid}
                type="number"
                onChange={(e) => this.setState({ plotsPaid: e.value.replace('.', ''), errorMessage: '' })}
              />
            </div>
            <div hidden={!this.state.cards.length}>
              <FormControlLabel
                control={
                  <Checkbox
                    checked={this.state.useCreditCard}
                    onChange={(e, c) => this.setState({ useCreditCard: c, card: this.state.cards[0].id })}
                    color="primary"
                  />
                }
                label="Cartão de crédito ?"
              />
            </div>
            {
              this.state.cards.length && this.state.useCreditCard ?
                <FormControl style={{ marginLeft: '20px', marginTop: '10px' }}>
                  <InputLabel htmlFor="select-tipo">Cartão de crédito</InputLabel>
                  <Select style={{ width: '200px' }} value={this.state.card}
                    onChange={(e) => this.setState({ card: e.target.value })}>
                    {this.state.cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
                  </Select>
                </FormControl>
                : null
            }

            <IconTextInput
              label="Valor Total"
              value={this.state.cost}
              type="number"
              onChange={(e) => this.setState({ cost: e.value, errorMessage: '' })}
            />
            <TextField style={{ marginLeft: '10px', marginTop: '10px' }}
              id="date"
              label="Primeiro Pagamento"
              type="datetime-local"
              onChange={(e) => this.setState({ firstPayment: e.target.value })}
              defaultValue={this.state.firstPayment}
              InputLabelProps={{
                shrink: true,
              }}
            />

            <div style={{ marginTop: '20px' }}>
              <Button color="primary" onClick={() => this.setState({ payment: null })}>
                Cancelar
              </Button>
              <Button variant="raised" color="primary"
                onClick={() => this.save()}>
                Salvar
              </Button>
            </div>
            <span style={{ color: '#d55', marginTop: '10px' }}>{this.state.errorMessage}</span>
          </div>
        </div>
      </CardMain>
    )
  }
}