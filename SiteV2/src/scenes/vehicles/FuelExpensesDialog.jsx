import React, { useState, useEffect } from 'react'
import {
  Box, Button, Card, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle,
  Table, TableBody, TableCell, TableHead, TablePagination, TableRow, TextField, Typography, useMediaQuery
} from '@mui/material'
import { useTheme } from '@mui/material/styles'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'

import { Money, MoneyField, DateField, ActionIcon, ConfirmDialog, HelpTip } from '../../components'
import { vehicleService } from '../../services'
import { dateToString, toThousandFormat, toDecimalText, toast } from '../../helpers'

const MAX_VALUE = 999999999

export function FuelExpensesDialog({ vehicle, onClose }) {
  const [id, setId] = useState(0)
  const [date, setDate] = useState(null)
  const [miliage, setMiliage] = useState('')
  const [pricePerLiter, setPricePerLiter] = useState(0)
  const [valueSupplied, setValueSupplied] = useState(0)
  const [fuelExpenses, setFuelExpenses] = useState([])
  const [loading, setLoading] = useState(false)
  const [rowsPerPage, setRowsPerPage] = useState(5)
  const [page, setPage] = useState(0)
  const [removeItem, setRemoveItem] = useState(null)

  const theme = useTheme()
  const fullScreen = useMediaQuery(theme.breakpoints.down('sm'))

  useEffect(() => {
    if (vehicle) {
      clear()
      setPage(0)
      refresh()
    }
  }, [vehicle])

  function refresh() {
    setLoading(true)
    vehicleService.get(vehicle.id)
      .then(res => setFuelExpenses((res || {}).fuelExpenses || []))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  function clear() {
    setId(0)
    setMiliage('')
    setPricePerLiter(0)
    setValueSupplied(0)
    setDate(null)
  }

  function edit(item) {
    setId(item.id)
    setMiliage(String(item.miliage))
    setPricePerLiter(item.pricePerLiter)
    setValueSupplied(item.valueSupplied)
    setDate(new Date(item.date))
  }

  function save() {
    vehicleService.saveFuelExpense({
      id,
      date,
      miliage: Number(miliage),
      pricePerLiter,
      valueSupplied,
      vehicleId: vehicle.id
    }).then(() => {
      toast.success('Abastecimento salvo!')
      clear()
      refresh()
    }).catch(err => console.log(err))
  }

  function remove() {
    vehicleService.removeFuelExpense(removeItem.id)
      .then(() => {
        setFuelExpenses(fuelExpenses.filter(p => p.id !== removeItem.id))
        setRemoveItem(null)
        clear()
      })
      .catch(err => console.log(err))
  }

  const valid = Number(miliage) > 0 && pricePerLiter > 0 && valueSupplied > 0 && !!date
  const liters = pricePerLiter > 0 ? valueSupplied / pricePerLiter : 0
  const visible = fuelExpenses.slice(page * rowsPerPage, (page + 1) * rowsPerPage)

  return (
    <Dialog open={!!vehicle} onClose={onClose} fullWidth maxWidth="md" fullScreen={fullScreen}>
      <DialogTitle sx={{ fontWeight: 800 }}>
        Abastecimentos<HelpTip id="abastecimento" />
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>{(vehicle || {}).description}</Typography>
      </DialogTitle>
      <DialogContent>
        {loading ?
          <Box sx={{ textAlign: 'center', py: 6 }}><CircularProgress /></Box>
          : fuelExpenses.length ?
            <Box sx={{ overflowX: 'auto' }}>
              <Table size="small" sx={{ minWidth: 600 }}>
                <TableHead>
                  <TableRow>
                    <TableCell>Data</TableCell>
                    <TableCell align="right">Km no painel</TableCell>
                    <TableCell align="right">Preço/litro</TableCell>
                    <TableCell align="right">Valor</TableCell>
                    <TableCell align="right">Litros</TableCell>
                    <TableCell align="right">Ações</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {visible.map(p =>
                    <TableRow key={p.id} hover selected={p.id === id}>
                      <TableCell>{dateToString(p.date)}</TableCell>
                      <TableCell align="right">{toThousandFormat(p.miliage)} km</TableCell>
                      <TableCell align="right"><Money value={p.pricePerLiter} /></TableCell>
                      <TableCell align="right"><Money value={p.valueSupplied} tone="out" /></TableCell>
                      <TableCell align="right">{toDecimalText(p.litersSupplied)}</TableCell>
                      <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                        <ActionIcon title="Editar" size="small" color="primary" onClick={() => edit(p)}><EditRoundedIcon fontSize="small" /></ActionIcon>
                        <ActionIcon title="Remover" size="small" color="error" onClick={() => setRemoveItem(p)}><DeleteOutlineRoundedIcon fontSize="small" /></ActionIcon>
                      </TableCell>
                    </TableRow>
                  )}
                </TableBody>
              </Table>
              <TablePagination component="div" rowsPerPageOptions={[5, 10]} count={fuelExpenses.length}
                rowsPerPage={rowsPerPage} page={page}
                labelRowsPerPage="Por página"
                labelDisplayedRows={({ from, to, count }) => `${from}–${to} de ${count}`}
                onPageChange={(e, p) => setPage(p)}
                onRowsPerPageChange={e => { setRowsPerPage(Number(e.target.value)); setPage(0) }} />
            </Box>
            :
            <Typography sx={{ color: 'text.secondary', py: 2 }}>Nenhum abastecimento registrado ainda.</Typography>
        }

        <Card sx={{ p: 2, mt: 2, bgcolor: 'action.hover' }}>
          <Typography sx={{ fontWeight: 800, mb: 1.5 }}>{id ? 'Editando abastecimento' : 'Registrar abastecimento'}</Typography>
          <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr' }, gap: 2 }}>
            <TextField label="Quilometragem no painel" value={miliage}
              onChange={e => setMiliage(String(Math.min(MAX_VALUE, Number(e.target.value.replace(/\D/g, '') || 0)) || ''))}
              slotProps={{ htmlInput: { inputMode: 'numeric' } }}
              helperText="O número do hodômetro na hora de abastecer." />
            <DateField label="Data" value={date} onChange={setDate} />
            <MoneyField label="Preço por litro" value={pricePerLiter} onChange={setPricePerLiter} max={MAX_VALUE} />
            <MoneyField label="Valor total abastecido" value={valueSupplied} onChange={setValueSupplied} max={MAX_VALUE}
              helperText={liters > 0 ? `Aproximadamente ${toDecimalText(liters.toFixed(2))} litros` : ' '} />
          </Box>
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 1, mt: 2 }}>
            <Button onClick={clear}>Limpar</Button>
            <Button variant="contained" disabled={!valid} onClick={save}>Salvar</Button>
          </Box>
        </Card>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 2.5 }}>
        <Button variant="outlined" onClick={onClose}>Fechar</Button>
      </DialogActions>
      <ConfirmDialog open={!!removeItem} danger
        title="Remover abastecimento?"
        text={removeItem ? `Abastecimento de ${dateToString(removeItem.date)} será apagado.` : ''}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
    </Dialog>
  )
}
