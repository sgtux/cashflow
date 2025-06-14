import styled from 'styled-components'

import { Colors } from '../helpers/themes'

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

export const MoneySpan = styled.span`
    color: ${props => props.$gain ? Colors.AppGreen : Colors.AppRed};
    font-size: ${props => getFont(props)};
    font-family: GraphikMedium;
    font-weight: ${props => props.$bold ? 'bold' : 'normal'};
    font-size: ${props => getFont(props)};
`