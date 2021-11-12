import React, { useState, useEffect } from 'react'

import {
    FormControl,
    InputLabel,
    MenuItem,
    Select
} from '@material-ui/core'

export function ConditionCreditCardBox({ cards, card, cardChanged, condition, conditionChanged }) {

    const [showCreditCard, setShowCreditCard] = useState(false)

    useEffect(() => setShowCreditCard(cards.length && [2, 3].includes(condition)), [condition])

    return (
        <div style={{ marginTop: 10 }}>
            <FormControl component="fieldset" style={{ marginRight: 20 }}>
                <InputLabel htmlFor="select-tipo">Condição</InputLabel>
                <Select style={{ width: '250px' }} value={condition}
                    onChange={e => conditionChanged(e.target.value)}>
                    <MenuItem value={1}>À Vista</MenuItem>
                    <MenuItem value={2}>Mensal</MenuItem>
                    <MenuItem value={3}>Parcelado</MenuItem>
                </Select>
            </FormControl>
            <span hidden={!showCreditCard}>
                <FormControl>
                    <InputLabel htmlFor="select-tipo">Cartão de Crédito</InputLabel>
                    <Select style={{ width: '200px' }} value={card || ''}
                        onChange={e => cardChanged(e.target.value)}>
                        <MenuItem value={0}><span style={{ color: 'gray' }}>LIMPAR</span></MenuItem>
                        {cards.map(p => <MenuItem key={p.id} value={p.id}>{p.name}</MenuItem>)}
                    </Select>
                </FormControl>
            </span>
        </div>
    )
}