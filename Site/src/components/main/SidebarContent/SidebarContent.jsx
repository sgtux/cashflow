import React from 'react'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemText from '@material-ui/core/ListItemText'
import Divider from '@material-ui/core/Divider'
import { Link } from 'react-router-dom'

import { getCurrentPath } from '../AppRouter'

const styles = {
  mainIcon: {
    color: 'white'
  },
  symbolDiv: {
    textAlign: 'center',
    width: '260px'
  },
  symbolSpan: {
    color: '#FFF',
    fontFamily: 'FrederickaGreat',
    fontSize: '60px'
  },
  subMainText: {
    color: '#FFF',
    marginLeft: '30px',
    fontSize: '14px'
  }
}

const LinkListItem = props => {
  const clickHandle = e => {
    if (getCurrentPath() === props.to)
      e.preventDefault()
    props.onClick()
  }
  return (
    <Link to={props.to}
      onClick={e => clickHandle(e)}
      style={{ textDecoration: 'none' }}>
      <ListItem button >
        <ListItemText primary={<span style={styles.subMainText}>{props.text}</span>} />
      </ListItem>
    </Link>
  )
}

export function SidebarContent({ closeSidebar }) {

  return (
    <div>
      <div style={styles.symbolDiv}>
        <span style={styles.symbolSpan}>
          <img src="favicon.ico" height="80" width="80" style={{ marginTop: 20 }} />
        </span>
      </div>
      <List>
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/payments" text="Pagamentos" />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/projection" text="Projeção" />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/credit-cards" text="Cartões de Crédito" />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/salary" text="Salários" />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/daily-expenses" text="Despesas Diárias" />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/vehicles" text="Veículos" />
        <Divider />
      </List>
    </div>
  )
}