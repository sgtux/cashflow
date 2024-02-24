import React from 'react'
import { useSelector, useDispatch } from 'react-redux'
import List from '@material-ui/core/List'
import Divider from '@material-ui/core/Divider'
import { Link } from 'react-router-dom'

import { MenuItemContainer } from './styles'
import { menuChanged } from '../../../store/actions'

import {
  AnalysisIcon,
  ChartPieIcon,
  CreditCardIcon,
  HomeIcon,
  MoneyBagIcon,
  MoneyExpenseIcon,
  MoneyIncomeIcon,
  RecurringExpenseIcon,
  VehicleIcon
} from '../../icons'

const styles = {
  symbolDiv: {
    textAlign: 'center',
    width: '260px'
  },
  subMainText: {
    color: '#FFF',
    fontFamily: 'Permanent Marker',
    fontSize: '20px',
    textTransform: 'uppercase',
    marginLeft: 10
  }
}

const LinkListItem = ({ to, text, onClick, icon }) => {

  const selectedMenu = useSelector(state => state.appState.selectedMenu)

  const dispatch = useDispatch()

  function menuClicked() {
    dispatch(menuChanged(to))
    onClick()
  }

  return (
    <Link to={to}
      onClick={() => menuClicked()}
      style={{ textDecoration: 'none' }}>
      <MenuItemContainer selected={selectedMenu === to}>
        {React.createElement(icon, { selected: selectedMenu === to })}
        <span>{text}</span>
      </MenuItemContainer>
    </Link>
  )
}

export function SidebarContent({ closeSidebar }) {

  return (
    <div>
      <div style={styles.symbolDiv}>
        <img src="favicon.ico" height="80" width="80" style={{ marginTop: 20 }} />
      </div>
      <List>
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/" text="Home" icon={HomeIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/payments" text="Parcelamentos" icon={ChartPieIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/projection" text="Projeção" icon={AnalysisIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/credit-cards" text="Cartões" icon={CreditCardIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/earnings" text="Proventos" icon={MoneyIncomeIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/household-expenses" text="Despesas" icon={MoneyExpenseIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/vehicles" text="Veículos" icon={VehicleIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/recurring-expenses" text="Recorrentes" icon={RecurringExpenseIcon} />
        <Divider />
        <LinkListItem onClick={() => closeSidebar()} to="/remaining-balance" text="Remanescente" icon={MoneyBagIcon} />
        <Divider />
      </List>
    </div>
  )
}