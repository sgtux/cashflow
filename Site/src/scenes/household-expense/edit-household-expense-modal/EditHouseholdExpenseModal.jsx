import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'

import {
    TextField,
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    Dialog,
    DialogContent,
    Zoom
} from '@mui/material'

import { InputMoney, DatePickerContainer, DatePickerInput } from '../../../components/inputs'
import { toReal, fromReal } from '../../../helpers'

import { householdExpenseService, vehicleService } from '../../../services'

export function EditHouseholdExpenseModal({ editHouseholdExpense, onClose, onSave }) {

    const [id, setId] = useState(0)
    const [description, setDescription] = useState('')
    const [date, setDate] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [vehicleId, setVehicleId] = useState('')
    const [vehicles, setVehicles] = useState([])
    const [types, setTypes] = useState([])
    const [type, setType] = useState('')

    useEffect(() => {
        async function fetchData() {
            if (editHouseholdExpense) {
                try {
                    const taskVehicle = vehicleService.getAll()
                    const taskTypes = householdExpenseService.getTypes()
                    const listVehicles = await taskVehicle
                    const listTypes = await taskTypes

                    setVehicles(listVehicles)
                    setTypes(listTypes)

                    if (editHouseholdExpense.id) {
                        setId(editHouseholdExpense.id)
                        setDescription(editHouseholdExpense.description)
                        setDate(new Date(editHouseholdExpense.date))
                        setValue(toReal(editHouseholdExpense.value))
                        setVehicleId(editHouseholdExpense.vehicleId)
                        setType(editHouseholdExpense.type)
                    }
                } catch (ex) {
                    console.log(ex)
                }
            } else {
                setId(0)
                setDescription('')
                setDate('')
                setValue('')
                setVehicleId('')
                setType('')
            }
        }
        fetchData()
    }, [editHouseholdExpense])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0, Number(type) > 0)
    }, [description, date, value, type])

    function save() {
        householdExpenseService.save({
            id,
            description,
            date,
            value: fromReal(value),
            vehicleId: vehicleId ? Number(vehicleId) : 0,
            type: Number(type)
        })
            .then(() => onSave())
            .catch(err => console.log(err))
    }

    return (
        <Dialog
            open={!!editHouseholdExpense}
            onClose={onClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}>
            <DialogContent>
                <TextField
                    label="Descrição"
                    placeholder="Placeholder"
                    multiline
                    onChange={e => setDescription(e.target.value)}
                    value={description}
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
                <div>
                    <FormControl>
                        <InputLabel htmlFor="select-tipo">Tipo</InputLabel>
                        <Select style={{ width: '200px' }} value={type || ''}
                            onChange={e => setType(e.target.value)}>
                            <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
                            {types.map(p => <MenuItem key={p.id} value={p.id}>{p.description}</MenuItem>)}
                        </Select>
                    </FormControl>
                    <br />
                </div>
                {!!vehicles.length && type === 6 &&
                    <div>
                        <FormControl>
                            <InputLabel htmlFor="select-tipo">Veículo</InputLabel>
                            <Select style={{ width: '200px' }} value={vehicleId || ''}
                                onChange={e => setVehicleId(e.target.value)}>
                                <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
                                {vehicles.map(p => <MenuItem key={p.id} value={p.id}>{p.description}</MenuItem>)}
                            </Select>
                        </FormControl>
                        <br />
                    </div>
                }
                <div style={{ display: 'flex', justifyContent: 'end', marginTop: 100, marginBottom: 10, width: 300 }}>
                    <Button onClick={() => onClose()} variant="contained" autoFocus>Cancel</Button>
                    <Button
                        style={{ marginLeft: 10 }}
                        disabled={!formIsValid}
                        onClick={() => save()}
                        color="primary"
                        variant="contained" autoFocus>salvar</Button>
                </div>
            </DialogContent>
        </Dialog>
    )
}