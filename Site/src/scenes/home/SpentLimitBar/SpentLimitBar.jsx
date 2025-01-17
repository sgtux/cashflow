import React, { useState, useEffect } from 'react'
import { toReal } from '../../../helpers'

import { Container, ContainerBar, FillBar, Label } from './styles'
import { Colors } from '../../../helpers/themes'

export function SpentLimitBar({ description, spent, limit }) {

    const [color, setColor] = useState(Colors.AppGreen)
    const [percent, setPercent] = useState(0)

    useEffect(() => setPercent((spent * 100) / limit), [spent, limit])

    useEffect(() => {
        if (percent < 65) {
            setColor(Colors.AppGreen)
        } else if (percent < 80) {
            setColor(Colors.AppYellow)
        } else if (percent < 95) {
            setColor(Colors.AppRed)
        } else {
            setColor(Colors.AppRedDark)
        }
    }, [percent])

    return (
        <Container>
            <Label>{description}</Label>
            <ContainerBar>
                <FillBar percent={percent} />
            </ContainerBar>
            <Label style={{ color }}>{`${toReal(spent)} / ${toReal(limit)}`}</Label>
        </Container>
    )
}