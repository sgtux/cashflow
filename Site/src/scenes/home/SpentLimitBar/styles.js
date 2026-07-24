import { styled } from '@mui/material/styles'

export const Container = styled('div')({
    margin: 20,
    textAlign: 'center'
})

export const ContainerBar = styled('div')(({ theme }) => ({
    display: 'flex',
    justifyContent: 'right',
    height: 20,
    borderRadius: 10,
    width: '90%',
    margin: '0 auto',
    marginBottom: 10,
    overflow: 'hidden',
    backgroundImage: `linear-gradient(to right, ${theme.palette.success.main} 60%, ${theme.palette.warning.main} 80%, ${theme.palette.error.main} 95%, ${theme.palette.error.dark})`
}))

export const FillBar = styled('div', {
    shouldForwardProp: prop => prop !== 'percent'
})(({ theme, percent }) => ({
    height: '100%',
    width: `${100 - percent}%`,
    backgroundColor: theme.palette.action.disabledBackground
}))

export const Label = styled('span')(({ theme }) => ({
    fontSize: 16,
    fontWeight: 'bold',
    color: theme.palette.text.secondary,
    fontFamily: '"Roboto","Helvetica","Arial",sans-serif'
}))
