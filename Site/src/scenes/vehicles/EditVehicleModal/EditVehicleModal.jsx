import React, { useState, useEffect } from 'react'
import { Button, Dialog, DialogContent, Zoom, IconButton } from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
} from '@material-ui/icons'

import { toReal, fromReal, dateToString } from '../../../helpers'
import { vehicleService } from '../../../services'
import { InputMoney, InputText } from '../../../components/inputs'
import { IconTextInput } from '../../../components/main'

import { FuelExpensesTable } from './styles'

const MAX_MILIAGE = 10000000

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
            setFuelExpenses(vehicle.fuelExpenses)
        }
    }, [vehicle])

    function refresh() {
        vehicleService.get(vehicle.id)
            .then(res => setFuelExpenses(res.fuelExpenses))
    }

    useEffect(() => {
        if (miliage > MAX_MILIAGE)
            setMiliage(MAX_MILIAGE + '')
        setFormIsValid(miliage > 0 && fromReal(pricePerLiter) > 0 && fromReal(valueSupplied) > 0 && date)
    }, [miliage, pricePerLiter, date, valueSupplied])

    function save() {
        const item = {
            id,
            date,
            miliage: Number(miliage),
            pricePerLiter: fromReal(pricePerLiter),
            valueSupplied: fromReal(valueSupplied),
            vehicleId: vehicle.id
        }
        vehicleService.saveFuelExpenses(item)
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

    function remove(id) {
        vehicleService.removeFuelExpenses(id)
            .then(() => setFuelExpenses(fuelExpenses.filter(p => p.id !== id)))
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
                <div style={{ fontFamily: '"Helvetica Neue", Helvetica, Arial, sans-serif', minWidth: 500, minHeight: 340 }}>
                    <h3 style={{ padding: 10, backgroundColor: '#ccc' }}>{vehicle && vehicle.description} (Gastos em Combustível)</h3>
                    <FuelExpensesTable>
                        <table>
                            <thead>
                                <tr>
                                    <th>Quilometragem</th>
                                    <th>Preço por Litro</th>
                                    <th>Valor Abastecido</th>
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
                    <div style={{ padding: 10 }}>
                        Quilometragem:
                        <InputText
                            value={miliage}
                            onChange={e => setMiliage((e.target.value).replace(/[^0-9]/g, ''))}
                        />
                        Preço por Litro: <InputMoney
                            onChangeText={e => setPricePerLiter(e)}
                            kind="money"
                            value={pricePerLiter} />
                        Valor Abastecido: <InputMoney
                            onChangeText={e => setValueSupplied(e)}
                            kind="money"
                            value={valueSupplied} />
                        Data: <DatePicker onChange={e => setDate(e)}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
                        <Button onClick={() => clear()} autoFocus>limpar</Button>
                        <Button onClick={() => save()}
                            disabled={!formIsValid}
                            variant="contained"
                            color="primary"
                            autoFocus>salvar</Button>
                        <div style={{ margin: '10px', textAlign: 'center' }}>
                            <Button onClick={() => onCancel()} variant="contained" autoFocus>fechar</Button>
                        </div>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}