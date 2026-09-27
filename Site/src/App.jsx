import React from 'react'
import { createRoot } from 'react-dom/client'

import { Provider } from 'react-redux'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import { MainComponent } from './components/main/MainComponent'

import { AppTheme } from './helpers/themes'
import { Store } from './store'

import 'react-toastify/dist/ReactToastify.css'
import 'react-datepicker/dist/react-datepicker.css'

import { broadcastResponseToMainFrame } from '@azure/msal-browser/redirect-bridge'

function isMsalBridgeResponse() {
  const hash = window.location.hash.replace(/^#/, '')
  const search = window.location.search.replace(/^\?/, '')
  return new URLSearchParams(hash).has('state') || new URLSearchParams(search).has('state')
}

class App extends React.Component {
  render() {
    return (
      <Provider store={Store}>
        <ThemeProvider theme={AppTheme}>
          <CssBaseline />
          <MainComponent />
        </ThemeProvider>
      </Provider>
    )
  }
}

if (!location.host.includes('localhost') && location.protocol === 'http:')
  location = location.href.replace('http', 'https')

if (module.hot) {
  console.log('Hot reload is enabled.')
  module.hot.accept('./App', () => {
    const NextApp = require('./App').default
    root.render(<NextApp />)
  })
}

if (isMsalBridgeResponse()) {
  broadcastResponseToMainFrame().catch(err => console.error('MSAL redirect bridge error:', err))
} else {
  var root = createRoot(document.getElementById('app'))
  root.render(<App />)
}