import React, { useState, useEffect } from 'react'
import {
  Box, Button, Card, Dialog, DialogActions, DialogContent, DialogTitle,
  Table, TableBody, TableCell, TableHead, TablePagination, TableRow, Typography, useMediaQuery
} from '@mui/material'
import { useTheme } from '@mui/material/styles'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'

import { Money, MoneyField, DateField, ConfirmDialog, ActionIcon } from '../../components'
import { dateToString, toast } from '../../helpers'

export function HistoryDialog({ config, item, open, onClose, requestRefresh }) {
  const { service, valueKey, parentKey, texts, tone } = config

  const [id, setId] = useState(0)
  const [date, setDate] = useState(null)
  const [value, setValue] = useState(0)
  const [removeItem, setRemoveItem] = useState(null)
  const [rowsPerPage, setRowsPerPage] = useState(5)
  const [page, setPage] = useState(0)

  const theme = useTheme()
  const fullScreen = useMediaQuery(theme.breakpoints.down('sm'))

  const history = (item || {}).history || []
  const visible = history.slice(page * rowsPerPage, (page + 1) * rowsPerPage)

  useEffect(() => { clear() }, [item && item.id])

  function clear() {
    setId(0)
    setDate(null)
    setValue(0)
  }

  function edit(h) {
    setId(h.id)
    setValue(h[valueKey])
    setDate(new Date(h.date))
  }

  function save() {
    service.saveHistory({
      id,
      [valueKey]: value,
      date,
      [parentKey]: item.id
    }).then(() => {
      requestRefresh()
      toast.success('Histórico salvo!')
      clear()
    }).catch(err => console.log(err))
  }

  function remove() {
    service.removeHistory(removeItem[parentKey], removeItem.id)
      .then(() => {
        requestRefresh()
        toast.success('Registro removido.')
        setRemoveItem(null)
      })
      .catch(err => console.log(err))
  }

  return (
    <Dialog open={!!open} onClose={onClose} fullWidth maxWidth="md" fullScreen={fullScreen}>
      <DialogTitle sx={{ fontWeight: 800 }}>
        {texts.historyTitle}
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>{(item || {}).description}</Typography>
      </DialogTitle>
      <DialogContent>
        {history.length ?
          <Box sx={{ overflowX: 'auto' }}>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Data</TableCell>
                  <TableCell align="right">{texts.historyValueLabel}</TableCell>
                  <TableCell align="right">Ações</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {visible.map(h =>
                  <TableRow key={h.id} hover selected={h.id === id}>
                    <TableCell>{dateToString(h.date)}</TableCell>
                    <TableCell align="right"><Money value={h[valueKey]} tone={tone} /></TableCell>
                    <TableCell align="right">
                      <ActionIcon title="Editar" size="small" color="primary" onClick={() => edit(h)}><EditRoundedIcon fontSize="small" /></ActionIcon>
                      <ActionIcon title="Remover" size="small" color="error" onClick={() => setRemoveItem(h)}><DeleteOutlineRoundedIcon fontSize="small" /></ActionIcon>
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
            <TablePagination
              component="div"
              rowsPerPageOptions={[5, 10]}
              count={history.length}
              rowsPerPage={rowsPerPage}
              page={page}
              labelRowsPerPage="Por página"
              labelDisplayedRows={({ from, to, count }) => `${from}–${to} de ${count}`}
              onPageChange={(e, p) => setPage(p)}
              onRowsPerPageChange={e => { setRowsPerPage(Number(e.target.value)); setPage(0) }} />
          </Box>
          :
          <Typography sx={{ color: 'text.secondary', py: 2 }}>{texts.historyEmpty}</Typography>
        }

        <Card sx={{ p: 2, mt: 2, bgcolor: 'action.hover' }}>
          <Typography sx={{ fontWeight: 800, mb: 1.5 }}>{id ? 'Editando registro' : 'Adicionar registro'}</Typography>
          <Box sx={{ display: 'grid', gridTemplateColumns: { xs: '1fr', sm: '1fr 1fr auto' }, gap: 2, alignItems: 'start' }}>
            <MoneyField label={texts.historyValueLabel} value={value} onChange={setValue} />
            <DateField label="Data" value={date} onChange={setDate} />
            <Box sx={{ display: 'flex', gap: 1, pt: 1 }}>
              <Button onClick={clear}>Limpar</Button>
              <Button variant="contained" disabled={!date || !(value > 0)} onClick={save}>Salvar</Button>
            </Box>
          </Box>
        </Card>
      </DialogContent>
      <DialogActions sx={{ px: 3, pb: 2.5 }}>
        <Button variant="outlined" onClick={onClose}>Fechar</Button>
      </DialogActions>
      <ConfirmDialog open={!!removeItem} danger
        title="Remover este registro?"
        text={removeItem ? `Registro de ${dateToString(removeItem.date)} será apagado do histórico.` : ''}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
    </Dialog>
  )
}
