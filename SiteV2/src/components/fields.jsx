import React from 'react'
import { Box, IconButton, MenuItem, TextField, Typography } from '@mui/material'
import ChevronLeftRoundedIcon from '@mui/icons-material/ChevronLeftRounded'
import ChevronRightRoundedIcon from '@mui/icons-material/ChevronRightRounded'
import { CurrencyInput } from 'react-currency-mask'
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers'
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import dayjs from 'dayjs'
import 'dayjs/locale/pt-br'

import { Months } from '../helpers'
import { HelpTip } from './Term'

function FieldLabel({ label, help }) {
  if (!help)
    return label
  return <Box component="span" sx={{ display: 'inline-flex', alignItems: 'center', pointerEvents: 'auto' }}>{label}<HelpTip id={help} /></Box>
}

// Valor em reais. value/onChange trabalham com número (ex.: 1234.56).
export function MoneyField({ label, value, onChange, helperText, help, autoFocus, max = 999999999 }) {
  return (
    <CurrencyInput
      value={value || ''}
      max={max}
      onChangeValue={(event, originalValue) => onChange(Number(originalValue) || 0)}
      InputElement={
        <TextField
          label={<FieldLabel label={label} help={help} />}
          helperText={helperText}
          autoFocus={autoFocus}
          placeholder="R$ 0,00"
          inputProps={{ inputMode: 'numeric' }}
          slotProps={{ inputLabel: { shrink: true } }}
        />
      }
    />
  )
}

const toDayjs = date => {
  if (!date)
    return null
  const d = dayjs(date)
  return d.isValid() ? d : null
}

// value/onChange trabalham com Date local (meia-noite do dia escolhido). O DatePicker
// fixa o formato dd/MM/aaaa, independente do idioma do navegador.
export function DateField({ label, value, onChange, helperText, help, clearable }) {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs} adapterLocale="pt-br">
      <DatePicker
        label={<FieldLabel label={label} help={help} />}
        value={toDayjs(value)}
        onChange={d => onChange(d && d.isValid() ? d.startOf('day').toDate() : null)}
        slotProps={{
          field: { clearable },
          textField: { helperText, fullWidth: true }
        }}
      />
    </LocalizationProvider>
  )
}

// options: [{ value, label }]. emptyLabel adiciona a opção "nenhum".
export function SelectField({ label, value, onChange, options, emptyLabel, helperText, help }) {
  return (
    <TextField
      select
      label={<FieldLabel label={label} help={help} />}
      value={value ?? ''}
      onChange={e => onChange(e.target.value)}
      helperText={helperText}
    >
      {emptyLabel && <MenuItem value={0}><em>{emptyLabel}</em></MenuItem>}
      {options.map(p => <MenuItem key={p.value} value={p.value}>{p.label}</MenuItem>)}
    </TextField>
  )
}

// Navegação "‹ Outubro de 2026 ›" para escolher o mês de referência.
export function MonthStepper({ month, year, onChange, minYear, maxYear }) {
  function move(delta) {
    let m = month + delta
    let y = year
    if (m < 1) { m = 12; y-- }
    if (m > 12) { m = 1; y++ }
    if ((minYear && y < minYear) || (maxYear && y > maxYear))
      return
    onChange(m, y)
  }

  const now = new Date()
  const isCurrent = month === now.getMonth() + 1 && year === now.getFullYear()

  return (
    <Box sx={{ display: 'inline-flex', alignItems: 'center', gap: 0.5, bgcolor: 'background.paper', border: 1, borderColor: 'divider', borderRadius: 99, px: 0.5 }}>
      <IconButton onClick={() => move(-1)} aria-label="Mês anterior"><ChevronLeftRoundedIcon /></IconButton>
      <Box sx={{ minWidth: 170, textAlign: 'center' }}>
        <Typography sx={{ fontWeight: 800, lineHeight: 1.1 }}>{Months[month - 1]} de {year}</Typography>
        <Typography variant="caption" sx={{ color: isCurrent ? 'primary.main' : 'text.secondary', fontWeight: 700 }}>
          {isCurrent ? 'mês atual' : <Box component="span" role="button" tabIndex={0} sx={{ cursor: 'pointer', textDecoration: 'underline' }}
            onClick={() => onChange(now.getMonth() + 1, now.getFullYear())}>voltar para o mês atual</Box>}
        </Typography>
      </Box>
      <IconButton onClick={() => move(1)} aria-label="Próximo mês"><ChevronRightRoundedIcon /></IconButton>
    </Box>
  )
}
