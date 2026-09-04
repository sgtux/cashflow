import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import {
    Button,
    Dialog,
    DialogContent,
    Zoom
} from '@mui/material'

import { InputMoney, DatePickerInput, DatePickerContainer } from '../../../components/inputs'

import { IconTextInput } from '../../../components/main'

import { earningService } from '../../../services'
import { toast, fromReal, toReal } from '../../../helpers'

export function EditEarning({ editEarning, onClose, onSave }) {

    const [description, setDescription] = useState('')
    const [value, setValue] = useState('')
    const [date, setDate] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)

    useEffect(() => {
        if (editEarning) {
            setDescription(editEarning.description || '')
            setDate(editEarning.date ? new Date(editEarning.date) : new Date())
            setValue(toReal(editEarning.value))
        }
    }, [editEarning])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0)
    }, [description, value, date])

    function save() {
        earningService.save({
            id: editEarning.id || 0,
            description,
            date,
            value: fromReal(value)
        }).then(() => {
            toast.success('Salvo com sucesso.')
            onSave()
        }).catch(err => console.log(err))
    }

    return (
        <Dialog
            open={!!editEarning}
            onClose={onClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}
            PaperProps={{ style: { overflow: 'visible' } }}>
            <DialogContent sx={{ overflow: 'visible' }}>
                <div style={{ fontFamily: 'GraphikRegular', fontSize: 14, color: 'var(--mui-palette-text-secondary)' }}>
                    <IconTextInput
                        label="Descrição"
                        value={description}
                        onChange={e => setDescription(e.value)}
                    />
                    <DatePickerContainer style={{ marginTop: 20 }}>
                        <span style={{ fontSize: 16, marginRight: 10 }}>Data:</span>
                        <DatePicker onChange={e => setDate(e)} customInput={<DatePickerInput />}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
                    </DatePickerContainer>
                    <div style={{ marginTop: 20 }}>
                        <span style={{ fontSize: 16 }}>Valor:</span>
                        <InputMoney
                            onChangeValue={(event, value, maskedValue) => setValue(value)}
                            value={value} />
                    </div>
                    <div style={{ margin: '10px', marginTop: 20, textAlign: 'center' }}>
                        <Button onClick={() => save()}
                            disabled={!formIsValid}
                            variant="contained"
                            color="primary"
                            style={{ marginLeft: 20, marginRight: 20 }}
                            autoFocus>salvar</Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}