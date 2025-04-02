import { Card } from '@mui/material'
import styled from 'styled-components'

export const EmailSpan = styled.span`
    border: 0;
    border-bottom: 1px solid #aaa;
    margin-left: 14px;
    font-size: 12px;
    color: #888;
    padding-bottom: 4px;
    padding-left: 4px;
    padding-right: 4px;
    width: 300px;
`

export const BenefitsCard = styled(Card)`
    padding: 10px;
    text-align: center;
    ${({ selected }) => selected ? 'border: solid #6cd064 3px;' : ''}
`

export const BenefitsContainer = styled.div`
    font-size: 12px;
    padding: 10px 0;
`

export const PlanTitle = styled.div`
    font-size: 30px;
    margin-top: 10px;
`

export const PlanCost = styled.div`
    font-size: 20px;
`