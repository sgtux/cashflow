import React, { useState } from 'react'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import {
    FormControlLabel,
    Checkbox,
    Button
} from '@material-ui/core'

import { InputText, DatePickerInput, DatePickerContainer, InputLabel } from '../../../components/inputs'

export function PaymentFilter({ filterChanged }) {

    const [description, setDescription] = useState('')
    const [inProgress, setInProgress] = useState(true)
    const [done, setDone] = useState(false)
    const [startDate, setStartDate] = useState(null)
    const [endDate, setEndDate] = useState(null)

    function reset() {
        setDescription('')
        setInProgress(true)
        setDone(false)
        setStartDate(null)
        setEndDate(null)
    }

    function filter() {
        filterChanged({
            description,
            done: inProgress && done ? undefined : done ? true : false,
            startDate,
            endDate
        })
    }

    return (
        <div style={{ margin: 20 }}>
            <DatePickerContainer>
                <InputLabel>Descrição:</InputLabel>
                <InputText style={{ width: 120 }}
                    onChange={e => setDescription(e.target.value)}
                    value={description} />
                <InputLabel>Desde:</InputLabel>
                <DatePicker customInput={<DatePickerInput style={{ width: 150 }} />}
                    onChange={e => setStartDate(e)}
                    dateFormat="dd/MM/yyyy" locale={ptBr} selected={startDate} />
                <InputLabel>Até:</InputLabel>
                <DatePicker customInput={<DatePickerInput style={{ width: 150 }} />}
                    onChange={e => setEndDate(e)}
                    dateFormat="dd/MM/yyyy" locale={ptBr} selected={endDate} />
                <br />
                <FormControlLabel label="Em Andamento"
                    control={<Checkbox
                        checked={inProgress}
                        onChange={(e, c) => setInProgress(c)}
                        color="primary"
                    />} />
                <FormControlLabel label="Concluído"
                    control={<Checkbox
                        checked={done}
                        onChange={(e, c) => setDone(c)}
                        color="primary"
                    />} />
                <div style={{ textAlign: 'end' }}>
                    <Button style={{ marginRight: 10 }} variant="contained" onClick={() => reset()}>Limpar</Button>
                    <Button variant="contained" onClick={() => filter()}>Filtrar</Button>
                </div>

            </DatePickerContainer>
        </div>
    )
}