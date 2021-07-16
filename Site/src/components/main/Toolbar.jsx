import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import { AppBar, Toolbar, Button } from '@material-ui/core/'
import Typography from '@material-ui/core/Typography'
import IconButton from '@material-ui/core/IconButton'
import * as Icons from '@material-ui/icons'

import { userChanged } from '../../actions'
import { authService } from '../../services'

const styles = {
  root: {
    flexGrow: 1,
  },
  grow: {
    flexGrow: 2,
    textTransform: 'uppercase',
    color: '#FFF',
    fontFamily: 'FrederickaGreat',
    fontSize: '40px'
  },
  menuButton: {
    marginLeft: -12,
    marginRight: 20,
  }
}

export function AppToolbar({ openSideBar, dockedMenu }) {

  const dispatch = useDispatch()

  const appState = useSelector(state => state.appState)
  console.log(appState.user.nickName)

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
            Fluxo de caixa (R$)
          </Typography>
          {appState.user.nickName}
          <Button color="secondary" onClick={() => logout()}>
            <Icons.ExitToApp style={{ color: '#FFF' }} />
          </Button>
        </Toolbar>
      </AppBar>
    </div>
  )
}