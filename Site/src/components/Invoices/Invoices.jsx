import React, { useState, useEffect } from 'react'
import {
    List,
    ListItem,
    ImageList,
    ImageListItem,
    Accordion,
    AccordionSummary,
    AccordionDetails,
    Typography
} from '@mui/material'

import { ExpandMore } from '@mui/icons-material'

import { toReal } from '../../helpers/utils'

import { InvoiceCost, InvoiceTotalCost, InvoiceCostSmall } from './styles'
import { MoneySpan } from '../../components'

export function Invoices(props) {

    const [cards, setCards] = useState([])
    const [showing, setShowing] = useState(false)
    const [total, setTotal] = useState(0)

    useEffect(() => {
        const cardsTemp = []
        let totalTemp = 0
        props.payments.forEach(p => {
            if (p.creditCard && !cardsTemp.find(x => x.id === p.creditCard.id))
                cardsTemp.push(p.creditCard)
            totalTemp += p.value
        })
        cardsTemp.forEach(c => {
            const pays = props.payments.filter(x => x.creditCard && x.creditCard.id === c.id)
            c.payments = pays
            c.value = pays.length ? pays.map(p => p.value).reduce((sum, p) => sum + p) : 0
        })
        setCards(cardsTemp)
        setShowing(showing)
        setTotal(totalTemp)
    }, [])

    return (
        cards.length ?
            <Accordion>
                <AccordionSummary expandIcon={<ExpandMore />}>
                    <Typography style={{ color: '#666' }}>Faturas - <MoneySpan bold>{toReal(total)}</MoneySpan></Typography>
                </AccordionSummary>
                <AccordionDetails>
                    <div style={{ marginLeft: 20, width: '100%', color: '#666' }}>
                        {cards.map((c, j) =>
                            <div key={j}>
                                <span style={{ fontWeight: 'bold' }}>{c.name}</span>
                                <List dense={true} style={{ marginLeft: 50, marginRight: 10 }}>
                                    {c.payments.map((p, k) =>
                                        <ListItem style={{ backgroundColor: k % 2 == 0 ? '#ddd' : '#eee' }} key={k}>
                                            <ImageList rowHeight={18} cols={5} style={{ width: '100%' }}>
                                                <ImageListItem cols={3}>
                                                    <span>{p.description}</span>
                                                </ImageListItem>
                                                <ImageListItem cols={1} style={{ textAlign: 'center' }}>
                                                    <span style={{ fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>{p.qtdInstallments ? `N°: ${p.number} - P: ${p.qtdPaidInstallments}/${p.qtdInstallments}` : ''}</span>
                                                </ImageListItem>
                                                <ImageListItem cols={1} style={{ textAlign: 'center' }}>
                                                    <InvoiceCostSmall in={p.in}>{toReal(p.value)}</InvoiceCostSmall>
                                                </ImageListItem>
                                            </ImageList>
                                        </ListItem>
                                    )}
                                </List>
                                <div style={{ textAlign: 'right' }}>
                                    <InvoiceCost>{toReal(c.value)}</InvoiceCost>
                                </div>
                            </div>
                        )}
                        <div style={{ textAlign: 'right', marginTop: 10 }}>
                            <InvoiceTotalCost>{toReal(total)}</InvoiceTotalCost>
                        </div>
                    </div>
                </AccordionDetails>
            </Accordion>
            : null
    )
}