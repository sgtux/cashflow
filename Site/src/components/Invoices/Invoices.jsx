import React, { useState, useEffect } from 'react'
import {
    List,
    ListItem,
    GridList,
    GridListTile
} from '@material-ui/core'

import { toReal } from '../../helpers/utils'

import { InvoiceCost, InvoiceTotalCost, InvoiceCostSmall } from './styles'

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
            totalTemp += p.cost
        })
        cardsTemp.forEach(c => {
            const pays = props.payments.filter(x => x.creditCard && x.creditCard.id === c.id)
            c.payments = pays
            c.cost = pays.length ? pays.map(p => p.cost).reduce((sum, p) => sum + p) : 0
        })
        setCards(cardsTemp)
        setShowing(showing)
        setTotal(totalTemp)
    }, [])

    return (
        cards.length ?
            <fieldset style={{ marginLeft: '10px', marginRight: '30px', color: '#666' }}>
                <legend>
                    <span onClick={() => setShowing(!showing)}
                        style={{ cursor: 'pointer', fontWeight: 'bold' }}>FATURAS</span>
                </legend>
                <div hidden={!showing} style={{ marginLeft: '50px' }}>
                    {cards.map((c, j) =>
                        <div key={j}>
                            <span style={{ fontWeight: 'bold' }}>{c.name}</span>
                            <List dense={true} style={{ marginLeft: '50px' }}>
                                {c.payments.map((p, k) =>
                                    <ListItem key={k}>
                                        <GridList cellHeight={18} cols={5} style={{ width: '100%' }}>
                                            <GridListTile cols={3}>
                                                <span>{p.description}</span>
                                            </GridListTile>
                                            <GridListTile cols={1} style={{ textAlign: 'center' }}>
                                                <span style={{ fontFamily: '"Roboto", "Helvetica", "Arial", "sans-serif"' }}>{p.fixedPayment ? '' : `${p.number}/${p.qtdInstallments}`}</span>
                                            </GridListTile>
                                            <GridListTile cols={1} style={{ textAlign: 'center' }}>
                                                <InvoiceCostSmall in={p.in}>{toReal(p.cost)}</InvoiceCostSmall>
                                            </GridListTile>
                                        </GridList>
                                    </ListItem>
                                )}
                            </List>
                            <div style={{ textAlign: 'right' }}>
                                <InvoiceCost>{toReal(c.cost)}</InvoiceCost>
                            </div>
                            <hr />
                        </div>
                    )}
                </div>
                <div style={{ textAlign: 'right' }}>
                    <InvoiceTotalCost>{toReal(total)}</InvoiceTotalCost>
                </div>
            </fieldset>
            : null
    )
}