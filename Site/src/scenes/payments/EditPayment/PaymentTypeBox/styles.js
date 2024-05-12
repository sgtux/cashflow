import styled from 'styled-components'

import {
    FormControl
} from '@mui/material'

export const Container = styled(FormControl)`
    width: 200px;
    margin-left: 20px;
    margin-top: 10px;
`

export const MenuItemSpan = styled.span`
    color: ${props => props.gain ? 'green' : 'red'};
    font-weight: 'bold';
`