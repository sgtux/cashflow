import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { AppBar, Toolbar, Button, Typography, IconButton } from '@mui/material'
import * as Icons from '@mui/icons-material'
import { Link } from 'react-router-dom'

import { userChanged } from '../../store/actions'
import { authService } from '../../services'
import { UsernameSpan } from './styles'

const styles = {
  root: {
    flexGrow: 1,
  },
  grow: {
    flexGrow: 2,
    textTransform: 'uppercase',
    color: '#FFF',
    fontFamily: 'Permanent Marker',
    fontSize: '30px'
  },
  menuButton: {
    marginLeft: -12,
    marginRight: 20,
  }
}

export function AppToolbar({ openSideBar, dockedMenu }) {

  const dispatch = useDispatch()

  const appState = useSelector(state => state.appState)

  function logout() {
    authService.logout()
    dispatch(userChanged(null))
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
          <Link to="/account"><UsernameSpan>{appState.user.email}</UsernameSpan></Link>
          <Button color="secondary" onClick={() => logout()}>
            <Icons.ExitToApp style={{ color: '#FFF' }} />
          </Button>
        </Toolbar>
      </AppBar>
    </div>
  )
}