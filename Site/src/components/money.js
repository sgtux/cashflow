import { styled } from '@mui/material/styles'

const getFont = props => {
    if (props.$small)
        return '10px'
    if (props.$medium)
        return '12px'
    if (props.$large)
        return '16px'
    if (props.$large2)
        return '20px'
    if (props.$bigger)
        return '30px'
    return '14px'
}

export const MoneySpan = styled('span', {
    shouldForwardProp: prop => !prop.startsWith('$')
})(({ theme, ...props }) => ({
    color: props.$gain ? theme.palette.success.main : theme.palette.error.main,
    fontSize: getFont(props),
    fontFamily: props.$bold ? 'FiraCodeSemiBold' : 'FiraCodeRegular',
    fontVariantNumeric: 'tabular-nums'
}))
