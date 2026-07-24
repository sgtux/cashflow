import { styled } from '@mui/material/styles'

export const GridTitle = styled('div')(({ theme }) => ({
    fontSize: 20,
    margin: 10,
    fontWeight: 'bold',
    fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
    color: theme.palette.text.secondary,
    textAlign: 'center'
}))
