import React from 'react'
import {
    FormControlLabel,
    FormControl,
    FormLabel,
    RadioGroup,
    Radio
} from '@material-ui/core'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { InputMoney } from '../../../../components/inputs'

export function CostDateConditionBox({ cost, costChanged, date, dateChanged, condition, conditionChanged }) {
    return (
        <div style={{ marginRight: '10px', marginTop: '10px', color: '#666' }}>
            <span>Valor:</span>
            <InputMoney
                onChangeText={e => costChanged(e)}
                kind="money"
                value={cost} />
            <span style={{ marginRight: 10 }}>Data:</span>
            <DatePicker onChange={e => dateChanged(e)}
                dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
            <FormControl style={{ marginTop: 20, marginBottom: 10 }} component="fieldset">
                <FormLabel style={{ fontSize: 12 }} component="legend">Condição:</FormLabel>
                <RadioGroup style={{ fontSize: 12 }} row value={condition} onChange={e => conditionChanged(e.target.value)}>
                    <FormControlLabel style={{ fontSize: 12 }} value={1} control={<Radio color="primary" />} label="À Vista" />
                    <FormControlLabel value={2} control={<Radio color="primary" />} label="Mensal" />
                    <FormControlLabel value={3} control={<Radio color="primary" />} label="Parcelado" />
                </RadioGroup>
            </FormControl>
        </div>
    )
}