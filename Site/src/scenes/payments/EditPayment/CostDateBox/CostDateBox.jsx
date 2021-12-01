import React from 'react'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import { InputMoney, DatePickerInput, DatePickerContainer } from '../../../../components/inputs'

export function CostDateBox({ cost, costChanged, date, dateChanged }) {
    return (
        <DatePickerContainer style={{ marginRight: '10px', marginTop: '10px', color: '#666' }}>
            <span>Valor:</span>
            <InputMoney
                style={{ fontSize: 16, width: 190 }}
                onChangeText={e => costChanged(e)}
                kind="money"
                value={cost} />
            <span style={{ marginLeft: 6 }}>Data:</span>
            <DatePicker customInput={<DatePickerInput style={{ width: 150 }} />} onChange={e => dateChanged(e)}
                dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
        </DatePickerContainer>
    )
}