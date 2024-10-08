import React, { useState, useEffect } from 'react'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import {
    Button,
    Dialog,
    DialogContent,
    Zoom,
    IconButton,
    Card
} from '@mui/material'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
} from '@mui/icons-material'

import { toReal, fromReal, dateToString } from '../../../helpers'
import { vehicleService } from '../../../services'
import { InputMoney, InputText, DatePickerContainer, DatePickerInput } from '../../../components/inputs'

import { FuelExpensesTable } from './styles'

const MAX_VALUE = 999999999

export function EditVehicleModal({ vehicle, onCancel }) {

    const [id, setId] = useState(0)
    const [date, setDate] = useState('')
    const [miliage, setMiliage] = useState('')
    const [pricePerLiter, setPricePerLiter] = useState('')
    const [valueSupplied, setValueSupplied] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [fuelExpenses, setFuelExpenses] = useState([])

    useEffect(() => {
        if (vehicle) {
            refresh()
        }
    }, [vehicle])

    function refresh() {
        vehicleService.get(vehicle.id)
            .then(res => setFuelExpenses(res.fuelExpenses))
    }

    useEffect(() => {
        if (miliage > MAX_VALUE)
            setMiliage(MAX_VALUE + '')
        if (fromReal(pricePerLiter) > MAX_VALUE)
            setPricePerLiter(MAX_VALUE + '')
        if (fromReal(valueSupplied) > MAX_VALUE)
            setValueSupplied(MAX_VALUE + '')
        setFormIsValid(miliage > 0 && fromReal(pricePerLiter) > 0 && fromReal(valueSupplied) > 0 && date)
    }, [miliage, pricePerLiter, date, valueSupplied])

    function save() {
        const item = {
            id,
            date,
            miliage: Number(miliage),
            pricePerLiter: fromReal(pricePerLiter),
            valueSupplied: fromReal(valueSupplied),
            vehicleId: vehicle.id,
        }
        vehicleService.saveFuelExpense(item)
            .then(() => {
                clear()
                refresh()
            })
    }

    function edit(item) {
        setId(item.id)
        setMiliage(item.miliage + '')
        setPricePerLiter(toReal(item.pricePerLiter))
        setValueSupplied(toReal(item.valueSupplied))
        setDate(new Date(item.date))
    }

    function clear() {
        setMiliage('')
        setPricePerLiter('')
        setValueSupplied('')
        setDate('')
        setId(0)
    }

    function remove(removeId) {
        vehicleService.removeFuelExpense(removeId)
            .then(() => {
                setFuelExpenses(fuelExpenses.filter(p => p.id !== removeId))
                clear()
            })
    }

    return (
        <Dialog
            open={!!vehicle}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            fullScreen={true}
            TransitionComponent={Zoom}>
            <DialogContent>
                <div style={{ fontFamily: 'GraphikRegular', minWidth: 500, minHeight: 340 }}>
                    <h3 style={{ padding: 10, backgroundColor: '#ccc' }}>{vehicle && vehicle.description} (Gastos em Combustível)</h3>
                    <FuelExpensesTable>
                        <table>
                            <thead>
                                <tr>
                                    <th>Quilometragem</th>
                                    <th>Preço por Litro</th>
                                    <th>Valor Abastecido</th>
                                    <th>Litros Abastecidos</th>
                                    <th>Data</th>
                                    <th>Ações</th>
                                </tr>
                            </thead>
                            <tbody>
                                {fuelExpenses.map((p, i) =>
                                    <tr key={i}>
                                        <td>{p.miliage} Km</td>
                                        <td>{toReal(p.pricePerLiter)}</td>
                                        <td>{toReal(p.valueSupplied)}</td>
                                        <td>{p.litersSupplied}</td>
                                        <td>{dateToString(p.date)}</td>
                                        <td>
                                            <IconButton onClick={() => edit(p)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => remove(p.id)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </FuelExpensesTable>
                    <Card>
                        <DatePickerContainer style={{ padding: 10 }}>
                            Quilometragem:
                            <InputText
                                value={miliage}
                                onChange={e => setMiliage((e.target.value).replace(/[^0-9]/g, ''))}
                            />
                            Preço por Litro: <InputMoney
                                onChangeValue={(event, value, maskedValue) => setPricePerLiter(value)}
                                value={pricePerLiter} />
                            Valor Abastecido: <InputMoney
                                onChangeValue={(event, value, maskedValue) => setValueSupplied(value)}
                                kind="money"
                                value={valueSupplied} />
                            Data: <DatePicker onChange={e => setDate(e)}
                                customInput={<DatePickerInput />}
                                dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
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
        </Dialog>
    )
}