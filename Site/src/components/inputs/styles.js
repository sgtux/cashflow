import styled from 'styled-components'
import TextInputMask from 'react-masked-text'

const DefaultInput = styled(TextInputMask)`
    color: #666;
    background-color: white;
    border: solid 0;
    border-bottom: solid 1px #666;
    margin: 10px;
    width: 100px;
    font-family: Roboto Helvetica Arial sans-serif;
`

export const InputMoney = DefaultInput

export const InputNumbers = DefaultInput

export const InputDate = DefaultInput