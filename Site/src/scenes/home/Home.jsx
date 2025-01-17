import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import Grid from '@mui/material/Grid2'
import { Divider, List, ListItem, ListItemIcon, ListItemText, Paper, styled } from '@mui/material'
import WarningIcon from '@mui/icons-material/Warning'

import { InputMonth, MainContainer } from '../../components'
import { Colors } from '../../helpers/themes'

import { homeService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { toReal } from '../../helpers'
import { GridTitle } from './styles'
import { SpentLimitBar } from './SpentLimitBar/SpentLimitBar'

const Item = styled(Paper)(({ theme }) => ({
    backgroundColor: '#fff',
    ...theme.typography.body2,
    padding: theme.spacing(1),
    textAlign: 'center',
    color: theme.palette.text.secondary,
    ...theme.applyStyles('dark', {
        backgroundColor: '#1A2027',
    }),
}));

export function Home() {

    const [selectedDate, setSelectedDate] = useState({ month: '', year: '' })
    const [pendingPayments, setPendingPayments] = useState([])
    const [inflows, setInflows] = useState([])
    const [outflows, setOutflows] = useState([])
    const [limitValues, setLimitValues] = useState([])
    const [totalInflows, setTotalInflows] = useState(0)
    const [totalOutflows, setTotalOutflows] = useState(0)

    const dispatch = useDispatch()

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        refresh({ month, year })
    }, [])

    async function refresh(date) {
        if (date.month && date.year) {
            setSelectedDate(date)
            dispatch(showGlobalLoader())
            try {
                const res = await homeService.getChart(date.month, date.year)                
                setPendingPayments(res.pendingPayments)
                setInflows(res.inflows)
                setOutflows(res.outflows)
                setLimitValues(res.limitValues)
                setTotalInflows(res.totalInflows)
                setTotalOutflows(res.totalOutflows)
            } catch (err) {
                console.log(err)
            } finally {
                dispatch(hideGlobalLoader())
            }
        }
    }

    return (
        <MainContainer title="Home">
            <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 1, md: 2 }}>
                <Grid size={6} padding={0} margin={0}>
                    <InputMonth
                        startYear={(new Date().getFullYear()) - 1}
                        selectedMonth={selectedDate.month}
                        selectedYear={selectedDate.year}
                        countYears={2}
                        label="Mês Referência:"
                        onChange={(month, year) => refresh({ month, year })} />
                    <Item>
                        <List>
                            <GridTitle>Entradas - <span style={{ color: Colors.AppGreen }}>{toReal(totalInflows)}</span></GridTitle>
                            {inflows.map((p, i) =>
                                <div key={i}>
                                    <Divider />
                                    <ListItem>
                                        <ListItemText>
                                            {p.description}
                                        </ListItemText>
                                        <ListItemText style={{ textAlign: 'right', color: Colors.AppGreen }}>
                                            {toReal(p.value)}
                                        </ListItemText>
                                    </ListItem>
                                </div>
                            )}
                        </List>
                    </Item>
                </Grid>
                <Grid size={6}>
                    <Item>
                        <List>
                            <GridTitle>Saídas - <span style={{ color: Colors.AppRed }}>{toReal(totalOutflows)}</span></GridTitle>
                            {outflows.map((p, i) =>
                                <div key={i}>
                                    <Divider />
                                    <ListItem>
                                        <ListItemText>
                                            {p.description}
                                        </ListItemText>
                                        <ListItemText style={{ textAlign: 'right', color: Colors.AppRed }}>
                                            {toReal(p.value)}
                                        </ListItemText>
                                    </ListItem>
                                </div>
                            )}
                        </List>
                    </Item>
                </Grid>
            </Grid>
            <Grid container rowSpacing={1} marginTop={1} columnSpacing={{ xs: 1, sm: 1, md: 2 }}>
                <Grid size={6}>
                    <Item>
                        <List>
                            <GridTitle>Pendências</GridTitle>
                            {pendingPayments.map((p, i) =>
                                <div key={i}>
                                    <Divider />
                                    <ListItem>
                                        <ListItemIcon>
                                            <WarningIcon color='warning' />
                                        </ListItemIcon>
                                        <ListItemText>
                                            {p.description}
                                        </ListItemText>
                                        <ListItemText style={{ textAlign: 'right', color: Colors.AppRed }}>
                                            {toReal(p.value)}
                                        </ListItemText>
                                    </ListItem>
                                </div>
                            )}
                        </List>
                    </Item>
                </Grid>
                <Grid size={6}>
                    <Paper style={{ padding: 4, marginBottom: 4 }}>
                        <GridTitle>Limites</GridTitle>
                        <Divider />
                        {limitValues.map((p, i) =>
                            <SpentLimitBar key={i} description={p.description} spent={p.spent} limit={p.limit} />
                        )}
                    </Paper>
                </Grid>
            </Grid>
        </MainContainer>
    )
}