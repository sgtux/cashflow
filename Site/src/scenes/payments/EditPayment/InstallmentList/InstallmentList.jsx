import React from 'react'

import { toDateFormat, toReal } from '../../../../helpers'
import { InstallmentTable, Container} from './styles'
import { TableActionPayButton, TableActionEditButton, TableActionExemptButton } from '../../../../components'

export function InstallmentList({ installments, hide, onEdit, onPay, onExempt }) {

    function pay(p) {
        p.paidDate = p.date
        p.paidValue = p.value
        p.exempt = false
        onPay(p)
    }

    function exempt(p) {
        p.paidDate = null
        p.paidValue = null
        p.exempt = true
        onExempt(p)
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
                                <td>{p.exempt ? 'ISENTO' : p.paidValue ? toReal(p.paidValue) : '-'}</td>
                                <td>{p.exempt ? 'ISENTO' : p.paidDate ? toDateFormat(p.paidDate, 'dd/MM/yyyy') : '-'}</td>
                                <td>
                                    <TableActionEditButton onClick={() => onEdit(p)}>editar</TableActionEditButton>
                                    {!p.paidDate && <TableActionPayButton onClick={() => pay(p)}>pagar</TableActionPayButton>}
                                    {!p.paidDate && <TableActionExemptButton onClick={() => exempt(p)}>isentar</TableActionExemptButton>}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </InstallmentTable>
        </Container>
    )
}