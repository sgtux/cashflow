import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { HashRouter } from 'react-router-dom'

import { ToastContainer } from 'react-toastify'

import { Box, Drawer, LinearProgress } from '@mui/material'
import { useTheme, useColorScheme } from '@mui/material/styles'
import useMediaQuery from '@mui/material/useMediaQuery'

import { AppToolbar } from './Toolbar'
import { SidebarContent } from './'
import AppRouter from './AppRouter'
import { Auth } from '../../scenes'
import { AlertModal } from '../main/Modal'
import { userChanged } from '../../store/actions'
import { registerCallbackUnauthorized } from '../../services/httpService'
import { ContainerLoader } from './MainContainer/styles'

const DRAWER_WIDTH = 260

export function MainComponent() {

  const [sidebarIsOpen, setSidebarIsOpen] = useState(false)
  const [showModal, setShowModal] = useState(false)

  const theme = useTheme()
  const { setMode } = useColorScheme()
  const sidebarDocked = useMediaQuery(theme.breakpoints.up('lg'))

  const { user, globalLoader, themeMode } = useSelector(state => state.appState)

  const dispatch = useDispatch()

  useEffect(() => {
    setMode(themeMode)
  }, [themeMode])

  useEffect(() => {
    registerCallbackUnauthorized(() => setShowModal(true))
  }, [])

  function logout() {
    setShowModal(false)
    dispatch(userChanged(null))
  }

  return (
    <div>
      {user ?
        <HashRouter>
          <Box sx={{ display: 'flex' }}>
            <Drawer
              variant={sidebarDocked ? 'permanent' : 'temporary'}
              open={sidebarDocked || sidebarIsOpen}
              onClose={() => setSidebarIsOpen(false)}
              ModalProps={{ keepMounted: true }}
              sx={theme => ({
                width: sidebarDocked ? DRAWER_WIDTH : 0,
                flexShrink: 0,
                '& .MuiDrawer-paper': {
                  width: DRAWER_WIDTH,
                  boxSizing: 'border-box',
                  backgroundColor: theme.palette.primary.main,
                  ...theme.applyStyles('dark', {
                    backgroundColor: theme.palette.background.paper
                  })
                }
              })}>
              <SidebarContent closeSidebar={() => setSidebarIsOpen(false)} />
            </Drawer>
            <Box sx={{ flexGrow: 1, minWidth: 0 }}>
              <AppToolbar
                dockedMenu={sidebarDocked}
                openSideBar={() => setSidebarIsOpen(true)}
              />
              <AppRouter />
            </Box>
          </Box>
        </HashRouter>
        :
        <Auth />
      }
      <ToastContainer />
      <AlertModal
        text='Sessão Expirada!'
        show={showModal}
        onClose={() => logout()} />
      {globalLoader &&
        <ContainerLoader>
          <div style={{ width: 200, margin: '0 auto' }}>
            <img src='./images/donald-loader.gif' style={{ borderRadius: '50%', height: 100, width: 100, marginBottom: 10 }} />
            <LinearProgress />
          </div>
        </ContainerLoader>
      }
    </div>
  )
}
