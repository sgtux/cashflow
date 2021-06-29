import styled from 'styled-components'

export const InstallmentTable = styled.div`
    overflow-y: auto;
    max-height: 320px;
    box-shadow: 0 0 3px black;
    margin: 5px;
    border-radius: 10px;
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
        background-color: #ededed;
    }
`