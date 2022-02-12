import React, { useState, useEffect } from 'react'
import { useParams, Link, useNavigate } from 'react-router-dom'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import {
    Button,
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

import { InputMoney, DatePickerInput, DatePickerContainer } from '../../../components/inputs'

import { MainContainer, IconTextInput } from '../../../components/main'

import { earningService } from '../../../services'
import { toast, fromReal, toReal } from '../../../helpers'

export function EditEarning() {

    const [id, setId] = useState(0)
    const [description, setDescription] = useState('')
    const [value, setValue] = useState('')
    const [date, setDate] = useState('')
    const [type, setType] = useState('')
    const [types, setTypes] = useState([])
    const [formIsValid, setFormIsValid] = useState(false)
    const [loading, setLoading] = useState(false)

    const params = useParams()
    const navigate = useNavigate()

    useEffect(() => {
        earningService.getTypes()
            .then(res => setTypes(res))
            .catch(err => console.log(err))
        if (params.id > 0) {
            setLoading(true)
            setId(Number(params.id))
            earningService.get(params.id)
                .then(res => {
                    if (!res) {
                        navigate('/earnings')
                        return
                    }
                    setDescription(res.description)
                    setDate(new Date(res.date))
                    setValue(toReal(res.value))
                    setType(res.type)
                })
                .catch(err => console.log(err))
                .finally(() => setLoading(false))
        } else {
            setDate(new Date())
            setType(1)
        }
    }, [])

    useEffect(() => {
        setFormIsValid(description && date && fromReal(value) > 0 && type > 0)
    }, [description, value, date, type])

    function save() {
        setLoading(true)
        earningService.save({
            id,
            description,
            type,
            date,
            value: fromReal(value),

        }).then(() => {
            toast.success('Salvo com sucesso.')
            console.log(id)
            if (!id)
                navigate('/earnings')
        })
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Ganho" loading={loading}>
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
                <div style={{ margin: '10px', textAlign: 'right' }}>
                    <Link to="/earnings">
                        <Button variant="contained" autoFocus>Listagem</Button>
                    </Link>
                    <Button onClick={() => save()}
                        disabled={!formIsValid}
                        variant="contained"
                        color="primary"
                        style={{ marginLeft: 20, marginRight: 20 }}
                        autoFocus>salvar</Button>
                </div>
            </div>
        </MainContainer >
    )
}