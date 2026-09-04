import React, { useState, useEffect } from 'react'
import { styled } from '@mui/material/styles'
import { Link } from 'react-router-dom'

import { Container } from './styles'
import { KeyboardArrowDown, KeyboardArrowUp } from '@mui/icons-material'
import { tableCellClasses } from '@mui/material/TableCell'

import { recurringEarningService } from '../../services'

import {
    IconButton,
    Tooltip,
    Checkbox,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    Collapse,
    Box,
    Card
} from '@mui/material'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    History as HistoryIcon
} from '@mui/icons-material'

import { MainContainer, ConfirmModal } from '../../components/main'
import { AddFloatingButton, TableActionPayButton } from '../../components'
import { dateToString, toReal, toast } from '../../helpers'
import { RecurringEarningHistoryModal } from './RecurringEarningHistoryModal/RecurringEarningHistoryModal'


const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
        backgroundColor: theme.palette.action.hover
    },
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}))

const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        backgroundColor: '#999',
        color: theme.palette.common.white,
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
    },
}))

export function RecurringEarnings() {

    const [recurringEarnings, setRecurringEarnings] = useState([])
    const [loading, setLoading] = useState(false)
    const [recurringEarningEditHistory, setRecurringEarningEditHistory] = useState(null)
    const [removeItem, setRemoveItem] = useState(null)
    const [showInactives, setShowInactives] = useState(false)
    const [open, setOpen] = useState({})

    useEffect(() => refresh(), [showInactives])

    function refresh() {
        setRecurringEarningEditHistory(null)
        setLoading(true)
        recurringEarningService.getAll(showInactives)
            .then(res => setRecurringEarnings(res))
            .finally(() => setLoading(false))
    }

    function receive(earning) {
        setLoading(true)
        recurringEarningService.saveHistory({
            value: earning.value,
            date: new Date(),
            recurringEarningId: earning.id
        })
            .then(res => refresh())
            .finally(() => setLoading(false))
    }

    function refreshEditHistory() {
        recurringEarningService.get(recurringEarningEditHistory.id)
            .then(res => {
                setRecurringEarningEditHistory(res)
            })
            .catch(err => console.log(err))
    }

    function remove() {
        setLoading(true)
        setRecurringEarningEditHistory(null)
        recurringEarningService.remove(removeItem.id)
            .then(() => toast.success('Removido com sucesso!'))
            .catch(err => setRemoveItem(null))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Ganhos Recorrentes" loading={loading}>
            <Container>
                <TableContainer>
                    <Table sx={{ minWidth: 700 }}>
                        <TableHead>
                            <TableRow>
                                <StyledTableCell style={{ fontWeight: 'bold' }} />
                                <StyledTableCell>Descrição</StyledTableCell>
                                <StyledTableCell align="right">Valor Base</StyledTableCell>
                                <StyledTableCell align="center">Situação</StyledTableCell>
                                <StyledTableCell style={{ fontWeight: 'bold' }} />
                                <StyledTableCell align="center">Ações</StyledTableCell>
                            </TableRow>
                        </TableHead>

                        <TableBody>
                            {recurringEarnings.map((p, i) =>
                                <React.Fragment key={i}>
                                    <TableRow sx={{ cursor: 'pointer' }}>
                                        <StyledTableCell>
                                            <IconButton
                                                aria-label="expand row"
                                                size="small"
                                                onClick={() => setOpen({ ...open, [i]: !open[i] })}>
                                                {open[i] ? <KeyboardArrowUp /> : <KeyboardArrowDown />}
                                            </IconButton>
                                        </StyledTableCell>

                                        <StyledTableCell>{p.description}</StyledTableCell>
                                        <StyledTableCell align="right">{toReal(p.value)}</StyledTableCell>
                                        <StyledTableCell align="center">{p.inactiveAt ? 'Inativo' : p.received ? 'Recebido' : 'Pendente'}</StyledTableCell>
                                        <StyledTableCell align="center">
                                            {!p.inactiveAt && !p.received && <TableActionPayButton onClick={() => receive(p)}>Receber</TableActionPayButton>}
                                        </StyledTableCell>
                                        <StyledTableCell align="center" onClick={e => e.stopPropagation()}>
                                            <Tooltip title="Histórico de Recebimentos">
                                                <IconButton onClick={() => setRecurringEarningEditHistory(p)} color="primary" aria-label="History">
                                                    <HistoryIcon />
                                                </IconButton>
                                            </Tooltip>
                                            <Tooltip title="Editar">
                                                <Link to={`/edit-recurring-earnings/${p.id}`}>
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
                                        </StyledTableCell>
                                    </TableRow>
                                    <TableRow>
                                        <StyledTableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
                                            <Collapse in={open[i]} timeout="auto">
                                                <Box sx={{ margin: 1 }}>
                                                    <Card>
                                                        <Table size='small'>
                                                            <TableHead>
                                                                <StyledTableRow>
                                                                    <StyledTableCell align="center">Valor Recebido</StyledTableCell>
                                                                    <StyledTableCell align="center">Data</StyledTableCell>
                                                                </StyledTableRow>
                                                            </TableHead>
                                                            <TableBody>
                                                                {p.history.slice(0, 10).map(h =>
                                                                    <StyledTableRow key={h.id} hover>
                                                                        <StyledTableCell align="center">{toReal(h.value)}</StyledTableCell>
                                                                        <StyledTableCell align="center">{dateToString(h.date)}</StyledTableCell>
                                                                    </StyledTableRow>
                                                                )}
                                                            </TableBody>
                                                        </Table>
                                                    </Card>
                                                </Box>
                                            </Collapse>
                                        </StyledTableCell>
                                    </TableRow>
                                </React.Fragment>
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
                <div style={{ textAlign: 'center', marginTop: 10, fontSize: 16 }}>
                    <span>Exibir Inativos:</span>
                    <Checkbox checked={showInactives} onChange={p => { setShowInactives(p.target.checked); refresh(p.target.checked) }} />
                </div>
            </Container>
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text={`Deseja realmente remover este ganho recorrente? (${(removeItem || {}).description || ''})`} />
            <RecurringEarningHistoryModal
                recurringEarning={recurringEarningEditHistory}
                show={!!recurringEarningEditHistory}
                requestRefresh={() => refreshEditHistory()}
                onCancel={() => refresh()} />
            <Link to="/edit-recurring-earnings/0">
                <AddFloatingButton />
            </Link>
        </MainContainer >
    )
}
