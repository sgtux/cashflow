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
  EditOutlined as EditIcon,
  AddCircle as AddCircleIcon
} from '@material-ui/icons'

import { MainContainer } from '../../components/main'
import { paymentService } from '../../services'
import { toReal } from '../../helpers'
import { PaymentFilter } from './PaymentFilter/PaymentFilter'

import { PaidDoneSpan } from './styles'

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
  const [filter, setFilter] = useState({ done: false })

  useEffect(() => refresh(), [filter])

  function refresh() {
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
      <PaymentFilter filterChanged={e => setFilter(e)} />
      {payments.length ?
        <div>
          <div style={styles.divNewPayment}>
            <Link to="/edit-payment/0">
              <IconButton variant="contained" color="primary">
                <AddCircleIcon />
              </IconButton>
            </Link>
          </div>
          <Paper>
            <List dense={true}>
              {payments.map(p =>
                <ListItem key={p.id}>
                  <ListItemText
                    primary={p.description}
                    style={{ width: '100px' }}
                    secondary={
                      <React.Fragment>
                        <Typography component="span" color={p.type.in ? 'primary' : 'secondary'}>
                          {p.type.description}
                        </Typography>
                      </React.Fragment>
                    }
                  />
                  <ListItemText
                    style={{ width: '40px' }}
                    primary={<span style={{color: '#666'}}>{`${p.firstPaymentDate} - ${p.lastPaymentDate}`}</span>}
                    secondary={`${toReal(p.installmentValue)} - ${toReal(p.total)}`}
                  />
                  <ListItemText
                    style={{ width: '40px' }}
                    secondary={p.creditCardText}
                  />
                  <ListItemText
                    style={{ width: '30px' }}
                    primary={(p.done || p.currentMonthPaid) && <PaidDoneSpan>{`${p.done ? 'Concluído' : p.currentMonthPaid ? 'pago' : ''}`}</PaidDoneSpan>}
                    secondary={`${p.paidInstallments}/${p.installments.length}`}
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
            <IconButton variant="contained" color="primary">
              <AddCircleIcon />
            </IconButton>
          </Link>
        </div>
      }
    </MainContainer>
  )
}