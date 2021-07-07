import React from 'react'
import {
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

import { Container, MenuItemSpan } from './styles'

export function PaymentTypeBox({ types, paymentType, paymentTypeChanged }) {

    return (
        <Container>
            <InputLabel htmlFor="select-tipo">Tipo</InputLabel>
            <Select
                value={paymentType || ''}
                color="secondary"
                onChange={e => paymentTypeChanged(e.target.value)}>
                {
                    types.map((p, i) => <MenuItem key={i} value={p.id}>
                        <MenuItemSpan gain={p.in}>{p.description}</MenuItemSpan>
                    </MenuItem>)
                }
            </Select>
        </Container>
    )
}