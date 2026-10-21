import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { HashRouter } from 'react-router-dom'
import { ToastContainer } from 'react-toastify'
import { Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Drawer, LinearProgress, Typography } from '@mui/material'
import { useTheme, useColorScheme } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'

import { Sidebar } from './Sidebar'
import { TopBar } from './TopBar'
import { AppRouter } from './AppRouter'
import { Auth } from '../scenes/auth/Auth'
import { userChanged } from '../store/actions'
import { authService } from '../services'
import { registerCallbackUnauthorized } from '../services/httpService'

const DRAWER_WIDTH = 280

export function MainComponent() {
  const [sidebarOpen, setSidebarOpen] = useState(false)
  const [sessionExpired, setSessionExpired] = useState(false)

  const theme = useTheme()
  const { setMode } = useColorScheme()
  const docked = useMediaQuery(theme.breakpoints.up('lg'))

  const { user, loaderCount, themeMode } = useSelector(state => state.appState)
  const dispatch = useDispatch()

  useEffect(() => { setMode(themeMode) }, [themeMode])

  useEffect(() => {
    registerCallbackUnauthorized(() => setSessionExpired(true))
  }, [])

  function logout() {
    setSessionExpired(false)
    authService.logout()
    dispatch(userChanged(null))
  }

  return (
    <>
      {user ?
        <HashRouter>
          <Box sx={{ display: 'flex', minHeight: '100vh', bgcolor: 'background.default' }}>
            <Drawer
              variant={docked ? 'permanent' : 'temporary'}
              open={docked || sidebarOpen}
              onClose={() => setSidebarOpen(false)}
              ModalProps={{ keepMounted: true }}
              sx={{
                width: docked ? DRAWER_WIDTH : 0,
                flexShrink: 0,
                '& .MuiDrawer-paper': { width: DRAWER_WIDTH, boxSizing: 'border-box', borderRight: 1, borderColor: 'divider', bgcolor: 'background.paper' }
              }}>
              <Sidebar onNavigate={() => setSidebarOpen(false)} />
            </Drawer>
            <Box component="main" sx={{ flexGrow: 1, minWidth: 0 }}>
              <TopBar showMenuButton={!docked} onMenuClick={() => setSidebarOpen(true)} />
              <AppRouter />
            </Box>
          </Box>
        </HashRouter>
        :
        <Auth />
      }
      <ToastContainer position="bottom-right" theme={themeMode === 'dark' ? 'dark' : 'light'} />
      <Dialog open={sessionExpired} onClose={logout} maxWidth="xs" fullWidth>
        <DialogTitle sx={{ fontWeight: 800 }}>Sua sessão expirou</DialogTitle>
        <DialogContent>
          <Typography sx={{ color: 'text.secondary' }}>
            Por segurança, você foi desconectado depois de um tempo. Entre de novo para continuar de onde parou.
          </Typography>
        </DialogContent>
        <DialogActions sx={{ px: 3, pb: 2.5 }}>
          <Button variant="contained" onClick={logout} autoFocus>Entrar novamente</Button>
        </DialogActions>
      </Dialog>
      {loaderCount > 0 &&
        <Box sx={{ position: 'fixed', inset: 0, zIndex: theme.zIndex.modal + 1, cursor: 'progress' }}>
          <LinearProgress color="secondary" sx={{ height: 4 }} />
        </Box>
      }
    </>
  )
}
