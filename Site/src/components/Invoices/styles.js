import styled from 'styled-components'

import { Colors } from '../../helpers/themes'

const InvoiceCostSpan = styled.span`
    color: ${Colors.AppRed};
    margin-top: 6px;
    padding: 3px;
    font-family: "Roboto","Helvetica","Arial",sans-serif;
`

export const InvoiceCostSmall = styled(InvoiceCostSpan)`
    font-size: 10px;
`

export const InvoiceCost = styled(InvoiceCostSpan)`
    font-size: 12px;
`

export const InvoiceTotalCost = styled(InvoiceCostSpan)`
    font-size: 16px;
`