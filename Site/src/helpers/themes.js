import { createTheme } from '@mui/material/styles'

export const Colors = {
  AppGreen: '#4b9372',
  AppGreenDark: '#34664f',
  AppRed: '#bb2222',
  AppRedDark: '#880000',
  AppYellow: '#cc6'
}

export const AppTheme = createTheme({
  palette: {
    primary: { main: Colors.AppGreen },
    secondary: { main: Colors.AppRed }
  }
})
