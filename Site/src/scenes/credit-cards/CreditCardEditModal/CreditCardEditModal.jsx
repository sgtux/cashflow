import React, { useState, useEffect } from 'react'

import {
    Dialog,
    DialogContent,
    DialogTitle,
    Button,
    Zoom
} from '@material-ui/core'

import CardIcon from '@material-ui/icons/CreditCardOutlined'

import IconTextInput from '../../../components/main/IconTextInput'

export function DailyExpensesDetailModal({ onClose, onSave, card }) {

    const [name, setName] = useState('')
    const [invoiceDay, setInvoiceDay] = useState('')
    const [cardIdValid, setCardIdValid] = useState(false)

    useEffect(() => setCardIdValid(!!name && invoiceDay > 0), [name, invoiceDay])

    useEffect(() => {
        if (card) {
            setInvoiceDay((card.invoiceDay || '') + '')
            setName(card.name || '')
        }
    }, [card])

    return (
        <Dialog
            open={!!card}
            onClose={() => onClose()}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}>
            <DialogTitle id="alert-dialog-title" style={{ textAlign: 'center' }}>
                <div style={{ color: 'gray' }}>
                    <span>
                        Cartão de Crédito
                    </span>
                </div>
            </DialogTitle>
            <DialogContent>
                <IconTextInput
                    label="Nome do cartão"
                    value={name}
                    onChange={(e) => setName(e.value)}
                    Icon={<CardIcon />}
                />
                <br />
                <IconTextInput
                    label="Dia da fatura"
                    value={invoiceDay}
                    onChange={e => setInvoiceDay((e.value || '').replace(/[^0-9]/g, ''))}
                    Icon={<CardIcon />}
                />
            </DialogContent>
            <div style={{ marginBottom: '20px', textAlign: 'center' }}>
                <Button color="primary" onClick={() => onClose()}>
                    Cancelar
                </Button>
                <Button variant="contained" color="primary" disabled={!cardIdValid}
                    onClick={() => onSave({ name, invoiceDay, id: card.id })}>
                    Salvar
                </Button>
            </div>
        </Dialog>
    )
}