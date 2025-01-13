import React, { useState, useEffect } from 'react'
import {
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    IconButton,
    Tooltip,
    Collapse,
    Card,
    Box
} from '@mui/material'

import { styled } from '@mui/material/styles'

import { KeyboardArrowDown, KeyboardArrowUp } from '@mui/icons-material'
import { tableCellClasses } from '@mui/material/TableCell'

import { Container } from './styles'

import { vehicleService } from '../../services'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    LocalGasStation
} from '@mui/icons-material'

import { MainContainer, ConfirmModal } from '../../components/main'

import { AddFloatingButton } from '../../components'

import { EditVehicleModal } from './VehicleModal/EditVehicleModal'
import { EditFuelExpensesModal } from './VehicleModal/EditFuelExpensesModal'
import { dateToString, toReal, toThousandFormat } from '../../helpers'

const StyledTableRow = styled(TableRow)(() => ({
    '&:nth-of-type(odd)': {
        backgroundColor: '#eee'
    },
    '&:last-child td, &:last-child th': {
        border: 0,
    },
}));

const StyledTableCell = styled(TableCell)(({ theme }) => ({
    [`&.${tableCellClasses.head}`]: {
        backgroundColor: '#999',
        color: theme.palette.common.white,
    },
    [`&.${tableCellClasses.body}`]: {
        fontSize: 14,
    },
}));

export function Vehicles() {

    const [vehicles, setVehicles] = useState([])
    const [loading, setLoading] = useState(false)

    const [removeId, setRemoveId] = useState(0)
    const [editVehicle, setEditVehicle] = useState(null)
    const [editVehicleFuelExpenses, setEditVehicleFuelExpenses] = useState(null)
    const [open, setOpen] = useState({})

    useEffect(() => {
        setLoading(true)
        vehicleService.getAll()
            .then(res => setVehicles(res))
            .finally(() => setLoading(false))
    }, [])

    function refresh() {
        setRemoveId(0)
        setLoading(true)
        vehicleService.getAll()
            .then(res => setVehicles(res))
            .finally(() => setLoading(false))
    }

    function remove() {
        vehicleService.remove(removeId)
            .then(() => refresh())
            .catch(() => setLoading(false))
    }

    function saveVehicle(vehicle) {
        setLoading(true)
        setEditVehicle(null)
        vehicleService.save(vehicle)
            .then(() => refresh())
            .catch(() => setLoading(false))
    }

    return (
        <MainContainer title="Veículos" loading={loading}>
            <Container>
                <TableContainer>
                    <Table sx={{ minWidth: 700 }}>
                        <TableHead>
                            <TableRow>
                                <StyledTableCell style={{ fontWeight: 'bold' }} />
                                <StyledTableCell>Descrição</StyledTableCell>
                                <StyledTableCell align="right">Percorrido (KM)</StyledTableCell>
                                <StyledTableCell align="right">Média (KM/L)</StyledTableCell>
                                <StyledTableCell align="center">Ações</StyledTableCell>
                            </TableRow>
                        </TableHead>

                        <TableBody>
                            {vehicles.map((p, i) =>
                                <React.Fragment key={i}>
                                    <TableRow hover onClick={() => setOpen({ ...open, [i]: !open[i] })} sx={{ cursor: 'pointer' }}>
                                        <StyledTableCell>
                                            <IconButton
                                                aria-label="expand row"
                                                size="small"
                                                onClick={() => setOpen({ ...open, [i]: !open[i] })}>
                                                {open[i] ? <KeyboardArrowUp /> : <KeyboardArrowDown />}
                                            </IconButton>
                                        </StyledTableCell>
                                        <StyledTableCell>{p.description}</StyledTableCell>
                                        <StyledTableCell align="right">{p.miliageTraveled}</StyledTableCell>
                                        <StyledTableCell align="right">{p.miliagePerLiter.toString().replace('.', ',')}</StyledTableCell>
                                        <StyledTableCell align="center" onClick={e => e.stopPropagation()}>
                                            <IconButton onClick={() => setEditVehicleFuelExpenses(p)} color="primary" aria-label="Edit">
                                                <Tooltip title="Abastecimentos">
                                                    <LocalGasStation />
                                                </Tooltip>
                                            </IconButton>
                                            <IconButton onClick={() => setEditVehicle(p)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => setRemoveId(p.id)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
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
                                                                    <StyledTableCell align="right">Quilometragem</StyledTableCell>
                                                                    <StyledTableCell align="right">Preço por Litro</StyledTableCell>
                                                                    <StyledTableCell align="right">Valor Abastecido</StyledTableCell>
                                                                    <StyledTableCell align="right">Litros Abastecidos</StyledTableCell>
                                                                    <StyledTableCell align="center">Data</StyledTableCell>
                                                                </StyledTableRow>
                                                            </TableHead>
                                                            <TableBody>
                                                                {p.fuelExpensesLast10.map(fuelExpense =>
                                                                    <StyledTableRow key={fuelExpense.id} hover>
                                                                        <StyledTableCell align="right">{toThousandFormat(fuelExpense.miliage)} Km</StyledTableCell>
                                                                        <StyledTableCell align="right">{toReal(fuelExpense.pricePerLiter)}</StyledTableCell>
                                                                        <StyledTableCell align="right">{toReal(fuelExpense.valueSupplied)}</StyledTableCell>
                                                                        <StyledTableCell align="right">{fuelExpense.litersSupplied.toString().replace('.', ',')}</StyledTableCell>
                                                                        <StyledTableCell align="center">{dateToString(fuelExpense.date)}</StyledTableCell>
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
            </Container>
            <ConfirmModal show={!!removeId}
                onClose={() => setRemoveId(0)}
                onConfirm={() => remove()}
                text="Deseja realmente remover este veículo?" />
            <EditVehicleModal vehicle={editVehicle}
                onCancel={() => setEditVehicle(null)}
                onSave={vehicle => saveVehicle(vehicle)} />
            <EditFuelExpensesModal vehicle={editVehicleFuelExpenses}
                onCancel={() => setEditVehicleFuelExpenses(null)}
                onSave={vehicle => saveVehicle(vehicle)} />
            <AddFloatingButton onClick={() => setEditVehicle({})} />
        </MainContainer >
    )
}