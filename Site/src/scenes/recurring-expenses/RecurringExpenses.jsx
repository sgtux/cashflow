import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

import { RecurringExpensesTable, Container } from './styles'

import { recurringExpenseService } from '../../services'

import {
    IconButton,
    Button,
    Tooltip,
    FormControlLabel,
    Checkbox
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    History as HistoryIcon
} from '@material-ui/icons'

import { MainContainer, ConfirmModal } from '../../components/main'
import { TableActionPayButton } from '../../components'
import { toReal, toast } from '../../helpers'
import { RecurringExpenseHistoryModal } from './RecurringExpenseHistoryModal/RecurringExpenseHistoryModal'

export function RecurringExpenses() {

    const [recurringExpenses, setRecurringExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [recurringExpenseEditHistory, setRecurringExpenseEditHistory] = useState(null)
    const [removeItem, setRemoveItem] = useState(null)
    const [showInactive, setShowInactive] = useState(false)

    useEffect(() => refresh(), [showInactive])

    function refresh() {
        setRecurringExpenseEditHistory(null)
        setLoading(true)
        recurringExpenseService.getAll(showInactive)
            .then(res => setRecurringExpenses(res))
            .finally(() => setLoading(false))
    }

    function pay(expense) {
        setLoading(true)
        recurringExpenseService.saveHistory({
            paidValue: expense.value,
            date: new Date(),
            recurringExpenseId: expense.id
        })
            .then(res => refresh())
            .finally(() => setLoading(false))
    }

    function refreshEditHistory() {
        recurringExpenseService.get(recurringExpenseEditHistory.id)
            .then(res => {
                setRecurringExpenseEditHistory(res)
            })
            .catch(err => console.log(err))
    }

    function remove() {
        setLoading(true)
        setRecurringExpenseEditHistory(null)
        recurringExpenseService.remove(removeItem.id)
            .then(() => toast.success('Removido com sucesso!'))
            .catch(err => setRemoveItem(null))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Pagamento Recorrentes" loading={loading}>
            <FormControlLabel label="Exibir Inativos"
                control={<Checkbox
                    value={showInactive}
                    onChange={(e, c) => setShowInactive(c)}
                    color="primary"
                />} />
            <Container>
                <RecurringExpensesTable>
                    <table>
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Descrição</th>
                                <th>Valor Base</th>
                                <th>Situação</th>
                                <th>Ações</th>
                                <th style={{ width: 80 }}></th>
                            </tr>
                        </thead>
                        <tbody>
                            {recurringExpenses.map((p, i) =>
                                <tr key={i}>
                                    <td>{p.id}</td>
                                    <td>{p.description}</td>
                                    <td>{toReal(p.value)}</td>
                                    <td>{p.inactiveAt ? 'Inativo' : p.paid ? 'Pago' : 'Pendente'}</td>
                                    <td>
                                        <Tooltip title="Histórico de Pagamentos">
                                            <IconButton onClick={() => setRecurringExpenseEditHistory(p)} color="primary" aria-label="History">
                                                <HistoryIcon />
                                            </IconButton>
                                        </Tooltip>
                                        <Tooltip title="Editar">
                                            <Link to={`/edit-recurring-expenses/${p.id}`}>
                                                <IconButton color="primary" aria-label="Edit">
                                                    <EditIcon />
                                                </IconButton>
                                            </Link>
                                        </Tooltip>
                                        <Tooltip title="Remover">
                                            <IconButton onClick={() => setRemoveItem(p)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </td>
                                    <td>
                                        {!p.inactiveAt && !p.paid && <TableActionPayButton onClick={() => pay(p)}>Pagar</TableActionPayButton>}
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </RecurringExpensesTable>
                <Link to="/edit-recurring-expenses/0">
                    <Button variant="contained" color="primary">Adicionar Despesa Recorrente</Button>
                </Link>
            </Container>
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text={`Deseja realmente remover esta despesa recorrente? (${(removeItem || {}).description})`} />
            <RecurringExpenseHistoryModal
                recurringExpense={recurringExpenseEditHistory}
                show={!!recurringExpenseEditHistory}
                requestRefresh={() => refreshEditHistory()}
                onCancel={() => refresh()} />
        </MainContainer>
    )
}