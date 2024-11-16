import styled from 'styled-components'

export const FuelExpensesTable = styled.div`
    overflow-y: auto;
    max-height: 320px;
    margin: 5px;
    font-size: 12px;
    text-align: center;
    & > table {
        border-collapse: collapse;
        width: 100%;
    }
    & thead th {
        position: sticky;
        top: 0;
        background-color: white;
        padding-top: 4px;
    }
    & tr:nth-child(even) {
        background-color: #ddd;
    }
`