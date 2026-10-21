import React, { useState, useEffect } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { Box, Button, Card, Collapse, FormControlLabel, Grid2 as Grid, Switch, Typography } from '@mui/material'
import AddRoundedIcon from '@mui/icons-material/AddRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import DeleteOutlineRoundedIcon from '@mui/icons-material/DeleteOutlineRounded'
import HistoryRoundedIcon from '@mui/icons-material/HistoryRounded'
import ExpandMoreRoundedIcon from '@mui/icons-material/ExpandMoreRounded'
import CheckRoundedIcon from '@mui/icons-material/CheckRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'

import { Page, Money, EmptyState, ConfirmDialog, ActionIcon, StatusChip, StatCard, HelpTip } from '../../components'
import { dateToString, toast, sum } from '../../helpers'
import { HistoryDialog } from './HistoryDialog'

export function RecurringList({ config, icon }) {
  const { service, texts, doneKey, valueKey, tone } = config

  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(false)
  const [showInactives, setShowInactives] = useState(false)
  const [historyItem, setHistoryItem] = useState(null)
  const [removeItem, setRemoveItem] = useState(null)
  const [open, setOpen] = useState({})

  const navigate = useNavigate()

  useEffect(() => { refresh() }, [showInactives])

  function refresh() {
    setLoading(true)
    service.getAll(showInactives)
      .then(res => setItems(res || []))
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  function quickDone(item) {
    setLoading(true)
    service.saveHistory({
      [valueKey]: item.value,
      date: new Date(),
      [config.parentKey]: item.id
    })
      .then(() => {
        toast.success(`${item.description}: registrado!`)
        refresh()
      })
      .catch(() => setLoading(false))
  }

  function refreshHistory() {
    service.get(historyItem.id)
      .then(res => setHistoryItem(res))
      .catch(err => console.log(err))
  }

  function remove() {
    setLoading(true)
    service.remove(removeItem.id)
      .then(() => {
        toast.success('Removido.')
        setRemoveItem(null)
        refresh()
      })
      .catch(() => {
        setRemoveItem(null)
        setLoading(false)
      })
  }

  const active = items.filter(p => !p.inactiveAt)
  const pending = active.filter(p => !p[doneKey])

  const status = p => p.inactiveAt
    ? <StatusChip status="inactive" label={texts.inactive} />
    : p[doneKey] ? <StatusChip status="done" label={texts.done} /> : <StatusChip status="pending" label={texts.pending} />

  return (
    <Page
      icon={icon}
      title={texts.title}
      subtitle={texts.subtitle}
      guide={config.guide}
      loading={loading}
      actions={<Button variant="contained" startIcon={<AddRoundedIcon />} component={Link} to={`${config.editPath}/0`}>{texts.add}</Button>}>

      {!!active.length &&
        <Grid container spacing={2} sx={{ mb: 2.5 }}>
          <Grid size={{ xs: 12, sm: 6 }}>
            <StatCard label={texts.totalLabel} help={config.help} tone={tone} value={sum(active, p => p.value)} />
          </Grid>
          <Grid size={{ xs: 12, sm: 6 }}>
            <Card sx={{ p: 2.5, height: '100%', borderTop: 4, borderTopColor: pending.length ? 'warning.main' : 'success.main' }}>
              <Typography variant="body2" sx={{ fontWeight: 700, color: 'text.secondary' }}>Situação deste mês</Typography>
              <Typography sx={{ fontWeight: 800, fontSize: 20, mt: 1 }}>
                {pending.length ? texts.pendingCount(pending.length) : 'Tudo certo por aqui!'}
              </Typography>
              {!!pending.length && <Money value={sum(pending, p => p.value)} tone={tone} bold size="md" />}
            </Card>
          </Grid>
        </Grid>
      }

      {items.length ?
        <Box sx={{ display: 'grid', gap: 1.25 }}>
          {items.map(p =>
            <Card key={p.id} sx={{ opacity: p.inactiveAt ? 0.7 : 1 }}>
              <Box sx={{ p: 1.5, pl: 2, display: 'flex', alignItems: 'center', gap: 1.5, flexWrap: 'wrap' }}>
                <ActionIcon title={open[p.id] ? 'Esconder últimos registros' : 'Ver últimos registros'} size="small"
                  onClick={() => setOpen({ ...open, [p.id]: !open[p.id] })}>
                  <ExpandMoreRoundedIcon sx={{ transition: '200ms', transform: open[p.id] ? 'rotate(180deg)' : 'none' }} />
                </ActionIcon>
                <Box sx={{ flex: '1 1 200px', minWidth: 0 }}>
                  <Typography sx={{ fontWeight: 700 }}>{p.description}</Typography>
                  <Box sx={{ display: 'flex', gap: 1, alignItems: 'center', flexWrap: 'wrap', mt: 0.5 }}>
                    {status(p)}
                    {p.creditCard && p.creditCard.name &&
                      <Typography variant="caption" sx={{ color: 'text.secondary', display: 'inline-flex', alignItems: 'center', gap: 0.5 }}>
                        <CreditCardRoundedIcon sx={{ fontSize: 14 }} /> {p.creditCard.name}
                      </Typography>}
                  </Box>
                </Box>
                <Box sx={{ textAlign: 'right' }}>
                  <Typography variant="caption" sx={{ color: 'text.secondary' }}>por mês</Typography>
                  <Box><Money value={p.value} tone={tone} bold size="md" /></Box>
                </Box>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 0.5 }}>
                  {!p.inactiveAt && !p[doneKey] &&
                    <Button size="small" variant="contained" color={tone === 'in' ? 'success' : 'primary'} startIcon={<CheckRoundedIcon />}
                      title={texts.quickActionHint} onClick={() => quickDone(p)}>
                      {texts.quickAction}
                    </Button>
                  }
                  <ActionIcon title={texts.historyTitle} color="primary" onClick={() => setHistoryItem(p)}><HistoryRoundedIcon /></ActionIcon>
                  <ActionIcon title="Editar" color="primary" component={Link} to={`${config.editPath}/${p.id}`}><EditRoundedIcon /></ActionIcon>
                  <ActionIcon title="Remover" color="error" onClick={() => setRemoveItem(p)}><DeleteOutlineRoundedIcon /></ActionIcon>
                </Box>
              </Box>
              <Collapse in={!!open[p.id]} unmountOnExit>
                <Box sx={{ px: 2, pb: 2 }}>
                  <Typography variant="body2" sx={{ fontWeight: 700, color: 'text.secondary', mb: 1 }}>Últimos registros</Typography>
                  {(p.history || []).length ?
                    <Box sx={{ display: 'grid', gap: 0.5 }}>
                      {p.history.slice(0, 10).map(h =>
                        <Box key={h.id} sx={{ display: 'flex', justifyContent: 'space-between', px: 1.5, py: 0.75, borderRadius: 1.5, bgcolor: 'action.hover' }}>
                          <Typography variant="body2">{dateToString(h.date)}</Typography>
                          <Money value={h[valueKey]} tone={tone} />
                        </Box>
                      )}
                    </Box>
                    :
                    <Typography variant="body2" sx={{ color: 'text.secondary' }}>{texts.historyEmpty}</Typography>
                  }
                </Box>
              </Collapse>
            </Card>
          )}
        </Box>
        : !loading &&
        <EmptyState icon={icon} title={texts.emptyTitle} text={texts.emptyText}
          actionLabel={texts.add} onAction={() => navigate(`${config.editPath}/0`)} />
      }

      <Box sx={{ textAlign: 'center', mt: 2 }}>
        <FormControlLabel
          control={<Switch checked={showInactives} onChange={e => setShowInactives(e.target.checked)} />}
          label={<>{texts.showInactive}<HelpTip text="Itens encerrados não entram mais nas contas do mês nem na projeção, mas o histórico fica guardado." /></>} />
      </Box>

      <ConfirmDialog open={!!removeItem} danger
        title={texts.removeTitle}
        text={`"${(removeItem || {}).description || ''}" e todo o histórico serão apagados. Se só parou de existir, prefira encerrar (editar → data de encerramento).`}
        confirmLabel="Remover"
        onClose={() => setRemoveItem(null)}
        onConfirm={remove} />
      <HistoryDialog config={config} item={historyItem} open={!!historyItem}
        requestRefresh={refreshHistory}
        onClose={() => { setHistoryItem(null); refresh() }} />
    </Page>
  )
}
