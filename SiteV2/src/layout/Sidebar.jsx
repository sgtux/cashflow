import React from 'react'
import { NavLink } from 'react-router-dom'
import { Box, List, ListItemButton, ListItemIcon, ListItemText, Typography } from '@mui/material'

import { NavigationGroups } from './navigation'
import { BrandMark } from '../components/BrandMark'

function StepBadge({ step }) {
  return (
    <Box component="span" sx={{
      width: 20, height: 20, borderRadius: '50%', fontSize: 12, fontWeight: 800,
      display: 'inline-flex', alignItems: 'center', justifyContent: 'center',
      bgcolor: 'secondary.main', color: 'secondary.contrastText', mr: 1
    }}>{step}</Box>
  )
}

export function Sidebar({ onNavigate }) {
  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
      <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, px: 2.5, py: 2.5 }}>
        <BrandMark size={42} />
        <Box>
          <Typography sx={{ fontWeight: 800, fontSize: 18, lineHeight: 1.1 }}>Bê-a-bá da Grana</Typography>
          <Typography variant="caption" sx={{ color: 'text.secondary' }}>Conta · organize o dia a dia</Typography>
        </Box>
      </Box>

      <Box sx={{ flex: 1, overflowY: 'auto', px: 1.5, pb: 2 }}>
        {NavigationGroups.map((group, i) =>
          <Box key={i} sx={{ mt: i ? 1.5 : 0 }}>
            {group.title &&
              <Typography variant="overline" sx={{ display: 'flex', alignItems: 'center', px: 1.5, color: 'text.secondary', fontSize: 11 }}>
                {group.step && <StepBadge step={group.step} />}
                {group.title}
              </Typography>
            }
            <List dense disablePadding>
              {group.items.map(item =>
                <ListItemButton
                  key={item.to}
                  component={NavLink}
                  to={item.to}
                  end={item.to === '/'}
                  onClick={onNavigate}
                  sx={{
                    borderRadius: 2.5,
                    mb: 0.25,
                    '&.active': {
                      bgcolor: 'primary.main',
                      color: 'primary.contrastText',
                      '& .MuiListItemIcon-root, & .MuiListItemText-secondary': { color: 'inherit' }
                    },
                    '&.active:hover': { bgcolor: 'primary.dark' }
                  }}>
                  <ListItemIcon sx={{ minWidth: 38 }}>
                    <item.icon />
                  </ListItemIcon>
                  <ListItemText
                    primary={item.label}
                    secondary={item.hint}
                    slotProps={{
                      primary: { sx: { fontWeight: 700 } },
                      secondary: { sx: { fontSize: 11.5, opacity: 0.85 } }
                    }} />
                </ListItemButton>
              )}
            </List>
          </Box>
        )}
      </Box>
    </Box>
  )
}
