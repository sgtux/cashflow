import React from 'react'
import { Route, Switch } from 'react-router-dom'
import { createHashHistory } from 'history'
import { CreditCards, Payments, Projection, Salary, EditPayment } from '../../scenes'

const history = createHashHistory()
const isAuthenticated = true
const freeRoutes = ['/', '/about']

let currentPath = location.hash.replace('#', '')

export const getCurrentPath = () => currentPath

history.listen((location, action) => {
  if (freeRoutes.indexOf(location.pathname) === -1 && !isAuthenticated) {
    window.location.hash = '#/'
    currentPath = '/'
  } else
    currentPath = location.pathname
})

export default function () {
  return (
    <Switch>
      <Route path="/" exact={true} component={Projection} />
      <Route path="/payments" component={Payments} />
      <Route path="/credit-cards" component={CreditCards} />
      <Route path="/projection" component={Projection} />
      <Route path="/salary" component={Salary} />
      <Route path="/edit-payment/:id" component={EditPayment} />
    </Switch>
  )
}