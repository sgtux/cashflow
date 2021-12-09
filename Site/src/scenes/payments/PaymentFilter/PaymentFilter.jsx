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
    const [active, setActive] = useState(true)
    const [inactive, setInactive] = useState(true)
    const [inactiveFrom, setInactiveFrom] = useState(null)
    const [inactiveTo, setInactiveTo] = useState(null)

    function reset() {
        setDescription('')
        setActive(true)
        setInactive(true)
        setInactiveFrom(null)
        setInactiveTo(null)
    }

    function filter() {
        filterChanged({
            description,
            active: active && inactive ? undefined : active ? true : false,
            inactiveFrom,
            inactiveTo
        })
    }

    return (
        <div style={{ margin: 20 }}>
            <DatePickerContainer>
                <InputLabel>Descrição:</InputLabel>
                <InputText style={{ width: 120 }}
                    onChange={e => setDescription(e.target.value)}
                    value={description} />
                <br />
                <FormControlLabel label="Ativo"
                    control={<Checkbox
                        checked={active}
                        onChange={(e, c) => setActive(c)}
                        color="primary"
                    />} />
                <br />
                <FormControlLabel label="Inativo"
                    control={<Checkbox
                        checked={inactive}
                        onChange={(e, c) => setInactive(c)}
                        color="primary"
                    />} />
                {inactive &&
                    <span>
                        <InputLabel>Desde:</InputLabel>
                        <DatePicker customInput={<DatePickerInput style={{ width: 150 }} />}
                            onChange={e => setInactiveFrom(e)}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={inactiveFrom} />
                        <InputLabel>Até:</InputLabel>
                        <DatePicker customInput={<DatePickerInput style={{ width: 150 }} />}
                            onChange={e => setInactiveTo(e)}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={inactiveTo} />
                    </span>
                }
                <div style={{ textAlign: 'end' }}>
                    <Button style={{ marginRight: 10 }} variant="contained" onClick={() => reset()}>Limpar</Button>
                    <Button variant="contained" onClick={() => filter()}>Filtrar</Button>
                </div>

            </DatePickerContainer>
        </div>
    )
}