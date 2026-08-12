import styled from 'styled-components'

export const Container = styled.div`
    text-align: center;
    margin-top: 20px;
`

export const EarningTable = styled.div`
    overflow-y: auto;
    margin: 5px;
    font-size: 14px;
    & > table {
        border-collapse: collapse;
        width: 100%;
    }
    & thead th {
        font-family: GraphikMedium;
        position: sticky;
        top: 0;
        background-color: var(--mui-palette-background-paper);
        padding-top: 4px;
    }
    & tbody td {
        font-family: GraphikRegular;
    }
    & tr:nth-child(even) {
        background-color: var(--mui-palette-action-hover);
    }
`