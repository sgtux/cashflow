import React, { useState, useEffect } from 'react'
import { Box, Button, Card, Collapse, FormControlLabel, Grid2 as Grid, Switch, Typography } from '@mui/material'
import DirectionsCarRoundedIcon from '@mui/icons-material/DirectionsCarRounded'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import LocalGasStationRoundedIcon from '@mui/icons-material/LocalGasStationRounded'
import ExpandMoreRoundedIcon from '@mui/icons-material/ExpandMoreRounded'

import { Page, Money, EmptyState, ConfirmDialog, ActionIcon, StatusChip, HelpTip } from '../../components'
import { vehicleService } from '../../services'
import { dateToString, toThousandFormat, toDecimalText, toast } from '../../helpers'
import { VehicleDialog } from './VehicleDialog'
import { FuelExpensesDialog } from './FuelExpensesDialog'

function Metric({ label, help, children }) {
  return (
    <Box sx={{ p: 1.5, borderRadius: 2, bgcolor: 'action.hover' }}>
      <Typography variant="caption" sx={{ color: 'text.secondary', fontWeight: 700 }}>{label}{help && <HelpTip id={help} />}</Typography>
      <Typography sx={{ fontWeight: 800, fontSize: 20 }}>{children}</Typography>
    </Box>
  )
}

export function Vehicles() {
  const [vehicles, setVehicles] = useState([])
  const [loading, setLoading] = useState(false)
  const [showInactives, setShowInactives] = useState(false)
  const [editVehicle, setEditVehicle] = useState(null)
  const [fuelVehicle, setFuelVehicle] = useState(null)
  const [removeItem, setRemoveItem] = useState(null)
  const [open, setOpen] = useState({})

  useEffect(() => { refresh() }, [showInactives])

  function refresh() {
    setLoading(true)
    vehicleService.getAll(showInactives)
      .then(res => setVehicles(res || []))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  function remove() {
    setLoading(true)
    vehicleService.remove(removeItem.id)
      .then(() => {
        toast.success('Veículo removido.')
        setRemoveItem(null)
        refresh()
      })
      .catch(() => setLoading(false))
  }

  function saveVehicle(vehicle) {
    setLoading(true)
    setEditVehicle(null)
    vehicleService.save(vehicle)
      .then(() => {
        toast.success('Veículo salvo!')
        refresh()
      })
      .catch(() => setLoading(false))
  }

  return (
    <Page
      icon={<DirectionsCarRoundedIcon />}
      title="Veículos"
      subtitle="Acompanhe quanto gasta com combustível e o consumo de cada veículo."
      guide="vehicles"
      loading={loading}
      actions={<Button variant="contained" startIcon={<AddRoundedIcon />} onClick={() => setEditVehicle({})}>Adicionar veículo</Button>}>

      {vehicles.length ?
        <Grid container spacing={2}>
          {vehicles.map(p =>
            <Grid key={p.id} size={{ xs: 12, md: 6 }}>
              <Card sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                  <DirectionsCarRoundedIcon sx={{ color: 'primary.main', fontSize: 32 }} />
                  <Box sx={{ flex: 1, minWidth: 0 }}>
                    <Typography sx={{ fontWeight: 800 }}>{p.description}</Typography>
                    {!p.active && <StatusChip status="inactive" label="Não uso mais" />}
                  </Box>
                  <ActionIcon title="Editar" color="primary" onClick={() => setEditVehicle(p)}><EditRoundedIcon /></ActionIcon>
                  <ActionIcon title="Remover" color="error" onClick={() => setRemoveItem(p)}><DeleteOutlineRoundedIcon /></ActionIcon>
                </Box>
                <Box sx={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: 1.5, mt: 2 }}>
                  <Metric label="Já percorrido">{toThousandFormat(p.miliageTraveled) || 0} km</Metric>
                  <Metric label="Média de consumo" help="kmPorLitro">{toDecimalText(p.miliagePerLiter) || 0} km/l</Metric>
                </Box>
                <Box sx={{ display: 'flex', gap: 1, mt: 2, flexWrap: 'wrap' }}>
                  <Button variant="contained" startIcon={<LocalGasStationRoundedIcon />} onClick={() => setFuelVehicle(p)}>Abastecimentos</Button>
                  <Button onClick={() => setOpen({ ...open, [p.id]: !open[p.id] })}
                    endIcon={<ExpandMoreRoundedIcon sx={{ transition: '200ms', transform: open[p.id] ? 'rotate(180deg)' : 'none' }} />}>
                    Últimos 10
                  </Button>
                </Box>
                <Collapse in={!!open[p.id]} unmountOnExit>
                  <Box sx={{ display: 'grid', gap: 0.75, mt: 1.5 }}>
                    {(p.fuelExpensesLast10 || []).length === 0 &&
                      <Typography variant="body2" sx={{ color: 'text.secondary' }}>Nenhum abastecimento ainda.</Typography>}
                    {(p.fuelExpensesLast10 || []).map(f =>
                      <Box key={f.id} sx={{ display: 'flex', justifyContent: 'space-between', gap: 1, px: 1.5, py: 0.75, borderRadius: 1.5, bgcolor: 'action.hover', flexWrap: 'wrap' }}>
                        <Typography variant="body2">{dateToString(f.date)} · {toThousandFormat(f.miliage)} km</Typography>
                        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
                          {toDecimalText(f.litersSupplied)} l a <Money value={f.pricePerLiter} size="xs" />
                        </Typography>
                        <Money value={f.valueSupplied} tone="out" size="sm" bold />
                      </Box>
                    )}
                  </Box>
                </Collapse>
              </Card>
            </Grid>
          )}
        </Grid>
        : !loading &&
        <EmptyState
          icon={<DirectionsCarRoundedIcon />}
          title="Nenhum veículo cadastrado"
          text="Tem carro ou moto? Registrar os abastecimentos mostra quanto o veículo realmente pesa no seu orçamento."
          actionLabel="Adicionar veículo"
          onAction={() => setEditVehicle({})} />
      }

      <Box sx={{ textAlign: 'center', mt: 2 }}>
        <FormControlLabel control={<Switch checked={showInactives} onChange={e => setShowInactives(e.target.checked)} />}
          label="Mostrar veículos que não uso mais" />
      </Box>

      <ConfirmDialog open={!!removeItem} danger
        title="Remover veículo?"
        text={`"${(removeItem || {}).description || ''}" será removido. Se só vendeu, prefira marcar como "não uso mais".`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
      <VehicleDialog vehicle={editVehicle} onCancel={() => setEditVehicle(null)} onSave={saveVehicle} />
      <FuelExpensesDialog vehicle={fuelVehicle} onClose={() => { setFuelVehicle(null); refresh() }} />
    </Page>
  )
}
