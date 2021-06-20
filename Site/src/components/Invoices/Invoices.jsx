import React, { useState, useEffect } from 'react'
import {
    List,
    ListItem,
    Typography,
    GridList,
    GridListTile
} from '@material-ui/core'

import { toReal } from '../../helpers/utils'

export function Invoices(props) {

    const [cards, setCards] = useState([])
    const [showing, setShowing] = useState(false)
    const [total, setTotal] = useState(0)

    useEffect(() => {
        const temp = []
        let totalTemp = 0
        props.payments.forEach(p => {
            if (p.creditCard && !temp.find(x => x.id === p.creditCard.id))
                temp.push(p.creditCard)
            totalTemp += p.cost
        })
        temp.forEach(c => {
            const pays = props.payments.filter(x => x.creditCard && x.creditCard.id === c.id)
            c.payments = pays
            c.cost = pays.length ? pays.map(p => p.cost).reduce((sum, p) => sum + p) : 0
        })
        setCards(temp)
        setShowing(showing)
        setTotal(total)
    })

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
                                                <Typography component="span" color={p.type === 1 ? 'primary' : 'secondary'}>
                                                    {toReal(p.cost)}
                                                </Typography>
                                            </GridListTile>
                                        </GridList>
                                    </ListItem>
                                )}
                            </List>
                            <div style={{ textAlign: 'right' }}>
                                <span style={{
                                    fontSize: '12px',
                                    color: Colors.AppRed,
                                    marginTop: '6px',
                                    padding: '3px',
                                    fontWeight: 'bold'
                                }}>
                                    {toReal(c.cost)}
                                </span>
                            </div>
                            <hr />
                        </div>
                    )}
                </div>
                <div style={{ textAlign: 'right' }}>
                    <span style={{
                        fontSize: '14px',
                        color: Colors.AppRed,
                        marginTop: '6px',
                        padding: '3px',
                        fontWeight: 'bold'
                    }}>
                        {toReal(total)}
                    </span>
                </div>
            </fieldset>
            : null
    )
}