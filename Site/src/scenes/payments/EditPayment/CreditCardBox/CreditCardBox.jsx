import React from 'react'
import {
    Checkbox,
    FormControl,
    FormControlLabel,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

export function CreditCardBox({ cards, useCreditCard, useCreditCardChanged, card, cardChanged, invoice, invoiceChanged }) {

    return (
        <div hidden={!cards.length}>
            <FormControlLabel
                control={
                    <Checkbox
                        checked={useCreditCard}
                        onChange={(e, c) => useCreditCardChanged(c)}
                        color="primary"
                    />
                }
                label="Cartão de crédito"
            />
            {
                cards.length && useCreditCard ?
                    <div>
                        <FormControl style={{ marginLeft: '20px', marginRight: '20px' }}>
                            <InputLabel htmlFor="select-tipo">Selecione</InputLabel>
                            <Select style={{ width: '160px' }} value={card || ''}
                                onChange={e => cardChanged(e.target.value)}>
                                {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
                            </Select>
                        </FormControl>
                        <FormControlLabel label="Fatura"
                            control={<Checkbox
                                checked={invoice}
                                onChange={(e, c) => invoiceChanged(c)}
                                color="primary"
                            />} />
                    </div>
                    : undefined
            }
        </div>
    )
}