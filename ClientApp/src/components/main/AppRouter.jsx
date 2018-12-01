import React from 'react'
import { Route, Switch, Link } from 'react-router-dom'
import createHistory from 'history/createHashHistory'
import MyQuestion from '../../scenes/question/MyQuestion'
import SharedQuestion from '../../scenes/question/SharedQuestion'

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
        <Route path="/my-questions" component={MyQuestion} />
        <Route path="/shared-questions" component={SharedQuestion} />
      </Switch>
    )
  }
}