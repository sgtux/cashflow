import React from 'react'
import {
    Checkbox,
    FormControl,
    FormControlLabel,
    InputLabel,
    Select
} from '@material-ui/core'

export function CreditCardBox({ cards, useCreditCard, useCreditCardChanged, card, cardChanged, invoice, invoiceChanged }) {
    return (
        <div hidden={!cards.length}>
            <FormControlLabel
                control={
                    <Checkbox
                        defaultChecked={useCreditCard}
                        onChange={(e, c) => useCreditCardChanged({ useCreditCard: c })}
                        color="primary"
                    />
                }
                label="Cartão de crédito"
            />
            {
                cards.length && useCreditCard ?
                    <span>
                        <FormControl style={{ marginLeft: '20px', marginRight: '20px' }}>
                            <InputLabel htmlFor="select-tipo">Cartão de crédito</InputLabel>
                            <Select style={{ width: '160px' }} value={card}
                                onChange={e => cardChanged(e.target.value)}>
                                {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
                            </Select>
                        </FormControl>
                        <FormControlLabel label="Fatura"
                            control={<Checkbox
                                defaultChecked={invoice}
                                onChange={(e, c) => invoiceChanged(c)}
                                color="primary"
                            />} />
                    </span>
                    : null
            }
        </div>
    )
}