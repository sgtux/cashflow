import React from 'react'

import {
    Dialog,
    DialogContent,
    DialogTitle,
    Button,
    Zoom,
    List,
    ListItem,
    ListItemText
} from '@material-ui/core'

import { dateToString, toReal } from '../../../helpers'

export function DailyExpensesDetailModal({ onClose, dailyExpense }) {

    return (
        <Dialog
            open={!!dailyExpense.id}
            onClose={() => onClose()}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}>
            <DialogTitle id="alert-dialog-title" style={{ textAlign: 'center' }}>
                <div style={{ color: 'gray' }}>
                    <span>
                        {dailyExpense.shopName}
                    </span>
                </div>
            </DialogTitle>
            <DialogContent>
                <div style={{ color: 'gray' }}>
                    {dateToString(dailyExpense.date)}
                </div>
                {
                    dailyExpense.items &&
                    <List dense={true}>
                        <ListItem>
                            <ListItemText
                                style={{ width: '200px' }}
                                secondary="Estabelecimento"
                            />
                            <ListItemText
                                style={{ width: '100px' }}
                                secondary="Preço"
                            />
                            <ListItemText
                                style={{ width: '100px' }}
                                secondary="Quantidade"
                            />
                            <ListItemText
                                style={{ width: '100px' }}
                                secondary="Total"
                            />
                        </ListItem>
                        {dailyExpense.items.map(p =>
                            <ListItem key={p.id}>
                                <ListItemText
                                    style={{ width: '200px' }}
                                    secondary={p.itemName}
                                />
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={toReal(p.price)}
                                />
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={p.amount}
                                />
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={toReal(p.totalPrice)}
                                />
                            </ListItem>
                        )}
                    </List>
                }
            </DialogContent>
            <div style={{ margin: '20px', textAlign: 'end', color: 'gray' }}>
                <span style={{ fontWeight: 'bold', marginRight: 10 }}>Total:</span>{toReal(dailyExpense.totalPrice)}
            </div>
            <div style={{ marginBottom: '20px', textAlign: 'center' }}>
                <Button size="large" onClick={() => onClose()} variant="contained" autoFocus>Fechar</Button>
            </div>
        </Dialog>
    )
}