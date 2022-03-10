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

export const ActionButton = styled.button`
    margin: 0 5px;
    text-transform: uppercase;
    border: none;
    border-radius: 4px;
    padding: 4px;
    color: #eee;
    font-size: 10px;
    transition: 300ms;
    &:hover{
        cursor: pointer;
        opacity: .8;
    }
`

export const PayButton = styled(ActionButton)`
    background-color: #4b9372;
`

export const EditButton = styled(ActionButton)`
    background-color: #3498db;
`