import React from 'react'
import { createRoot } from 'react-dom/client'

import { Provider } from 'react-redux'
import { ThemeProvider } from '@mui/material/styles'
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
        <ThemeProvider theme={AppTheme}>
          <MainComponent />
        </ThemeProvider>
      </Provider>
    )
  }
}

if (!location.host.includes('localhost') && location.protocol === 'http:')
  location = location.href.replace('http', 'https')

const domNode = document.getElementById('app')
const root = createRoot(domNode)
root.render(<App />)