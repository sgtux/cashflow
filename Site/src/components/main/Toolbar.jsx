import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { AppBar, Toolbar, Button, Typography, IconButton } from '@mui/material'
import * as Icons from '@mui/icons-material'
import { useColorScheme } from '@mui/material/styles'

import { userChanged, menuChanged, themeModeChanged } from '../../store/actions'
import { authService } from '../../services'
import { UserPicture, ToolbarMenuContainer } from './styles'

const styles = {
  root: {
    flexGrow: 1,
  },
  grow: {
    flexGrow: 2,
    textTransform: 'uppercase',
    color: 'inherit',
    fontFamily: 'GraphikMedium',
    fontSize: '22px',
    letterSpacing: '0.5px'
  },
  menuButton: {
    marginLeft: -12,
    marginRight: 20,
  }
}

export function AppToolbar({ openSideBar, dockedMenu }) {

  const [showMenu, setShowMenu] = useState(false)

  const { mode } = useColorScheme()

  useEffect(() => {
    document.body.addEventListener('click', (e) => {
      if (e.target.id !== 'user-picture')
        setShowMenu(false)
    })
  }, [])

  const dispatch = useDispatch()

  const appState = useSelector(state => state.appState)

  function logout() {
    authService.logout()
    dispatch(userChanged(null))
  }

  function editAccount() {
    dispatch(menuChanged())
    window.location = '#/account'
  }

  function toggleThemeMode() {
    dispatch(themeModeChanged(mode === 'dark' ? 'light' : 'dark'))
  }

  return (
    <div style={styles.root} >
      <AppBar position='static' color='primary'>
        <Toolbar>
          {
            dockedMenu ? null :
              <IconButton style={styles.menuButton}
                color="inherit"
                aria-label="Menu"
                onClick={() => openSideBar()}>
                <Icons.Menu />
              </IconButton>
          }
          <Typography variant="h2" color="inherit" style={styles.grow}>
            Fluxo de Caixa (R$)
          </Typography>
          <IconButton
            color="inherit"
            aria-label="Alternar tema"
            onClick={() => toggleThemeMode()}>
            {mode === 'dark' ? <Icons.LightMode /> : <Icons.DarkMode />}
          </IconButton>
          <UserPicture id="user-picture" src={appState.user.picture} onClick={e => setShowMenu(!showMenu)} />
          <ToolbarMenuContainer $show={showMenu}>
            <Button onClick={() => editAccount()}>Editar Conta</Button>
            <Button onClick={() => logout()}>Sair</Button>
          </ToolbarMenuContainer>
        </Toolbar>
      </AppBar>
    </div>
  )
}