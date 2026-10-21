import React, { useState, useEffect } from 'react'
import { Alert, Box, FormControlLabel, Switch, Typography } from '@mui/material'

import { FormDialog, MoneyField, DateField, Money, HelpTip } from '../../components'
import { dateToString } from '../../helpers'

export function EditInstallmentDialog({ installment, onCancel, onSave }) {
  const [paidValue, setPaidValue] = useState(0)
  const [paidDate, setPaidDate] = useState(null)
  const [exempt, setExempt] = useState(false)

  useEffect(() => {
    if (installment) {
      setPaidDate(installment.paidDate ? new Date(installment.paidDate) : null)
      setPaidValue(installment.paidValue || 0)
      setExempt(!!installment.exempt)
    }
  }, [installment])

  // Valor pago e data de pagamento andam juntos: ou os dois, ou nenhum.
  const invalid = !exempt && ((!!paidDate && !(paidValue > 0)) || (!paidDate && paidValue > 0))

  function save() {
    onSave({
      ...installment,
      paidValue: exempt || !(paidValue > 0) ? undefined : paidValue,
      paidDate: exempt || !paidDate ? undefined : paidDate,
      exempt
    })
  }

  return (
    <FormDialog open={!!installment} title={`Parcela nº ${(installment || {}).number}`}
      subtitle="Informe quando e quanto você pagou. Deixe em branco se ainda não pagou."
      onClose={onCancel} onSave={save} saveLabel="Aplicar" saveDisabled={invalid} maxWidth="xs">
      {installment &&
        <Box sx={{ p: 1.5, borderRadius: 2, bgcolor: 'action.hover' }}>
          <Typography variant="body2">Vence em <b>{dateToString(installment.date)}</b></Typography>
          <Typography variant="body2">Valor da parcela: <Money value={installment.value} bold /></Typography>
        </Box>
      }
      <FormControlLabel
        control={<Switch checked={exempt} onChange={e => setExempt(e.target.checked)} />}
        label={<>Isentar esta parcela<HelpTip id="isentar" /></>} />
      {!exempt && <>
        <MoneyField label="Valor pago" value={paidValue} onChange={setPaidValue} helperText="Pagou com desconto ou juros? Coloque o valor real." />
        <DateField label="Data do pagamento" value={paidDate} onChange={setPaidDate} clearable />
      </>}
      {invalid && <Alert severity="warning">Preencha o valor pago e a data juntos (ou deixe os dois vazios).</Alert>}
    </FormDialog>
  )
}
