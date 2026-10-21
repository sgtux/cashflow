import React, { useState } from 'react'
import { Box, Card, InputAdornment, TextField, Typography } from '@mui/material'
import MenuBookRoundedIcon from '@mui/icons-material/MenuBookRounded'
import SearchRoundedIcon from '@mui/icons-material/SearchRounded'

import { Page } from '../../components'
import { Glossary as Terms, GlossaryGroups } from '../../content/glossary'

const normalize = text => (text || '').normalize('NFD').replace(/[̀-ͯ]/g, '').toLowerCase()

export function Glossary() {
  const [search, setSearch] = useState('')
  const query = normalize(search)

  const groups = GlossaryGroups
    .map(g => ({ ...g, ids: g.ids.filter(id => !query || normalize(`${Terms[id].term} ${Terms[id].short}`).includes(query)) }))
    .filter(g => g.ids.length)

  return (
    <Page icon={<MenuBookRoundedIcon />} title="Glossário" subtitle="Os termos de finanças que aparecem no app, explicados sem complicação." maxWidth={960}>
      <TextField placeholder="Procure um termo, ex.: fatura" value={search} onChange={e => setSearch(e.target.value)} sx={{ mb: 3 }}
        slotProps={{ input: { startAdornment: <InputAdornment position="start"><SearchRoundedIcon /></InputAdornment> } }} />
      {groups.map(g =>
        <Box key={g.title} sx={{ mb: 3 }}>
          <Typography variant="overline" sx={{ color: 'secondary.main' }}>{g.title}</Typography>
          <Box sx={{ display: 'grid', gap: 1.5, gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' } }}>
            {g.ids.map(id =>
              <Card key={id} sx={{ p: 2 }}>
                <Typography sx={{ fontWeight: 800 }}>{Terms[id].term}</Typography>
                <Typography variant="body2" sx={{ mt: 0.5 }}>{Terms[id].short}</Typography>
                {Terms[id].example &&
                  <Typography variant="body2" sx={{ mt: 1, color: 'text.secondary', fontStyle: 'italic' }}>Ex.: {Terms[id].example}</Typography>}
              </Card>
            )}
          </Box>
        </Box>
      )}
      {!groups.length && <Typography sx={{ color: 'text.secondary' }}>Nenhum termo encontrado para "{search}".</Typography>}
    </Page>
  )
}
