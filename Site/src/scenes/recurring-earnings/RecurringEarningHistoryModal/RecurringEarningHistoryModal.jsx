import React, { useState, useEffect } from 'react'
import ptBr from 'date-fns/locale/pt-BR'
import DatePicker from 'react-datepicker'
import { styled } from '@mui/material/styles'

import {
    Button,
    Dialog,
    DialogContent,
    Zoom,
    IconButton,
    Card,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    TablePagination
} from '@mui/material'

import {
    Delete as DeleteIcon,
    Edit as EditIcon
} from '@mui/icons-material'

import { tableCellClasses } from '@mui/material/TableCell'

import { toReal, fromReal, dateToString, toast } from '../../../helpers'
import { InputMoney, DatePickerContainer, DatePickerInput } from '../../../components/inputs'
import { recurringEarningService } from '../../../services'
import { ConfirmModal } from '../../../components/main'

const StyledTableRow = styled(TableRow)(({ theme }) => ({
    '&:nth-of-type(odd)': {
        backgroundColor: theme.palette.action.hover
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

export function RecurringEarningHistoryModal({ recurringEarning, onCancel, show, requestRefresh }) {

    const [id, setId] = useState(0)
    const [date, setDate] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [removeItem, setRemoveItem] = useState(null)
    const [rowsPerPage, setRowsPerPage] = useState(5)
    const [page, setPage] = useState(0)
    const [historyFiltered, setHistoryFiltered] = useState([])

    const [history, setHistory] = useState([])

    useEffect(() => {
        if ((recurringEarning || {}).history)
            setHistory(recurringEarning.history)
    }, [recurringEarning])

    useEffect(() => {
        const from = page * rowsPerPage
        const to = rowsPerPage * (page + 1)
        const filtered = history.slice(from, to)
        setHistoryFiltered(filtered)
    }, [page, history, rowsPerPage])

    useEffect(() => {
        setFormIsValid(date && fromReal(value) > 0)
    }, [date, value])

    function clear() {
        setDate('')
        setValue('')
        setId(0)
    }

    function save() {
        recurringEarningService.saveHistory({
            id: id,
            value: fromReal(value),
            date: date,
            recurringEarningId: recurringEarning.id
        }).then(() => {
            requestRefresh()
            toast.success('Salvo com sucesso!')
            clear()
        })
    }

    function edit(item) {
        setValue(toReal(item.value))
        setDate(new Date(item.date))
        setId(item.id)
    }

    function remove() {
        recurringEarningService.removeHistory(removeItem.recurringEarningId, removeItem.id)
            .then(() => {
                requestRefresh()
                toast.success('Removido com sucesso!')
                setRemoveItem(null)
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
            open={show}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            maxWidth="lg"
            TransitionComponent={Zoom}>
            <DialogContent>
                <div style={{ fontFamily: 'GraphikRegular', minWidth: 700 }}>
                    <div>
                        <Table size='small'>
                            <TableHead>
                                <StyledTableRow>
                                    <StyledTableCell align="center">Id</StyledTableCell>
                                    <StyledTableCell align="center">Valor Recebido</StyledTableCell>
                                    <StyledTableCell align="center">Data</StyledTableCell>
                                    <StyledTableCell align="center">Ações</StyledTableCell>
                                </StyledTableRow>
                            </TableHead>
                            <TableBody>
                                {historyFiltered.map(h =>
                                    <StyledTableRow key={h.id} hover>
                                        <StyledTableCell align="center">{h.id}</StyledTableCell>
                                        <StyledTableCell align="center">{toReal(h.value)}</StyledTableCell>
                                        <StyledTableCell align="center">{dateToString(h.date)}</StyledTableCell>
                                        <StyledTableCell align="center">
                                            <IconButton onClick={() => edit(h)} color="primary" aria-label="Edit">
                                                <EditIcon />
                                            </IconButton>
                                            <IconButton onClick={() => setRemoveItem(h)} color="secondary" aria-label="Delete">
                                                <DeleteIcon />
                                            </IconButton>
                                        </StyledTableCell>
                                    </StyledTableRow>
                                )}
                            </TableBody>

                        </Table>
                        <TablePagination rowsPerPageOptions={[5, 10]}
                            component="div"
                            count={history.length}
                            rowsPerPage={rowsPerPage}
                            page={page}
                            onPageChange={(e, newPage) => handleChangePage(newPage)}
                            onRowsPerPageChange={e => handleChangeRowsPerPage(e.target.value)} />
                    </div>
                    <Card>
                        <DatePickerContainer style={{ padding: 10 }}>
                            Valor Recebido:
                            <InputMoney
                                onChangeValue={(event, value, maskedValue) => setValue(value)}
                                value={value} />
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
                </div>
            </DialogContent>
            <ConfirmModal show={!!removeItem}
                onClose={() => setRemoveItem(null)}
                onConfirm={() => remove()}
                text="Deseja realmente remover este histórico?" />
        </Dialog>
    )
}
