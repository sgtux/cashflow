import { createMuiTheme } from '@material-ui/core/styles'

export const Colors = {
  AppGreen: '#4b9372',
  AppGreenDark: '#34664f'
}

export const AppTheme = createMuiTheme({
  palette: {
    primary: { main: Colors.AppGreen },
    secondary: { main: '#bb2222' }
  }
})
