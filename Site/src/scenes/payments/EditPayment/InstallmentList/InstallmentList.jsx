import React from 'react'

import { toDateFormat, toReal } from '../../../../helpers'
import { InstallmentTable, Container, PayButton, EditButton } from './styles'

export function InstallmentList({ installments, hide, onEdit, onPay }) {

    function pay(p) {
        p.paidDate = p.date
        p.paidValue = p.value
        onPay(p)
    }

    return (
        <Container hidden={hide}>
            <InstallmentTable>
                <table>
                    <thead>
                        <tr>
                            <th>N°</th>
                            <th>VALOR</th>
                            <th>DATA VENCIMENTO</th>
                            <th>VALOR PAGO</th>
                            <th>DATA PAGAMENTO</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {installments.map((p, i) =>
                            <tr key={i}>
                                <td>{p.number}</td>
                                <td>{toReal(p.value)}</td>
                                <td>{toDateFormat(p.date, 'dd/MM/yyyy')}</td>
                                <td>{p.paidValue ? toReal(p.paidValue) : '-'}</td>
                                <td>{p.paidDate ? toDateFormat(p.paidDate, 'dd/MM/yyyy') : '-'}</td>
                                <td>
                                    <EditButton onClick={() => onEdit(p)}>editar</EditButton>
                                    {!p.paidDate && <PayButton onClick={() => pay(p)}>pagar</PayButton>}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </InstallmentTable>
        </Container>
    )
}