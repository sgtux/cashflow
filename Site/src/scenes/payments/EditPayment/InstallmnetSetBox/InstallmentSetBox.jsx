import React from 'react'

import { InputNumbers } from '../../../../components/inputs'

export function InstallmentSetBox({ hide, qtdInstallments, qtdInstallmentsChanged }) {
    return (
        <div hidden={hide} style={{ color: 'var(--mui-palette-text-secondary)' }}>
            <span>Qtd. Parcelas:</span>
            <InputNumbers
                onChange={e => qtdInstallmentsChanged(e.target.value.replace(/\D/g, ""))}
                value={qtdInstallments} />
        </div>
    )
}