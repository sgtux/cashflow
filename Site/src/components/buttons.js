import React from 'react'
import { Add } from '@mui/icons-material'
import { Fab } from '@mui/material'
import styled from 'styled-components'

export const TableActionButton = styled.button`
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

export const TableActionPayButton = styled(TableActionButton)`
    background-color: #4b9372;
`

export const TableActionEditButton = styled(TableActionButton)`
    background-color: #3498db;
`

export const TableActionExemptButton = styled(TableActionButton)`
    background-color: #aaa;
`

export const AddFloatingButton = ({ onClick }) => {
    return (
        <Fab style={{ position: 'fixed', right: 50, bottom: 50 }}
            size='large'
            color='primary'
            onClick={() => onClick && onClick()}>
            <Add />
        </Fab>
    )
}