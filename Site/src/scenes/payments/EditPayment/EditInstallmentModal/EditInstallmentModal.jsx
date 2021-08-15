import React, { useState, useEffect } from 'react'
import { Button, Dialog, DialogTitle, DialogContent, Zoom } from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { toReal, fromReal } from '../../../../helpers'

import { InputMoney } from '../../../../components/inputs'

export function EditInstallmentModal({ installment, onCancel, onSave }) {

    const [cost, setCost] = useState('')
    const [date, setDate] = useState('')
    const [paidDate, setPaidDate] = useState('')

    useEffect(() => {
        setDate(new Date(installment.date))
        if (installment.paidDate)
            setPaidDate(new Date(installment.paidDate))
        setCost(toReal(installment.cost))
    }, [installment])

    function save() {
        onSave({
            number: installment.number,
            cost: fromReal(cost),
            date,
            paidDate: paidDate || undefined
        })
    }

    return (
        <Dialog
            open={!!installment}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}>
            <DialogTitle id="alert-dialog-title">Editar Parcela</DialogTitle>
            <DialogContent>
                <div style={{ minWidth: 360, minHeight: 340 }}>
                    N°: <span>{installment.number}</span><br />
                    Valor: <InputMoney
                        onChangeText={e => setCost(e)}
                        kind="money"
                        value={cost} /><br />
                    Vencimento: <DatePicker onChange={e => setDate(e)}
                        dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} /><br />
                    <div style={{ marginTop: 10 }}>
                        Pago em: <DatePicker onChange={e => setPaidDate(e)}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={paidDate} /><br />
                    </div>
                    <div style={{ margin: '10px', textAlign: 'end', marginTop: 200 }}>
                        <Button onClick={() => onCancel()} variant="contained" autoFocus>cancelar</Button>
                        <Button
                            style={{ marginLeft: 10 }}
                            onClick={() => save()}
                            color="primary"
                            variant="contained" autoFocus>ok</Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}