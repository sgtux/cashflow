import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Link } from 'react-router-dom'

import {
  Paper,
  List,
  ListItem,
  ListItemSecondaryAction,
  IconButton,
  ListItemText,
  Tooltip,
  Typography
} from '@mui/material'

import {
  Delete as DeleteIcon,
  EditOutlined as EditIcon,
  AddCircle as AddCircleIcon
} from '@mui/icons-material'

import { MainContainer } from '../../components/main'
import { paymentService } from '../../services'
import { toReal } from '../../helpers'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
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

  const [payments, setPayments] = useState([])
  const [filter, setFilter] = useState({ description: '', done: false, startDate: null, endDate: null })

  const dispatch = useDispatch()

  useEffect(() => {
    refresh()
  }, [filter])

  async function refresh() {
    try {
      dispatch(showGlobalLoader())
      const res = await paymentService.getAll(filter)
      setPayments(res)
    } finally {
      dispatch(hideGlobalLoader())
    }
  }

  async function removePayment(id) {
    await paymentService.remove(id)
    refresh()
  }

  function filterChanged(newFilter) {
    if (newFilter.description !== filter.description
      || newFilter.done !== filter.done
      || newFilter.startDate !== filter.startDate
      || newFilter.endDate !== filter.endDate)
      setFilter(newFilter)
  }

  return (
    <MainContainer title="Parcelamentos">
      <PaymentFilter filterChanged={e => filterChanged(e)} />
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
              {payments.map((p, index) =>
                <ListItem key={index} style={{ backgroundColor: index % 2 === 0 ? '#eee' : '#fff' }}>
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
                    primary={<span style={{ color: '#666' }}>{`${p.firstPaymentDate} - ${p.lastPaymentDate}`}</span>}
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