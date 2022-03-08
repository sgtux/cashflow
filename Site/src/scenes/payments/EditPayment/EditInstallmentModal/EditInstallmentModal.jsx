import React, { useState, useEffect } from 'react'
import { Button, Dialog, DialogTitle, DialogContent, Zoom } from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { toReal, fromReal, toDateFormat } from '../../../../helpers'

import { InputMoney } from '../../../../components/inputs'

export function EditInstallmentModal({ installment, onCancel, onSave }) {

    const [paidValue, setPaidValue] = useState('')
    const [paidDate, setPaidDate] = useState('')


    useEffect(() => {
        if (installment.paidDate)
            setPaidDate(new Date(installment.paidDate))
        if (installment.paidValue)
            setPaidValue(toReal(installment.paidValue))
    }, [installment])

    function save() {
        onSave({
            ...installment,
            number: installment.number,
            paidValue: fromReal(paidValue) > 0 ? fromReal(paidValue) : undefined,
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
                    Vencimento: <span>{toDateFormat(installment.date)}</span><br />
                    Valor Pago: <InputMoney
                        onChangeText={e => setPaidValue(e)}
                        kind="money"
                        value={paidValue} /><br />
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
                            disabled={(paidDate && !fromReal(paidValue)) || (!paidDate && fromReal(paidValue))}
                            variant="contained" autoFocus>ok</Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}