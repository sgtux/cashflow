import React from 'react'
import {
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

export function PaymentTypeBox({ paymentType, paymentTypeChanged }) {
    return (
        <FormControl style={{ width: '200px', marginLeft: '20px', marginTop: '10px' }}>
            <InputLabel htmlFor="select-tipo">Tipo</InputLabel>
            <Select
                value={paymentType}
                color="secondary"
                onChange={e => paymentTypeChanged(e.target.value)}>
                { }
                <MenuItem key={1} value={1}><span style={{ color: 'green', fontWeight: 'bold' }}>RENDA</span></MenuItem>
                <MenuItem key={2} value={2}><span style={{ color: 'red', fontWeight: 'bold' }}>DESPESA</span></MenuItem>
            </Select>
        </FormControl>
    )
}