import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import Grid from '@mui/material/Grid2'
import { Divider, List, ListItem, ListItemIcon, ListItemText, Paper, styled } from '@mui/material'
import WarningIcon from '@mui/icons-material/Warning'

import { MainContainer } from '../../components'
import { InputMonth } from '../../components/inputs'

import { MonthExpensesChart } from './MonthExpensesChart/MonthExpensesChart'

import { homeService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { toReal } from '../../helpers'
import {PendenciesTitle} from './styles'

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
    const [homeChart, setHomeChart] = useState([])
    const [pendingPayments, setPendingPayments] = useState([])

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
                setHomeChart(res.chartInfos)
                setPendingPayments(res.pendingPayments)
            } catch (err) {
                console.log(err)
            } finally {
                dispatch(hideGlobalLoader())
            }
        }
    }

    return (
        <MainContainer title="Home">
            <Grid container rowSpacing={1} columnSpacing={{ xs: 1, sm: 2, md: 3 }}>
                <Grid size={6}>
                    <Item>
                        <div>
                            <InputMonth
                                startYear={(new Date().getFullYear()) - 1}
                                selectedMonth={selectedDate.month}
                                selectedYear={selectedDate.year}
                                countYears={2}
                                label="Mês Referência:"
                                onChange={(month, year) => refresh({ month, year })} />
                            <MonthExpensesChart data={homeChart} />
                        </div>
                    </Item>
                </Grid>
                <Grid size={6}>
                    <Item>
                        <List>
                            <PendenciesTitle>Pendências</PendenciesTitle>
                            {pendingPayments.map(p =>
                                <>
                                    <Divider />
                                    <ListItem disablePadding>
                                        <ListItemIcon>
                                            <WarningIcon color='error' />
                                        </ListItemIcon>
                                        <ListItemText>
                                            {p.description}
                                        </ListItemText>
                                        <ListItemText style={{ textAlign: 'right' }}>
                                            {toReal(p.value)}
                                        </ListItemText>
                                    </ListItem>
                                </>
                            )}
                        </List>
                    </Item>
                </Grid>
            </Grid>
        </MainContainer>
    )
}