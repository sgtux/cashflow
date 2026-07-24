import { alpha, styled } from '@mui/material/styles'

export const MenuItemContainer = styled('div', {
    shouldForwardProp: prop => prop !== 'selected'
})(({ theme, selected }) => ({
    textAlign: 'center',
    margin: 0,
    display: 'flex',
    justifyContent: 'center',
    padding: 10,
    transition: '400ms',
    color: selected ? theme.palette.primary.main : theme.palette.primary.contrastText,
    backgroundColor: selected ? theme.palette.primary.contrastText : 'transparent',
    '&:hover': {
        cursor: 'pointer',
        backgroundColor: selected ? theme.palette.primary.contrastText : alpha(theme.palette.primary.contrastText, 0.2),
        transition: '400ms'
    },
    '& > span': {
        fontFamily: 'GraphikMedium',
        fontSize: 15,
        letterSpacing: '0.3px',
        textTransform: 'uppercase',
        marginLeft: 10
    }
}))