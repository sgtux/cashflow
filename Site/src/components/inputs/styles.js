import styled from 'styled-components'
import { CurrencyInput } from 'react-currency-mask'

export const InputMoney = styled(CurrencyInput)`
    color: #666;
    background-color: white;
    border: solid 0;
    border-bottom: solid 1px #666;
    margin: 10px;
    width: 100px;
    font-family: Roboto Helvetica Arial sans-serif;
`

const DefaultInput = styled.input`
    color: #666;
    background-color: white;
    border: solid 0;
    border-bottom: solid 1px #666;
    margin: 10px;
    width: 100px;
    font-family: Roboto Helvetica Arial sans-serif;
`

export const InputNumbers = DefaultInput

export const DatePickerInput = styled.input`
    color: #666;
    border: 0;
    border-bottom: solid 1px #666;
    margin: 10px;
    width: 80px;
    font-family: Roboto Helvetica Arial sans-serif;
    font-size: 16px;
`

export const InputText = styled.input`
    color: #666;
    border: 0;
    border-bottom: solid 1px #666;
    margin: 10px;
    width: 80px;
    font-family: Roboto Helvetica Arial sans-serif;
    font-size: 16px;
`

export const InputLabel = styled.span`
    color: #666;
    font-family: Roboto Helvetica Arial sans-serif;
    font-size: 16px;
`

export const DatePickerContainer = styled.div`
    & div.react-datepicker-wrapper, & div.react-datepicker__input-container {
        display: inline;
    }
`