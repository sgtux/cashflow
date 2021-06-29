import React from 'react'
import { Checkbox, FormControlLabel } from '@material-ui/core'

import { InputNumbers } from '../../../../components/inputs'

export function InstallmentSetBox({ hide, qtdInstallments, costByInstallment, costByInstallmentChanged, qtdInstallmentsChanged }) {
    return (
        <div hidden={hide} style={{ color: '#666' }}>
            <FormControlLabel label="Valor por parcela"
                control={<Checkbox
                    value={costByInstallment}
                    onChange={(e, c) => costByInstallmentChanged(c)}
                    color="primary"
                />} />
            <span>Qtd. Parcelas:</span>
            <InputNumbers
                onChangeText={e => qtdInstallmentsChanged(e)}
                kind="only-numbers"
                value={qtdInstallments} />
        </div>
    )
}