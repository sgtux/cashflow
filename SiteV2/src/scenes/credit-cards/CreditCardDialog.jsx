import React, { useState, useEffect } from 'react'
import { Box, TextField, Typography } from '@mui/material'

import { FormDialog, Term } from '../../components'

const onlyDay = text => {
  const n = parseInt((text || '').replace(/\D/g, '').slice(0, 2) || '0', 10)
  return n > 31 ? '31' : (n ? String(n) : '')
}

export function CreditCardDialog({ card, onClose, onSave }) {
  const [name, setName] = useState('')
  const [invoiceClosingDay, setInvoiceClosingDay] = useState('')
  const [invoiceDueDay, setInvoiceDueDay] = useState('')

  useEffect(() => {
    if (card) {
      setName(card.name || '')
      setInvoiceClosingDay(card.invoiceClosingDay ? String(card.invoiceClosingDay) : '')
      setInvoiceDueDay(card.invoiceDueDay ? String(card.invoiceDueDay) : '')
    }
  }, [card])

  const valid = !!name.trim() && invoiceClosingDay > 0 && invoiceDueDay > 0

  return (
    <FormDialog open={!!card} title={card && card.id ? 'Editar cartão' : 'Novo cartão de crédito'}
      subtitle="Essas datas estão no aplicativo do banco ou na própria fatura."
      onClose={onClose}
      saveDisabled={!valid}
      onSave={() => onSave({ id: card.id, name, invoiceClosingDay, invoiceDueDay })}>
      <TextField label="Nome do cartão" placeholder="Ex.: Nubank, Itaú Visa" value={name} onChange={e => setName(e.target.value)} autoFocus />
      <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 2 }}>
        <TextField label="Dia do fechamento" value={invoiceClosingDay} onChange={e => setInvoiceClosingDay(onlyDay(e.target.value))}
          slotProps={{ htmlInput: { inputMode: 'numeric' } }} placeholder="Ex.: 5" />
        <TextField label="Dia do vencimento" value={invoiceDueDay} onChange={e => setInvoiceDueDay(onlyDay(e.target.value))}
          slotProps={{ htmlInput: { inputMode: 'numeric' } }} placeholder="Ex.: 15" />
      </Box>
      <Box sx={{ p: 1.5, borderRadius: 2, bgcolor: 'action.hover' }}>
        <Typography variant="body2">
          <Term id="fechamento">Fechamento</Term>: depois desse dia, as compras vão para a fatura do mês seguinte.<br />
          <Term id="vencimento">Vencimento</Term>: último dia para pagar sem juros.
        </Typography>
        {invoiceClosingDay > 0 && invoiceDueDay > 0 &&
          <Typography variant="body2" sx={{ mt: 1, fontWeight: 700 }}>
            Exemplo: uma compra no dia {Math.min(Number(invoiceClosingDay) + 1, 31)} só será paga no vencimento do dia {invoiceDueDay} do mês seguinte.
          </Typography>
        }
      </Box>
    </FormDialog>
  )
}
