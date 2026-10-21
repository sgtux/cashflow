import React, { useState, useEffect } from 'react'
import { Box, Button, Card, FormControlLabel, Switch, Typography } from '@mui/material'
import SavingsRoundedIcon from '@mui/icons-material/SavingsRounded'
import EditRoundedIcon from '@mui/icons-material/EditRounded'
import RefreshRoundedIcon from '@mui/icons-material/RefreshRounded'

import { Page, Money, MoneyField, EmptyState, ConfirmDialog, ActionIcon, FormDialog, HelpTip, Term } from '../../components'
import { remainingBalanceService } from '../../services'
import { toast } from '../../helpers'

export function RemainingBalances() {
  const [items, setItems] = useState([])
  const [loading, setLoading] = useState(false)
  const [loaded, setLoaded] = useState(false)
  const [showAll, setShowAll] = useState(false)
  const [selected, setSelected] = useState(null)
  const [value, setValue] = useState(0)
  const [debt, setDebt] = useState(false)
  const [recalculateItem, setRecalculateItem] = useState(null)

  useEffect(() => { refresh() }, [])

  function refresh() {
    setSelected(null)
    setRecalculateItem(null)
    setLoading(true)
    remainingBalanceService.getAll()
      .then(res => setItems(res || []))
      .catch(err => console.log(err))
      .finally(() => {
        setLoading(false)
        setLoaded(true)
      })
  }

  function edit(p) {
    setSelected(p)
    setValue(Math.abs(p.value || 0))
    setDebt(p.value < 0)
  }

  function update() {
    setLoading(true)
    remainingBalanceService.update({
      month: selected.month,
      year: selected.year,
      value: debt ? -value : value
    })
      .then(() => {
        toast.success('Valor ajustado!')
        refresh()
      })
      .catch(() => setLoading(false))
  }

  function recalculate() {
    setLoading(true)
    remainingBalanceService.recalculate(recalculateItem)
      .then(() => {
        toast.success('Mês recalculado!')
        refresh()
      })
      .catch(() => setLoading(false))
  }

  const visible = showAll ? items : items.slice(0, 5)

  return (
    <Page
      icon={<SavingsRoundedIcon />}
      title="Fechamento do mês"
      subtitle="Quanto realmente sobrou (ou faltou) no fim de cada mês."
      guide="remainingBalance"
      loading={loading}
      maxWidth={860}>

      {visible.length ?
        <>
          <Box sx={{ display: 'grid', gap: 1.25 }}>
            {visible.map((p, i) => {
              const simulated = !p.id
              return (
                <Card key={p.id || `sim-${i}`} sx={{ p: 2, display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap', borderStyle: simulated ? 'dashed' : 'solid' }}>
                  <Box sx={{ width: 6, alignSelf: 'stretch', borderRadius: 99, bgcolor: p.value >= 0 ? 'success.main' : 'error.main' }} />
                  <Box sx={{ flex: '1 1 180px' }}>
                    <Typography sx={{ fontWeight: 800 }}>{p.monthYearText}</Typography>
                    <Typography variant="caption" sx={{ color: 'text.secondary' }}>
                      {simulated
                        ? <>Simulação do mês atual<HelpTip text="O mês ainda não acabou: este valor muda conforme você lança ganhos e gastos." /></>
                        : p.value >= 0 ? 'Sobrou dinheiro neste mês' : <><Term id="devedor">Ficou devendo</Term> neste mês</>}
                    </Typography>
                  </Box>
                  <Money value={p.value} tone="auto" bold size="lg" />
                  {!simulated &&
                    <Box>
                      <ActionIcon title="Ajustar valor manualmente" color="primary" onClick={() => edit(p)}><EditRoundedIcon /></ActionIcon>
                      <ActionIcon title="Recalcular pelo que está lançado" color="primary" onClick={() => setRecalculateItem(p)}><RefreshRoundedIcon /></ActionIcon>
                    </Box>
                  }
                </Card>
              )
            })}
          </Box>
          {items.length > 5 &&
            <Box sx={{ textAlign: 'center', mt: 2 }}>
              <Button onClick={() => setShowAll(!showAll)}>{showAll ? 'Mostrar só os últimos 5 meses' : `Mostrar todos os ${items.length} meses`}</Button>
            </Box>
          }
        </>
        : loaded &&
        <EmptyState icon={<SavingsRoundedIcon />} title="Nenhum mês fechado ainda"
          text="Quando um mês termina, calculamos automaticamente quanto sobrou. Volte aqui no começo do próximo mês!" />
      }

      <FormDialog open={!!selected} maxWidth="xs"
        title={`Ajustar ${(selected || {}).monthYearText || ''}`}
        subtitle="Use quando o valor não bater com o saldo real da sua conta no fim do mês."
        onClose={() => setSelected(null)}
        onSave={update}>
        <MoneyField label="Quanto sobrou (ou faltou)?" value={value} onChange={setValue} />
        <FormControlLabel control={<Switch checked={debt} onChange={e => setDebt(e.target.checked)} color="error" />}
          label={<>Fechei o mês devendo<HelpTip id="devedor" /></>} />
        <Typography variant="body2" sx={{ color: 'text.secondary' }}>
          Valor final: <Money value={debt ? -value : value} tone="auto" bold />
        </Typography>
      </FormDialog>
      <ConfirmDialog open={!!recalculateItem}
        title="Recalcular este mês?"
        text={`O valor de ${(recalculateItem || {}).monthYearText || ''} será refeito a partir dos ganhos e gastos lançados, substituindo qualquer ajuste manual.`}
        confirmLabel="Recalcular"
        onClose={() => setRecalculateItem(null)}
        onConfirm={recalculate} />
    </Page>
  )
}
