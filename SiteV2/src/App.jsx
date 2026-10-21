import React from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux'
import { ThemeProvider } from '@mui/material/styles'
import CssBaseline from '@mui/material/CssBaseline'
import { broadcastResponseToMainFrame } from '@azure/msal-browser/redirect-bridge'

import 'react-toastify/dist/ReactToastify.css'

import { MainComponent } from './layout/MainComponent'
import { AppTheme } from './theme'
import { Store } from './store'

function isMsalBridgeResponse() {
  const hash = window.location.hash.replace(/^#/, '')
  const search = window.location.search.replace(/^\?/, '')
  return new URLSearchParams(hash).has('state') || new URLSearchParams(search).has('state')
}

function App() {
  return (
    <Provider store={Store}>
      <ThemeProvider theme={AppTheme} defaultMode="light">
        <CssBaseline />
        <MainComponent />
      </ThemeProvider>
    </Provider>
  )
}

if (!location.host.includes('localhost') && location.protocol === 'http:')
  location = location.href.replace('http', 'https')

if (isMsalBridgeResponse()) {
  broadcastResponseToMainFrame().catch(err => console.error('MSAL redirect bridge error:', err))
} else {
  createRoot(document.getElementById('app')).render(<App />)
}
