import React, { useState } from 'react'
import { useDispatch, useSelector } from 'react-redux'
import { useNavigate } from 'react-router-dom'
import { AppBar, Avatar, Box, Divider, IconButton, ListItemIcon, Menu, MenuItem, Toolbar, Tooltip, Typography } from '@mui/material'
import MenuRoundedIcon from '@mui/icons-material/MenuRounded'
import LightModeRoundedIcon from '@mui/icons-material/LightModeRounded'
import DarkModeRoundedIcon from '@mui/icons-material/DarkModeRounded'
import TuneRoundedIcon from '@mui/icons-material/TuneRounded'
import LogoutRoundedIcon from '@mui/icons-material/LogoutRounded'
import MenuBookRoundedIcon from '@mui/icons-material/MenuBookRounded'

import { authService } from '../services'
import { userChanged, themeModeChanged } from '../store/actions'

function greeting() {
  const hour = new Date().getHours()
  if (hour < 12) return 'Bom dia'
  if (hour < 18) return 'Boa tarde'
  return 'Boa noite'
}

export function TopBar({ showMenuButton, onMenuClick }) {
  const [anchor, setAnchor] = useState(null)
  const { user, themeMode } = useSelector(state => state.appState)
  const dispatch = useDispatch()
  const navigate = useNavigate()

  function go(path) {
    setAnchor(null)
    navigate(path)
  }

  function logout() {
    setAnchor(null)
    authService.logout()
    dispatch(userChanged(null))
  }

  const name = ((user || {}).email || '').split('@')[0]

  return (
    <AppBar position="sticky" color="inherit" elevation={0} sx={{ borderBottom: 1, borderColor: 'divider', bgcolor: 'background.default' }}>
      <Toolbar sx={{ gap: 1 }}>
        {showMenuButton &&
          <IconButton edge="start" aria-label="Abrir menu" onClick={onMenuClick}><MenuRoundedIcon /></IconButton>
        }
        <Box sx={{ flex: 1, minWidth: 0 }}>
          <Typography sx={{ fontWeight: 800, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap' }}>
            {greeting()}{name ? `, ${name}` : ''}!
          </Typography>
          <Typography variant="caption" sx={{ color: 'text.secondary', display: { xs: 'none', sm: 'block' } }}>
            Um passo de cada vez: registrar, entender e planejar.
          </Typography>
        </Box>
        <Tooltip title={themeMode === 'dark' ? 'Usar tema claro' : 'Usar tema escuro'}>
          <IconButton onClick={() => dispatch(themeModeChanged(themeMode === 'dark' ? 'light' : 'dark'))}>
            {themeMode === 'dark' ? <LightModeRoundedIcon /> : <DarkModeRoundedIcon />}
          </IconButton>
        </Tooltip>
        <Tooltip title="Sua conta">
          <IconButton onClick={e => setAnchor(e.currentTarget)} sx={{ p: 0.5 }}>
            <Avatar src={(user || {}).picture || undefined} sx={{ width: 36, height: 36, bgcolor: 'primary.main' }}>
              {(name[0] || '?').toUpperCase()}
            </Avatar>
          </IconButton>
        </Tooltip>
        <Menu anchorEl={anchor} open={!!anchor} onClose={() => setAnchor(null)}
          anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }} transformOrigin={{ vertical: 'top', horizontal: 'right' }}>
          <Box sx={{ px: 2, py: 1 }}>
            <Typography variant="body2" sx={{ fontWeight: 700 }}>{(user || {}).email}</Typography>
          </Box>
          <Divider />
          <MenuItem onClick={() => go('/account')}>
            <ListItemIcon><TuneRoundedIcon fontSize="small" /></ListItemIcon>Minha conta e limites
          </MenuItem>
          <MenuItem onClick={() => go('/glossary')}>
            <ListItemIcon><MenuBookRoundedIcon fontSize="small" /></ListItemIcon>Glossário
          </MenuItem>
          <Divider />
          <MenuItem onClick={logout}>
            <ListItemIcon><LogoutRoundedIcon fontSize="small" /></ListItemIcon>Sair
          </MenuItem>
        </Menu>
      </Toolbar>
    </AppBar>
  )
}
