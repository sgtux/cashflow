import styled from 'styled-components'
import { CurrencyInput } from 'react-currency-mask'

export const InputMoney = styled(CurrencyInput)`
    color: var(--mui-palette-text-secondary);
    background-color: transparent;
    border: solid 0;
    border-bottom: solid 1px var(--mui-palette-text-secondary);
    margin: 10px;
    width: 100px;
    font-family: GraphikMedium;
`

const DefaultInput = styled.input`
    color: var(--mui-palette-text-secondary);
    background-color: transparent;
    border: solid 0;
    border-bottom: solid 1px var(--mui-palette-text-secondary);
    margin: 10px;
    width: 100px;
    font-family: GraphikMedium;
`

export const InputNumbers = DefaultInput

export const DatePickerInput = styled.input`
    color: var(--mui-palette-text-secondary);
    background-color: transparent;
    border: 0;
    border-bottom: solid 1px var(--mui-palette-text-secondary);
    margin: 10px;
    width: 100px;
    font-family: GraphikMedium;
    font-size: 16px;
`

export const InputText = styled.input`
    color: var(--mui-palette-text-secondary);
    background-color: transparent;
    border: 0;
    border-bottom: solid 1px var(--mui-palette-text-secondary);
    margin: 10px;
    width: 80px;
    font-family: GraphikMedium;
    font-size: 16px;
`

export const InputLabel = styled.span`
    color: var(--mui-palette-text-secondary);
    font-family: GraphikMedium;
    font-size: 16px;
`

export const DatePickerContainer = styled.div`
    & div.react-datepicker-wrapper, & div.react-datepicker__input-container {
        display: inline;
    }

    & .react-datepicker-popper {
        z-index: 10;
    }

    & .react-datepicker {
        background-color: var(--mui-palette-background-paper);
        color: var(--mui-palette-text-primary);
        border-color: var(--mui-palette-divider);
    }

    & .react-datepicker__header {
        background-color: var(--mui-palette-action-hover);
        border-bottom-color: var(--mui-palette-divider);
    }

    & .react-datepicker__current-month,
    & .react-datepicker__day-name,
    & .react-datepicker__day,
    & .react-datepicker__time-name {
        color: var(--mui-palette-text-primary);
    }

    & .react-datepicker__navigation-icon::before,
    & .react-datepicker__year-read-view--down-arrow,
    & .react-datepicker__month-read-view--down-arrow,
    & .react-datepicker__month-year-read-view--down-arrow {
        border-color: var(--mui-palette-text-secondary);
    }

    & .react-datepicker__day:hover,
    & .react-datepicker__month-text:hover,
    & .react-datepicker__quarter-text:hover,
    & .react-datepicker__year-text:hover {
        background-color: var(--mui-palette-action-hover);
    }

    & .react-datepicker__day--selected,
    & .react-datepicker__day--keyboard-selected {
        background-color: var(--mui-palette-primary-main);
        color: var(--mui-palette-primary-contrastText);
    }

    & .react-datepicker__day--selected:hover,
    & .react-datepicker__day--keyboard-selected:hover {
        background-color: var(--mui-palette-primary-dark);
    }

    & .react-datepicker__day--disabled,
    & .react-datepicker__day--outside-month {
        color: var(--mui-palette-text-disabled);
    }

    & .react-datepicker-popper[data-placement^='bottom'] .react-datepicker__triangle {
        fill: var(--mui-palette-action-hover);
        color: var(--mui-palette-action-hover);
    }

    & .react-datepicker-popper[data-placement^='top'] .react-datepicker__triangle {
        fill: var(--mui-palette-background-paper);
        color: var(--mui-palette-background-paper);
    }

    & .react-datepicker-popper .react-datepicker__triangle {
        stroke: var(--mui-palette-divider);
    }
`