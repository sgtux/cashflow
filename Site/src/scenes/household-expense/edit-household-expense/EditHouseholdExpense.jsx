import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import { Link, useParams, useNavigate } from 'react-router-dom'

import {
    TextField,
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

import { MainContainer } from '../../../components/main'
import { InputMoney, DatePickerContainer, DatePickerInput } from '../../../components/inputs'
import { toReal, fromReal } from '../../../helpers'

import { householdExpenseService, vehicleService } from '../../../services'

export function EditHouseholdExpense() {

    const [id, setId] = useState(0)
    const [loading, setLoading] = useState(false)
    const [description, setDescription] = useState('')
    const [date, setDate] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [vehicleId, setVehicleId] = useState('')
    const [vehicles, setVehicles] = useState([])
    const [types, setTypes] = useState([])
    const [type, setType] = useState('')

    const params = useParams()
    const navigate = useNavigate()

    useEffect(async () => {
        setLoading(true)
        try {
            const taskVehicle = vehicleService.getAll()
            const taskTypes = householdExpenseService.getTypes()
            let expense = null
            if (Number(params.id) > 0) {
                expense = await householdExpenseService.get(params.id)
                if (!expense) {
                    navigate('/household-expense')
                    return
                }

                setId(expense.id)
                setDescription(expense.description)
                setDate(new Date(expense.date))
                setValue(toReal(expense.value))
            }
            const list = await taskVehicle
            const listTypes = await taskTypes
            setVehicles(list)
            setTypes(listTypes)
            if (expense) {
                setVehicleId(expense.vehicleId)
                setType(expense.type)
            }

            setLoading(false)
        } catch (ex) {
            console.log(ex)
            setLoading(false)
        }
    }, [])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0, Number(type) > 0)
    }, [description, date, value, type])

    function save() {
        setLoading(true)
        householdExpenseService.save({
            id,
            description,
            date,
            value: fromReal(value),
            vehicleId: vehicleId ? Number(vehicleId) : 0,
            type: Number(type)
        })
            .then(() => navigate('/household-expenses'))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Despesas Domésticas" loading={loading}>
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
                    onChangeText={e => setValue(e)}
                    kind="money"
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
            {!!vehicles.length &&
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
            <div style={{ display: 'flex', justifyContent: 'end' }}>
                <Link to="/household-expenses">
                    <Button onClick={() => { }} variant="contained" autoFocus>Lista de Despesas</Button>
                </Link>

                <Button
                    style={{ marginLeft: 10 }}
                    disabled={loading || !formIsValid}
                    onClick={() => save()}
                    color="primary"
                    variant="contained" autoFocus>salvar</Button>
            </div>
        </MainContainer>
    )
}