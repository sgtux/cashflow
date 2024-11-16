import React, { useState, useEffect } from 'react'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'

import {
    Button,
    Dialog,
    DialogContent,
    Zoom,
    IconButton,
    Card,
    TableRow,
    Table,
    TableHead,
    TableBody,
    TableCell,
    CircularProgress,
    TablePagination
} from '@mui/material'

import { tableCellClasses } from '@mui/material/TableCell'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    ArrowLeft as ArrowLeftIcon,
    ArrowRight as ArrowRightIcon
} from '@mui/icons-material'

import { styled } from '@mui/material/styles'

import { toReal, fromReal, dateToString, toThousandFormat } from '../../../helpers'
import { vehicleService } from '../../../services'
import { InputMoney, InputText, DatePickerContainer, DatePickerInput } from '../../../components/inputs'

const MAX_VALUE = 999999999

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

export function EditFuelExpensesModal({ vehicle, onCancel }) {

    const [id, setId] = useState(0)
    const [date, setDate] = useState('')
    const [miliage, setMiliage] = useState('')
    const [pricePerLiter, setPricePerLiter] = useState('')
    const [valueSupplied, setValueSupplied] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [fuelExpenses, setFuelExpenses] = useState([])
    const [loading, setLoading] = useState(false)
    const [rowsPerPage, setRowsPerPage] = useState(5)
    const [page, setPage] = useState(0)
    const [fuelExpensesFiltered, setFuelExpensesFiltered] = useState([])

    useEffect(() => {
        if (vehicle) {
            refresh()
        }
    }, [vehicle])

    useEffect(() => {
        const from = page * rowsPerPage
        const to = rowsPerPage * (page + 1)
        const filtered = fuelExpenses.slice(from, to)
        setFuelExpensesFiltered(filtered)
    }, [page, fuelExpenses, rowsPerPage])

    function refresh() {
        setLoading(true)
        vehicleService.get(vehicle.id)
            .then(res => setFuelExpenses(res.fuelExpenses))
            .finally(() => setLoading(false))
    }

    useEffect(() => {
        if (miliage > MAX_VALUE)
            setMiliage(MAX_VALUE + '')
        if (fromReal(pricePerLiter) > MAX_VALUE)
            setPricePerLiter(MAX_VALUE + '')
        if (fromReal(valueSupplied) > MAX_VALUE)
            setValueSupplied(MAX_VALUE + '')
        setFormIsValid(miliage > 0 && fromReal(pricePerLiter) > 0 && fromReal(valueSupplied) > 0 && date)
    }, [miliage, pricePerLiter, date, valueSupplied])

    function save() {
        const item = {
            id,
            date,
            miliage: Number(miliage),
            pricePerLiter: fromReal(pricePerLiter),
            valueSupplied: fromReal(valueSupplied),
            vehicleId: vehicle.id,
        }
        vehicleService.saveFuelExpense(item)
            .then(() => {
                clear()
                refresh()
            })
    }

    function edit(item) {
        setId(item.id)
        setMiliage(item.miliage + '')
        setPricePerLiter(toReal(item.pricePerLiter))
        setValueSupplied(toReal(item.valueSupplied))
        setDate(new Date(item.date))
    }

    function clear() {
        setMiliage('')
        setPricePerLiter('')
        setValueSupplied('')
        setDate('')
        setId(0)
    }

    function remove(removeId) {
        vehicleService.removeFuelExpense(removeId)
            .then(() => {
                setFuelExpenses(fuelExpenses.filter(p => p.id !== removeId))
                clear()
            })
    }

    function handleChangePage(newValue) {
        setPage(newValue)
    }

    function handleChangeRowsPerPage(newValue) {        
        setRowsPerPage(newValue)
        setPage(0)
    }

    return (
        <Dialog
            open={!!vehicle}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            fullWidth={true}
            maxWidth="lg"
            TransitionComponent={Zoom}>
            <DialogContent>
                <h3 style={{ padding: 10, backgroundColor: '#ccc' }}>{vehicle && vehicle.description} (Gastos em Combustível)</h3>
                {loading ? <div style={{ textAlign: 'center', margin: 50 }}><CircularProgress /></div> :
                    <div>
                        <Table size='small'>
                            <TableHead>
                                <StyledTableRow hover>
                                    <StyledTableCell align='right'>Quilometragem</StyledTableCell>
                                    <StyledTableCell align='right'>Preço por Litro</StyledTableCell>
                                    <StyledTableCell align='right'>Valor Abastecido</StyledTableCell>
                                    <StyledTableCell align='right'>Litros Abastecidos</StyledTableCell>
                                    <StyledTableCell align='center'>Data</StyledTableCell>
                                    <StyledTableCell align='center'>Ações</StyledTableCell>
                                </StyledTableRow>
                            </TableHead>
                            <TableBody>
                                {fuelExpensesFiltered.map((p, i) =>
                                    <StyledTableRow key={i}>
                                        <StyledTableCell align='right'>{toThousandFormat(p.miliage)} Km</StyledTableCell>
                                        <StyledTableCell align='right'>{toReal(p.pricePerLiter)}</StyledTableCell>
                                        <StyledTableCell align='right'>{toReal(p.valueSupplied)}</StyledTableCell>
                                        <StyledTableCell align='right'>{p.litersSupplied.toString().replace('.', ',')}</StyledTableCell>
                                        <StyledTableCell align='center'>{dateToString(p.date)}</StyledTableCell>
                                        <StyledTableCell align='center'>
                                            <IconButton onClick={() => edit(p)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => remove(p.id)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </StyledTableCell>
                                    </StyledTableRow>
                                )}
                            </TableBody>
                        </Table>
                        <TablePagination rowsPerPageOptions={[5, 10]}
                            component="div"
                            count={fuelExpenses.length}
                            rowsPerPage={rowsPerPage}
                            page={page}
                            onPageChange={(e, newPage) => handleChangePage(newPage)}
                            onRowsPerPageChange={e => handleChangeRowsPerPage(e.target.value)} />
                    </div>
                }
                <Card>
                    <DatePickerContainer style={{ padding: 10 }}>
                        Quilometragem:
                        <InputText
                            value={miliage}
                            onChange={e => setMiliage((e.target.value).replace(/[^0-9]/g, ''))}
                        />
                        Preço por Litro: <InputMoney
                            onChangeValue={(event, value, maskedValue) => setPricePerLiter(value)}
                            value={pricePerLiter} />
                        Valor Abastecido: <InputMoney
                            onChangeValue={(event, value, maskedValue) => setValueSupplied(value)}
                            kind="money"
                            value={valueSupplied} />
                        Data: <DatePicker onChange={e => setDate(e)}
                            customInput={<DatePickerInput />}
                            dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
                        <Button onClick={() => clear()} autoFocus>limpar</Button>
                        <Button onClick={() => save()}
                            disabled={!formIsValid}
                            variant="contained"
                            color="primary"
                            autoFocus>salvar</Button>
                    </DatePickerContainer>
                </Card>
                <div style={{ margin: '10px', textAlign: 'center' }}>
                    <Button onClick={() => onCancel()} variant="contained" autoFocus>fechar</Button>
                </div>
            </DialogContent>
        </Dialog>
    )
}