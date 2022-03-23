import styled from 'styled-components'

export const Container = styled.div`
    text-align: center;
    margin-top: 20px;
`

export const RecurringExpensesTable = styled.div`
    overflow-y: auto;
    margin: 5px;
    font-size: 14px;
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