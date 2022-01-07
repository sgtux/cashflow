import React, { useState, useEffect } from 'react'
import { Button, Dialog, DialogContent, Zoom, IconButton, Card } from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import {
    Delete as DeleteIcon,
    Edit as EditIcon
} from '@material-ui/icons'

import { toReal, fromReal, dateToString, toast } from '../../../helpers'
import { InputMoney, DatePickerContainer, DatePickerInput } from '../../../components/inputs'
import { recurringExpenseService } from '../../../services'
import { ConfirmModal } from '../../../components/main'

import { RecurringExpenseHistoryTable } from './styles'

export function RecurringExpenseHistoryModal({ recurringExpense, onCancel, show, requestRefresh }) {

    const [id, setId] = useState(0)
    const [date, setDate] = useState('')
    const [paidValue, setPaidValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [removeItem, setRemoveItem] = useState(null)

    const [history, setHistory] = useState([])

    useEffect(() => {
        if ((recurringExpense || {}).history)
            setHistory(recurringExpense.history)
    }, [recurringExpense])

    useEffect(() => {
        setFormIsValid(date && fromReal(paidValue) > 0)
    }, [date, paidValue])

    function clear() {
        setDate('')
        setPaidValue('')
        setId(0)
    }

    function save() {
        recurringExpenseService.saveHistory({
            id: id,
            paidValue: fromReal(paidValue),
            date: date,
            recurringExpenseId: recurringExpense.id
        }).then(() => {
            requestRefresh()
            toast.success('Salvo com sucesso!')
            clear()
        })
    }

    function edit(item) {
        setPaidValue(toReal(item.paidValue))
        setDate(new Date(item.date))
        setId(item.id)
    }

    function remove() {
        recurringExpenseService.removeHistory(removeItem.recurringExpenseId, removeItem.id)
            .then(() => {
                requestRefresh()
                toast.success('Removido com sucesso!')
                setRemoveItem(null)
            })
    }

    return (
        <Dialog
            open={show}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            fullScreen={true}
            TransitionComponent={Zoom}>
            <DialogContent>
                <div style={{ fontFamily: '"Helvetica Neue", Helvetica, Arial, sans-serif', minWidth: 500, minHeight: 340 }}>
                    <div>
                        {recurringExpense && recurringExpense.description} - {recurringExpense && toReal(recurringExpense.value)}
                    </div>
                    <RecurringExpenseHistoryTable>
                        <table>
                            <thead>
                                <tr>
                                    <th>Id</th>
                                    <th>Valor Pago</th>
                                    <th>Data</th>
                                    <th>Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {history.map((p, i) =>
                                    <tr key={i}>
                                        <td>{p.id}</td>
                                        <td>{toReal(p.paidValue)}</td>
                                        <td>{dateToString(p.date)}</td>
                                        <td>
                                            <IconButton onClick={() => edit(p)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => setRemoveItem(p)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </RecurringExpenseHistoryTable>
                    <Card>
                        <DatePickerContainer style={{ padding: 10 }}>
                            Valor Pago:
                            <InputMoney
                                onChangeText={e => setPaidValue(e)}
                                kind="money"
                                value={paidValue} />
                            <br />
                            Data: <DatePicker onChange={e => setDate(e)}
                                customInput={<DatePickerInput />}
                                dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
                            <br />
                            <Button onClick={() => clear()} autoFocus>limpar</Button>
                            <Button onClick={() => save()}
                                disabled={!formIsValid}
                                variant="contained"
                                color="primary"
                                autoFocus>salvar</Button>
                        </DatePickerContainer>
                    </Card>
                    <div style={{ margin: '10px', textAlign: 'center' }}>
                        <Button onClick={() => onCancel()} variant="contained" autoFocus>fechar</Button>
                    </div>
                </div>
            </DialogContent>
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text="Deseja realmente remover este histórico?" />
        </Dialog>
    )
}