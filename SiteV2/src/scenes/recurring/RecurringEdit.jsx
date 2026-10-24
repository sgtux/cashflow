import React, { useState, useEffect } from 'react'
import { useParams, useNavigate, Link } from 'react-router-dom'
import { Box, Button, Card, MenuItem, TextField } from '@mui/material'
import ArrowBackRoundedIcon from '@mui/icons-material/ArrowBackRounded'
import HistoryRoundedIcon from '@mui/icons-material/HistoryRounded'

import { Page, MoneyField, DateField, SelectField } from '../../components'
import { creditCardService } from '../../services'
import { toast } from '../../helpers'
import { HistoryDialog } from './HistoryDialog'

export function RecurringEdit({ config, icon }) {
  const { service, texts, TypeAvatar } = config

  const params = useParams()
  const navigate = useNavigate()
  const id = Number(params.id) || 0

  const [description, setDescription] = useState('')
  const [value, setValue] = useState(0)
  const [card, setCard] = useState(0)
  const [cards, setCards] = useState([])
  const [type, setType] = useState('')
  const [types, setTypes] = useState([])
  const [inactiveAt, setInactiveAt] = useState(null)
  const [item, setItem] = useState(null)
  const [showHistory, setShowHistory] = useState(false)
  const [loading, setLoading] = useState(false)

  useEffect(() => {
    if (config.withCreditCard)
      creditCardService.get().then(res => setCards(res || [])).catch(err => console.log(err))
    config.getTypes().then(res => setTypes(res || [])).catch(err => console.log(err))
    refresh()
  }, [id])

  function refresh() {
    if (!id)
      return
    setLoading(true)
    service.get(id)
      .then(res => {
        if (res) {
          setDescription(res.description || '')
          setValue(res.value || 0)
          setCard((res.creditCard || {}).id || 0)
          setType(res.type || '')
          setInactiveAt(res.inactiveAt ? new Date(res.inactiveAt) : null)
          setItem(res)
        }
      })
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  const valid = !!description.trim() && value > 0 && Number(type) > 0

  function save() {
    setLoading(true)
    const body = { id, description, value, inactiveAt, type: Number(type) }
    if (config.withCreditCard)
      body.creditCardId = card || null
    service.save(body)
      .then(() => {
        toast.success('Salvo!')
        if (!id)
          navigate(config.listPath)
      })
      .catch(err => console.log(err))
      .finally(() => setLoading(false))
  }

  return (
    <Page icon={icon} title={id ? texts.editTitle : texts.newTitle} subtitle={texts.subtitle} loading={loading} maxWidth={760}>
      <Card sx={{ p: { xs: 2, sm: 3 } }}>
        <Box sx={{ display: 'grid', gap: 2.5 }}>
          <TextField label={texts.descriptionLabel} placeholder={texts.descriptionPlaceholder}
            value={description} onChange={e => setDescription(e.target.value)} autoFocus={!id} />
          <MoneyField label={texts.valueLabel} value={value} onChange={setValue} helperText={texts.valueHelper} />
          <TextField select label={texts.typeLabel} value={type || ''} onChange={e => setType(e.target.value)}
            helperText={texts.typeHelper}>
            {types.map(p =>
              <MenuItem key={p.id} value={p.id}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                  <TypeAvatar type={p.id} size={26} /> {p.description}
                </Box>
              </MenuItem>
            )}
          </TextField>
          {config.withCreditCard && !!cards.length &&
            <SelectField label="É cobrada no cartão de crédito?" help="cartao" value={card || 0} onChange={setCard}
              emptyLabel="Não, pago por fora (boleto, Pix, débito)"
              options={cards.map(p => ({ value: p.id, label: p.name }))}
              helperText="Se for no cartão, o valor entra na fatura dele." />
          }
          {!!id &&
            <DateField label={texts.inactiveAtLabel} value={inactiveAt} onChange={setInactiveAt} helperText={texts.inactiveAtHelper} clearable />
          }
        </Box>
        <Box sx={{ display: 'flex', gap: 1, justifyContent: 'flex-end', flexWrap: 'wrap', mt: 3 }}>
          <Button component={Link} to={config.listPath} startIcon={<ArrowBackRoundedIcon />}>{texts.backToList}</Button>
          {!!id && <Button variant="outlined" startIcon={<HistoryRoundedIcon />} onClick={() => setShowHistory(true)}>{texts.historyTitle}</Button>}
          <Button variant="contained" disabled={!valid || loading} onClick={save}>Salvar</Button>
        </Box>
      </Card>
      <HistoryDialog config={config} item={item} open={showHistory}
        requestRefresh={refresh}
        onClose={() => setShowHistory(false)} />
    </Page>
  )
}
