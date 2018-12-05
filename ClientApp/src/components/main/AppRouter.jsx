import React from 'react'
import { Route, Switch, Link } from 'react-router-dom'
import createHistory from 'history/createHashHistory'
import CreditCards from '../../scenes/credit-cards/CreditCards'
import Payment from '../../scenes/payments/Payment'
import FuturePayment from '../../scenes/payments/Future'

const history = createHistory()
const isAuthenticated = true
const freeRoutes = ['/', '/about']

const App = () => (
  <div>
    <span>App</span><br />
    <Link to="/question">Questions</Link><br />
    <Link to="/about">About</Link>
  </div>
)

const About = () => (
  <div>
    <span>About</span><br />
    <Link to="/">Home</Link>
  </div>
)

let currentPath = location.hash.replace('#', '')

export const getCurrentPath = () => currentPath

history.listen((location, action) => {
  if (freeRoutes.indexOf(location.pathname) === -1 && !isAuthenticated) {
    window.location.hash = '#/'
    currentPath = '/'
  } else
    currentPath = location.pathname
})

export default class AppRouter extends React.Component {
  render() {
    return (
      <Switch>
        <Route path="/" exact={true} component={App} />
        <Route path="/about" component={About} />
        <Route path="/my-payments" component={Payment} />
        <Route path="/credit-cards" component={CreditCards} />
        <Route path="/payment-future" component={FuturePayment} />        
      </Switch>
    )
  }
}