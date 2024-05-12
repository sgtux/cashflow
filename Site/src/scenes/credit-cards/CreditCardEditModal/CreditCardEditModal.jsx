import React, { useState, useEffect } from 'react'

import {
    Dialog,
    DialogContent,
    DialogTitle,
    Button,
    Zoom
} from '@mui/material'

import CardIcon from '@mui/icons-material/CreditCardOutlined'

import IconTextInput from '../../../components/main/IconTextInput'

export function CreditCardDetailModal({ onClose, onSave, card }) {

    const [name, setName] = useState('')
    const [invoiceClosingDay, setInvoiceClosingDay] = useState('')
    const [invoiceDueDay, setInvoiceDueDay] = useState('')
    const [cardIdValid, setCardIdValid] = useState(false)

    useEffect(() => setCardIdValid(!!name && invoiceDueDay > 0 && invoiceClosingDay > 0), [name, invoiceClosingDay, invoiceDueDay])

    useEffect(() => {
        if (card) {
            setName(card.name || '')
            setInvoiceClosingDay((card.invoiceClosingDay || '') + '')
            setInvoiceDueDay((card.invoiceDueDay || '') + '')
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
                    label="Fechamento da fatura"
                    value={invoiceClosingDay}
                    onChange={e => setInvoiceClosingDay((e.value || '').replace(/[^0-9]/g, ''))}
                    Icon={<CardIcon />}
                />
                <br />
                <IconTextInput
                    label="Vencimento da fatura"
                    value={invoiceDueDay}
                    onChange={e => setInvoiceDueDay((e.value || '').replace(/[^0-9]/g, ''))}
                    Icon={<CardIcon />}
                />
            </DialogContent>
            <div style={{ marginBottom: '20px', textAlign: 'center' }}>
                <Button color="primary" onClick={() => onClose()}>
                    Cancelar
                </Button>
                <Button variant="contained" color="primary" disabled={!cardIdValid}
                    onClick={() => onSave({ name, invoiceDueDay, invoiceClosingDay, id: card.id })}>
                    Salvar
                </Button>
            </div>
        </Dialog>
    )
}