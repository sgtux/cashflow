import React from 'react'
import { List, ListItem, ListItemText, Checkbox } from '@material-ui/core'
import { toDateFormat, toReal } from '../../../../helpers'
import { InstallmentTable } from './styles'

export function InstallmentList({ installments, hide, paidChanged }) {
    return (
        <div hidden={hide}
            style={{ textAlign: 'center', marginTop: '20px' }}>
            <InstallmentTable>
                <table>
                    <thead>
                        <tr>
                            <th>N°</th>
                            <th>VALOR</th>
                            <th>VENCIMENTO</th>
                            <th>PAGA?</th>
                        </tr>
                    </thead>
                    <tbody>
                        {installments.map((p, i) =>
                            <tr>
                                <td>{p.number}</td>
                                <td>{toReal(p.cost)}</td>
                                <td>{toDateFormat(p.date, 'dd/MM/yyyy')}</td>
                                <td>
                                    <Checkbox
                                        checked={!!p.paidDate}
                                        onChange={(e, c) => paidChanged(p, c)}
                                        color="primary"
                                    />
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </InstallmentTable>
        </div>
    )
}