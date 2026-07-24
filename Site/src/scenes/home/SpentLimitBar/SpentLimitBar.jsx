import React, { useState, useEffect } from 'react'
import { useTheme } from '@mui/material/styles'
import { toReal } from '../../../helpers'

import { Container, ContainerBar, FillBar, Label } from './styles'

export function SpentLimitBar({ description, spent, limit }) {

    const theme = useTheme()

    const [color, setColor] = useState(theme.palette.success.main)
    const [percent, setPercent] = useState(0)

    useEffect(() => setPercent((spent * 100) / limit), [spent, limit])

    useEffect(() => {
        if (percent < 65) {
            setColor(theme.palette.success.main)
        } else if (percent < 80) {
            setColor(theme.palette.warning.main)
        } else if (percent < 95) {
            setColor(theme.palette.error.main)
        } else {
            setColor(theme.palette.error.dark)
        }
    }, [percent, theme])

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
