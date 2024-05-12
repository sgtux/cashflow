import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import {
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
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
    const [type, setType] = useState('')
    const [types, setTypes] = useState([])
    const [formIsValid, setFormIsValid] = useState(false)

    useEffect(() => {
        earningService.getTypes()
            .then(res => setTypes(res))
            .catch(err => console.log(err))
    }, [])

    useEffect(() => {
        if (editEarning) {
            setDescription(editEarning.description || '')
            setDate(editEarning.date ? new Date(editEarning.date) : new Date())
            setValue(toReal(editEarning.value))
            setType(editEarning.type || 2)
        }
    }, [editEarning])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0 && type > 0)
    }, [description, value, date, type])

    function save() {
        earningService.save({
            id: editEarning.id || 0,
            description,
            type,
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
            TransitionComponent={Zoom}>
            <DialogContent>
                <div style={{ fontFamily: '"Helvetica Neue", Helvetica, Arial, sans-serif', fontSize: 14, color: '#666' }}>
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
                        <span style={{ fontSize: 16 }}>Valor:</span> <InputMoney
                            onChangeText={e => setValue(e)}
                            kind="money"
                            value={value} />
                    </div>
                    <div>
                        <FormControl>
                            <InputLabel htmlFor="select-tipo">Tipo</InputLabel>
                            <Select style={{ width: '200px' }} value={type || ''}
                                onChange={e => setType(e.target.value)}>
                                <MenuItem value={0}><span style={{ color: 'gray' }}>Selecione</span></MenuItem>
                                {types.map(p => <MenuItem key={p.id} value={p.id}>{p.description}</MenuItem>)}
                            </Select>
                        </FormControl>
                    </div>
                    <div style={{ margin: '10px', textAlign: 'center' }}>
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