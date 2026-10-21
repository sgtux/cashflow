import React, { useState, useEffect } from 'react'
import { TextField } from '@mui/material'

import { FormDialog, MoneyField, DateField } from '../../components'
import { earningService } from '../../services'
import { toast } from '../../helpers'

export function EditEarningDialog({ earning, onClose, onSave }) {
  const [description, setDescription] = useState('')
  const [value, setValue] = useState(0)
  const [date, setDate] = useState(null)

  useEffect(() => {
    if (earning) {
      setDescription(earning.description || '')
      setDate(earning.date ? new Date(earning.date) : new Date())
      setValue(earning.value || 0)
    }
  }, [earning])

  const valid = !!description.trim() && !!date && value > 0

  function save() {
    earningService.save({
      id: earning.id || 0,
      description,
      date,
      value
    }).then(() => {
      toast.success('Ganho salvo!')
      onSave()
    }).catch(err => console.log(err))
  }

  return (
    <FormDialog
      open={!!earning}
      title={earning && earning.id ? 'Editar ganho' : 'Novo ganho'}
      subtitle="Registre qualquer dinheiro que entrou para você."
      onClose={onClose}
      onSave={save}
      saveDisabled={!valid}>
      <TextField label="De onde veio?" placeholder="Ex.: Freela de design, venda da bicicleta"
        value={description} onChange={e => setDescription(e.target.value)} autoFocus />
      <MoneyField label="Quanto entrou?" value={value} onChange={setValue} />
      <DateField label="Quando entrou?" value={date} onChange={setDate} helperText="A data em que o dinheiro caiu na sua conta." />
    </FormDialog>
  )
}
