import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

import {
  Paper,
  List,
  ListItem,
  ListItemSecondaryAction,
  IconButton,
  ListItemText,
  Tooltip,
  Button,
  Typography
} from '@material-ui/core'

import {
  Delete as DeleteIcon,
  EditOutlined as EditIcon
} from '@material-ui/icons'

import { MainContainer } from '../../components/main'
import { paymentService } from '../../services'
import { toReal, dateToString } from '../../helpers'
import { PaymentFilter } from './PaymentFilter/PaymentFilter'

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
  }
}

export function Payments() {

  const [loading, setLoading] = useState(false)
  const [payments, setPayments] = useState([])

  useEffect(() => refresh(), [])

  function refresh(filter) {
    setLoading(true)
    paymentService.getAll(filter)
      .then(res => setPayments(res))
      .finally(res => setLoading(false))
  }

  function removePayment(id) {
    paymentService.remove(id)
      .then(() => refresh())
      .finally(() => setLoading(false))
  }

  return (
    <MainContainer title="Pagamentos" loading={loading}>
      <PaymentFilter filterChanged={e => refresh(e)} />
      {payments.length ?
        <div>
          <div style={styles.divNewPayment}>
            <Link to="/edit-payment/0">
              <Button variant="contained" color="primary">Adicionar Pagamento</Button>
            </Link>
          </div>
          <Paper>
            <List dense={true}>
              {payments.map(p =>
                <ListItem key={p.id}>
                  <ListItemText
                    primary={p.description}
                    style={{ width: '160px' }}
                    secondary={
                      <React.Fragment>
                        <Typography component="span" color={p.type.in ? 'primary' : 'secondary'}>
                          {p.type.description}
                        </Typography>
                      </React.Fragment>
                    }
                  />
                  <ListItemText
                    style={{ width: '20px' }}
                    secondary={p.firstPaymentFormatted}
                  />
                  <ListItemText
                    style={{ width: '40px' }}
                    secondary={toReal(p.total)}
                  />
                  <ListItemText
                    style={{ width: '20px' }}
                    secondary={p.conditionText}
                  />
                  <ListItemText
                    style={{ width: '40px' }}
                    secondary={p.creditCardText}
                  />
                  <ListItemText
                    style={{ width: '30px' }}
                    secondary={`${p.paidInstallments}/${p.installments.length}`}
                  />
                  <ListItemText style={{ width: '40px', color: 'gray' }}
                    primary={p.active ? '' : 'Inativado em:'}
                    secondary={p.active ? '' : dateToString(p.inactiveAt)}
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
                        onClick={() => removePayment(p.id)}>
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
            <span>A busca não retornou registros.</span>
          </div>
          <Link to="/edit-payment/0">
            <Button variant="contained" color="primary">Adicionar Pagamento</Button>
          </Link>
        </div>
      }
    </MainContainer>
  )
}