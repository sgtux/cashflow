import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { MuiThemeProvider } from '@material-ui/core'
import { MainComponent } from './components/main/MainComponent'

import { AppTheme } from './helpers/themes'
import { Store } from './store'

import './App.css'
import 'react-toastify/dist/ReactToastify.css'
import 'react-datepicker/dist/react-datepicker.css'

class App extends React.Component {
  render() {
    return (
      <Provider store={Store}>
        <MuiThemeProvider theme={AppTheme}>
          <MainComponent />
        </MuiThemeProvider>
      </Provider>
    )
  }
}

if (!location.host.includes('localhost') && location.protocol === 'http:')
  location = location.href.replace('http', 'https')

ReactDOM.render(<App />, document.getElementById('app'))