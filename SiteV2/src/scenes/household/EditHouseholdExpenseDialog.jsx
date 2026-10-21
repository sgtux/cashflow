import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import { Box, MenuItem, TextField } from '@mui/material'

import { FormDialog, MoneyField, DateField, SelectField, ExpenseTypeAvatar, ExpenseTypes } from '../../components'
import { householdExpenseService, vehicleService, creditCardService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { toast } from '../../helpers'

export function EditHouseholdExpenseDialog({ expense, onClose, onSave }) {
  const [description, setDescription] = useState('')
  const [date, setDate] = useState(null)
  const [value, setValue] = useState(0)
  const [type, setType] = useState('')
  const [creditCardId, setCreditCardId] = useState(0)
  const [vehicleId, setVehicleId] = useState(0)
  const [types, setTypes] = useState([])
  const [cards, setCards] = useState([])
  const [vehicles, setVehicles] = useState([])

  const dispatch = useDispatch()

  useEffect(() => {
    if (!expense)
      return
    setDescription(expense.description || '')
    setDate(expense.date ? new Date(expense.date) : new Date())
    setValue(expense.value || 0)
    setType(expense.type || '')
    setCreditCardId(expense.creditCardId || 0)
    setVehicleId(expense.vehicleId || 0)

    dispatch(showGlobalLoader())
    Promise.all([vehicleService.getAll(false), householdExpenseService.getTypes(), creditCardService.get()])
      .then(([listVehicles, listTypes, listCards]) => {
        setVehicles(listVehicles || [])
        setTypes(listTypes || [])
        setCards(listCards || [])
      })
      .catch(ex => console.log(ex))
      .finally(() => dispatch(hideGlobalLoader()))
  }, [expense])

  const valid = !!description.trim() && !!date && value > 0 && Number(type) > 0

  function save() {
    householdExpenseService.save({
      id: expense.id || 0,
      description,
      date,
      value,
      vehicleId: vehicleId ? Number(vehicleId) : 0,
      type: Number(type),
      creditCardId: creditCardId ? Number(creditCardId) : 0
    })
      .then(() => {
        toast.success('Despesa anotada!')
        onSave()
      })
      .catch(err => console.log(err))
  }

  return (
    <FormDialog
      open={!!expense}
      title={expense && expense.id ? 'Editar despesa' : 'Anotar despesa'}
      subtitle="Quanto mais você anota, mais claro fica para onde vai o seu dinheiro."
      onClose={onClose}
      onSave={save}
      saveDisabled={!valid}>
      <TextField label="Com o que você gastou?" placeholder="Ex.: Mercado da semana, almoço, Uber"
        value={description} onChange={e => setDescription(e.target.value)} autoFocus />
      <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' }, gap: 2.5 }}>
        <MoneyField label="Quanto?" value={value} onChange={setValue} />
        <DateField label="Quando?" value={date} onChange={setDate} />
      </Box>
      <TextField select label="Tipo de gasto" value={type || ''} onChange={e => setType(e.target.value)}
        helperText="Separar por tipo mostra em que você mais gasta.">
        {types.map(p =>
          <MenuItem key={p.id} value={p.id}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
              <ExpenseTypeAvatar type={p.id} size={26} /> {p.description}
            </Box>
          </MenuItem>
        )}
      </TextField>
      {!!cards.length &&
        <SelectField label="Pagou no cartão de crédito?" help="cartao" value={creditCardId || 0} onChange={setCreditCardId}
          emptyLabel="Não (dinheiro, Pix ou débito)"
          options={cards.map(p => ({ value: p.id, label: p.name }))}
          helperText="No cartão, o valor só sai do seu bolso quando a fatura vencer." />
      }
      {!!vehicles.length && Number(type) === ExpenseTypes.Vehicle &&
        <SelectField label="Qual veículo?" value={vehicleId || 0} onChange={setVehicleId}
          emptyLabel="Nenhum em específico"
          options={vehicles.map(p => ({ value: p.id, label: p.description }))} />
      }
    </FormDialog>
  )
}
