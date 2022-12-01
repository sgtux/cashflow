import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

import {
    IconButton,
    Tooltip,
    Paper, Select,
    MenuItem
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    AddCircle as AddCircleIcon,
    FileCopy as CopyIcon
} from '@material-ui/icons'

import { EarningTable, Container } from './styles'

import { earningService } from '../../services'

import { MainContainer, ConfirmModal } from '../../components/main'
import { toReal, toast, dateToString, isSameMonth } from '../../helpers'

export function Earnings() {

    const [earnings, setEarnings] = useState([])
    const [loading, setLoading] = useState(false)
    const [removeItem, setRemoveItem] = useState(null)
    const [selectedMonthYear, setSelectedMonthYear] = useState('')
    const [monthYearList, setMonthYearList] = useState([])

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

    useEffect(() => refresh(), [selectedMonthYear])

    function refresh() {
        if (selectedMonthYear) {
            const temp = selectedMonthYear.split('/')
            const fromDate = new Date(`${temp[0]}/1/${temp[1]}`)
            setLoading(true)
            earningService.getAll({ fromDate })
                .then(res => setEarnings(res))
                .finally(() => setLoading(false))
        }
    }

    function remove() {
        setLoading(true)
        earningService.remove(removeItem.id)
            .then(() => {
                toast.success('Removido com sucesso!')
                setRemoveItem(null)
                refresh()
            })
            .catch(err => {
                console.log(err)
                setLoading(false)
            })
    }

    function copyEarning(earning) {
        let date = new Date(earning.date)
        date = new Date(date.getFullYear(), new Date().getMonth(), date.getDate())
        earning = { ...earning, id: 0, date }
        setLoading(true)
        earningService.save(earning)
            .then(() => {
                toast.success('Salvo com sucesso.')
                refresh()
            })
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Ganhos/Benefícios" loading={loading}>

            <Paper>
                <span style={{ margin: 10, fontSize: 16, color: '#666' }}>Desde: </span>
                <Select
                    value={selectedMonthYear}
                    style={{ width: '130px' }}
                    onChange={e => setSelectedMonthYear(e.target.value)}>
                    {monthYearList.map((p, i) => <MenuItem key={i} value={p}>{p}</MenuItem>)}
                </Select>
            </Paper>

            <Container>
                <EarningTable>
                    <table>
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Descrição</th>
                                <th>Valor</th>
                                <th>Data</th>
                                <th>Ações</th>
                            </tr>
                        </thead>
                        <tbody>
                            {earnings.map((p, i) =>
                                <tr key={i}>
                                    <td>{p.id}</td>
                                    <td>{p.description} ({p.typeDescription})</td>
                                    <td>{toReal(p.value)}</td>
                                    <td>{dateToString(p.date)}</td>
                                    <td>
                                        {!isSameMonth(new Date(), p.date) &&
                                            <Tooltip title="Copiar para o mês atual">
                                                <IconButton onClick={() => copyEarning(p)} color="primary" aria-label="Edit">
                                                    <CopyIcon />
                                                </IconButton>
                                            </Tooltip>
                                        }
                                        <Tooltip title="Editar">
                                            <Link to={`/edit-earning/${p.id}`}>
                                                <IconButton color="primary" aria-label="Edit">
                                                    <EditIcon />
                                                </IconButton>
                                            </Link>
                                        </Tooltip>
                                        <Tooltip title="Remover">
                                            <IconButton onClick={() => setRemoveItem(p)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </Tooltip>
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </EarningTable>
                <Link to="/edit-earning/0">
                    <IconButton variant="contained" color="primary">
                        <AddCircleIcon />
                    </IconButton>
                </Link>
            </Container>
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text={`Deseja realmente remover este benefício? (${(removeItem || {}).description})`} />
        </MainContainer>
    )
}