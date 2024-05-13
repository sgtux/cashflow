import React, { useState, useEffect } from 'react'
import { Button, Dialog, DialogTitle, DialogContent, Zoom, Checkbox, FormControlLabel } from '@mui/material'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { toReal, fromReal, toDateFormat } from '../../../../helpers'

import { InputMoney } from '../../../../components/inputs'

export function EditInstallmentModal({ installment, onCancel, onSave }) {

    const [paidValue, setPaidValue] = useState('')
    const [paidDate, setPaidDate] = useState('')
    const [exempt, setExempt] = useState(false)

    useEffect(() => {
        if (installment.paidDate)
            setPaidDate(new Date(installment.paidDate))
        if (installment.paidValue)
            setPaidValue(toReal(installment.paidValue))
        setExempt(installment.exempt)
    }, [installment])

    function save() {
        onSave({
            ...installment,
            number: installment.number,
            paidValue: exempt || fromReal(paidValue) <= 0 ? undefined : fromReal(paidValue),
            paidDate: exempt || !paidDate ? undefined : paidDate,
            exempt
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
                    <div hidden={exempt}>
                        Valor Pago: <InputMoney
                            onChangeValue={(event, value, maskedValue) => setPaidValue(value)}
                            value={paidValue} /><br />
                        <div style={{ marginTop: 10 }}>
                            Pago em: <DatePicker onChange={e => setPaidDate(e)}
                                dateFormat="dd/MM/yyyy" locale={ptBr} selected={paidDate} /><br />

                        </div>
                    </div>
                    <div style={{ marginTop: 10 }}>
                        <FormControlLabel label="Isentar"
                            control={<Checkbox
                                checked={exempt}
                                value={exempt}
                                onChange={(e, c) => setExempt(c)}
                                color="primary"
                            />} />
                    </div>
                    <div style={{ margin: '10px', textAlign: 'end', marginTop: 200 }}>
                        <Button onClick={() => onCancel()} variant="contained" autoFocus>cancelar</Button>
                        <Button
                            style={{ marginLeft: 10 }}
                            onClick={() => save()}
                            color="primary"
                            disabled={(!!paidDate && !fromReal(paidValue)) || (!paidDate && fromReal(paidValue) > 0)}
                            variant="contained" autoFocus>ok</Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}