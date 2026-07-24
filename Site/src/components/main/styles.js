import { styled } from '@mui/material/styles'

export const UserPicture = styled('img')(({ theme }) => ({
    height: 50,
    borderRadius: '50%',
    boxShadow: `1px 1px 6px 0px ${theme.palette.divider}`,
    '&:hover': {
        cursor: 'pointer'
    }
}))

export const ToolbarMenuContainer = styled('div', {
    shouldForwardProp: prop => prop !== '$show'
})(({ theme, $show }) => ({
    textDecoration: 'none',
    color: theme.palette.text.primary,
    backgroundColor: theme.palette.background.paper,
    boxShadow: `1px 1px 10px ${theme.palette.divider}`,
    position: 'fixed',
    top: 70,
    right: 6,
    width: 150,
    height: $show ? 75 : 0,
    transition: '300ms',
    overflow: 'hidden',
    borderRadius: 4,
    '& > button': {
        textAlign: 'center',
        width: '100%'
    },
    '& > button:hover': {
        backgroundColor: theme.palette.action.hover
    }
}))