import React, { useEffect, useState } from 'react'
import { FormControlLabel, Switch, TextField } from '@mui/material'

import { FormDialog } from '../../components'

export function VehicleDialog({ vehicle, onSave, onCancel }) {
  const [description, setDescription] = useState('')
  const [active, setActive] = useState(true)

  useEffect(() => {
    if (vehicle) {
      setDescription(vehicle.description || '')
      setActive(vehicle.id ? !!vehicle.active : true)
    }
  }, [vehicle])

  return (
    <FormDialog open={!!vehicle} title={vehicle && vehicle.id ? 'Editar veículo' : 'Novo veículo'} maxWidth="xs"
      onClose={onCancel}
      saveDisabled={!description.trim()}
      onSave={() => onSave({ id: vehicle.id, description, active })}>
      <TextField label="Nome do veículo" placeholder="Ex.: Moto CG 160, Carro da família" value={description}
        onChange={e => setDescription(e.target.value)} autoFocus />
      <FormControlLabel control={<Switch checked={active} onChange={e => setActive(e.target.checked)} />}
        label="Ainda uso este veículo" />
    </FormDialog>
  )
}
