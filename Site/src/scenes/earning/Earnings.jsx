import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import {
    IconButton,
    Tooltip,
    Paper, Select,
    MenuItem
} from '@mui/material'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    FileCopy as CopyIcon
} from '@mui/icons-material'

import { EditEarning } from './EditEarningModal/EditEarningModal'

import { earningService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'

import { MainContainer, ConfirmModal } from '../../components/main'
import { AddFloatingButton, MoneySpan } from '../../components'
import { toReal, toast, dateToString, isSameMonth } from '../../helpers'

export function Earnings() {

    const [earnings, setEarnings] = useState([])
    const [removeItem, setRemoveItem] = useState(null)
    const [selectedMonthYear, setSelectedMonthYear] = useState('')
    const [monthYearList, setMonthYearList] = useState([])
    const [editEarning, setEditEarning] = useState(null)

    const dispatch = useDispatch()

    useEffect(() => {
        const now = new Date()
        let year = now.getFullYear() - 1
        let month = now.getMonth() + 1
        const list = []
        while (year < now.getFullYear() || (year === now.getFullYear() && month <= now.getMonth())) {
            month++
            if (month > 12) {
                year++
                month = 1
            }
            list.push(`${month}/${year}`)
        }
        setMonthYearList(list)
        setSelectedMonthYear(`${month}/${year}`)
    }, [])

    useEffect(() => { refresh() }, [selectedMonthYear])

    async function refresh() {
        if (selectedMonthYear) {
            const temp = selectedMonthYear.split('/')
            const fromDate = new Date(`${temp[0]}/1/${temp[1]}`)
            try {
                dispatch(showGlobalLoader())
                const res = await earningService.getAll({ fromDate })
                setEarnings(res)
            } catch (ex) {
                console.log(ex)
            } finally {
                dispatch(hideGlobalLoader())
            }
        }
    }

    async function remove() {
        dispatch(showGlobalLoader())
        try {
            await earningService.remove(removeItem.id)

            toast.success('Removido com sucesso!')
            setRemoveItem(null)
            refresh()
        } catch (ex) {
            console.log(ex)
        } finally {
            dispatch(hideGlobalLoader())
        }
    }

    async function copyEarning(earning) {
        let date = new Date(earning.date)
        const now = new Date()
        date = new Date(now.getFullYear(), now.getMonth(), date.getDate())
        earning = { ...earning, id: 0, date }
        dispatch(showGlobalLoader())
        try {
            await earningService.save(earning)
            toast.success('Salvo com sucesso.')
            await refresh()
        } catch (ex) {
            console.log(ex)
        } finally {
            dispatch(hideGlobalLoader())
        }
    }

    return (
        <MainContainer title="Ganhos">

            <Paper>
                <span style={{ margin: 10, fontSize: 16, color: '#666' }}>Desde: </span>
                <Select
                    value={selectedMonthYear}
                    style={{ width: '130px' }}
                    onChange={e => setSelectedMonthYear(e.target.value)}>
                    {monthYearList.map((p, i) => <MenuItem key={i} value={p}>{p}</MenuItem>)}
                </Select>
            </Paper>

            {earnings.map((p, i) =>
                <Paper key={i} style={{ padding: 10, margin: 10, textAlign: 'center', fontFamily: 'GraphikRegular', fontSize: 14 }}>
                    <MoneySpan style={{ fontSize: 16 }} $gain>{toReal(p.value)}</MoneySpan>
                    <span> - {p.description} {p.type !== 2 ? `(${p.typeDescription}) - ` : ' - '}</span>
                    <span>{dateToString(p.date)}</span>
                    <span>
                        {!isSameMonth(new Date(), p.date) &&
                            <Tooltip title="Copiar para o mês atual">
                                <IconButton onClick={() => copyEarning(p)} color="primary" aria-label="Edit">
                                    <CopyIcon />
                                </IconButton>
                            </Tooltip>
                        }
                        <Tooltip title="Editar">
                            <IconButton onClick={() => setEditEarning(p)} color="primary" aria-label="Edit">
                                <EditIcon />
                            </IconButton>
                        </Tooltip>
                        <Tooltip title="Remover">
                            <IconButton onClick={() => setRemoveItem(p)} color="secondary" aria-label="Delete">
                                <DeleteIcon />
                            </IconButton>
                        </Tooltip>
                    </span>
                </Paper>
            )}
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text={`Deseja realmente remover este Provento? (${(removeItem || {}).description})`} />
            <EditEarning editEarning={editEarning}
                onSave={() => { setEditEarning(null); refresh() }}
                onClose={() => setEditEarning(null)} />
            <AddFloatingButton onClick={() => setEditEarning({})} />
        </MainContainer>
    )
}