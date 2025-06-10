import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import Grid from '@mui/material/Grid2'
import { Divider, List, ListItem, ListItemIcon, ListItemText, Paper, styled } from '@mui/material'
import WarningIcon from '@mui/icons-material/Warning'

import { InputMonth, MainContainer, MoneySpan } from '../../components'
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
    const [pendingTotalValue, setPendingTotalValue] = useState(0)

    const dispatch = useDispatch()

    useEffect(() => {
        const now = new Date()
        let month = now.getMonth() + 1
        let year = now.getFullYear()
        refresh({ month, year })
    }, [])

    useEffect(() => {
        setPendingTotalValue(pendingPayments.reduce((total, item) => total + item.value, 0))
    }, [pendingPayments])

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
            <InputMonth
                startYear={(new Date().getFullYear()) - 1}
                selectedMonth={selectedDate.month}
                selectedYear={selectedDate.year}
                countYears={2}
                label="Mês Referência:"
                onChange={(month, year) => refresh({ month, year })} />
            <div style={{ marginTop: 10 }}></div>
            <Divider />
            <div style={{ marginTop: 10 }}></div>
            <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 1, md: 2 }}>
                <Grid size={6} padding={0} margin={0}>
                    <Item>
                        <List>
                            <GridTitle>Entradas - <MoneySpan $bold $large2 $gain>{toReal(totalInflows)}</MoneySpan></GridTitle>
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
                            <GridTitle>Saídas - <MoneySpan $bold $large2>{toReal(totalOutflows)}</MoneySpan></GridTitle>
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
                            <GridTitle>Pendências - <MoneySpan $bold $large2 $gain={pendingTotalValue <= 0}>{toReal(pendingTotalValue)}</MoneySpan></GridTitle>
                            {pendingPayments.length == 0 ?
                                <div>Sem pendências no momento.</div>
                                : pendingPayments.map((p, i) =>
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
                                )
                            }
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