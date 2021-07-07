import React from 'react'
import { IconButton } from '@material-ui/core'

import { EditOutlined as EditIcon } from '@material-ui/icons'

import { toDateFormat, toReal } from '../../../../helpers'
import { InstallmentTable, Container } from './styles'

export function InstallmentList({ installments, hide, onEdit }) {
    return (
        <Container hidden={hide}>
            <InstallmentTable>
                <table>
                    <thead>
                        <tr>
                            <th>N°</th>
                            <th>VALOR</th>
                            <th>VENCIMENTO</th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {installments.map((p, i) =>
                            <tr key={i}>
                                <td>{p.number}</td>
                                <td>{toReal(p.cost)}</td>
                                <td>{toDateFormat(p.date, 'dd/MM/yyyy')}</td>
                                <td>{p.paidDate ? 'PAGO' : 'PENDENTE'}</td>
                                <td>
                                    <IconButton onClick={() => onEdit(p)} color="primary" aria-label="Edit">
                                        <EditIcon />
                                    </IconButton>
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </InstallmentTable>
        </Container>
    )
}