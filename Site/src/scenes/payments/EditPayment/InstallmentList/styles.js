import styled from 'styled-components'

export const Container = styled.div`
    text-align: center;
    margin-top: 20px;
`

export const InstallmentTable = styled.div`
    overflow-y: auto;
    margin: 5px;
    max-height: 450px;
    & > table {
        border-collapse: collapse;
        width: 100%;
        font-size: 12px;
    }
    & thead th {
        position: sticky;
        top: 0;
        background-color: white;
        padding-top: 4px;
        font-size: 14px;
    }
    & tr:nth-child(even) {
        background-color: #ededed;
    }
`