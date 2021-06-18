import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import 'react-datepicker/dist/react-datepicker.css';
import ptBr from 'date-fns/locale/pt-BR';

import {
    Paper,
    List,
    ListItem,
    ListItemSecondaryAction,
    IconButton,
    ListItemText,
    Tooltip,
    Button,
    Divider
} from '@material-ui/core'

import DeleteIcon from '@material-ui/icons/Delete'

import { CardMain, ErrorMessages } from '../../components/main'
import { InputMoney, SalaryEvolution } from '../../components'

import { fromReal, dateToString, toReal } from '../../helpers/utils'
import { salaryService } from '../../services'

const styles = {
    noRecords: {
        textTransform: 'none',
        fontSize: '18px',
        textAlign: 'center'
    },
    divNewCard: {
        textTransform: 'none',
        fontSize: '18px',
        textAlign: 'center',
        marginTop: '20px'
    },
    errorMessage: {
        color: 'red'
    }
}

export default function Salary() {

    const [salaries, setSalaries] = useState([])
    const [errors, setErrors] = useState()
    const [salary, setSalary] = useState()
    const [loading, setLoading] = useState()
    const [startDate, setStartDate] = useState()
    const [endDate, setEndDate] = useState()
    const [value, setValue] = useState()

    useEffect(() => {
        refresh()
    }, [])

    useEffect(() => {
        if ((salary || {}).id) {
            setStartDate(new Date(salary.startDate))
            setEndDate(new Date(salary.endDate))
            setValue(toReal(salary.value))
        } else {
            setStartDate()
            setEndDate()
            setValue()
        }
    }, [salary])

    function saveSalary() {
        setLoading(true)
        setErrors()
        salaryService.save({ id: salary.id, value: fromReal(value), startDate, endDate })
            .then(() => refresh())
            .catch(err => {
                setLoading(false)
                console.log(err)
                setErrors(err.messages)
            })
    }

    function refresh() {
        setLoading(true)
        setSalary(null)
        salaryService.get()
            .then(res => setSalaries(res.data))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function removeSalary(id) {
        setLoading(true)
        salaryService.remove(id)
            .then(() => refresh())
            .catch(err => {
                setLoading(false)
                console.log(err)
            })
    }

    return (
        <CardMain title="Salários" loading={loading}>
            {salaries.length > 0 ?
                <Paper>
                    <List dense={true}>
                        {salaries.map(p =>
                            <ListItem button key={p.id}
                                onClick={() => setSalary(p)}>
                                <ListItemText
                                    primary={p.id}
                                    secondary=""
                                />
                                <ListItemText
                                    primary={dateToString(p.startDate)}
                                    secondary=""
                                />
                                <ListItemText
                                    primary={p.endDate ? dateToString(p.endDate) : 'VIGENTE'}
                                    secondary=""
                                />
                                <ListItemText
                                    primary={toReal(p.value)}
                                    secondary=""
                                />
                                <ListItemSecondaryAction>
                                    <Tooltip title="Remover este salário">
                                        <IconButton color="secondary" aria-label="Delete"
                                            onClick={() => removeSalary(p.id)}>
                                            <DeleteIcon />
                                        </IconButton>
                                    </Tooltip>
                                </ListItemSecondaryAction>
                            </ListItem>
                        )}
                    </List>
                </Paper>
                :
                <div style={styles.noRecords}>
                    <span>Nenhum salário cadastrado.</span>
                </div>
            }

            <div style={styles.divNewCard}>
                <Divider />
                <div style={{ marginTop: '20px' }} hidden={salary}>
                    <Button variant="text" color="primary" onClick={() => setSalary({})}>Adicionar Salário</Button>
                </div>
                <div style={{ marginTop: '20px' }} hidden={!salary}>
                    Data Início: <DatePicker onChange={e => setStartDate(e)} dateFormat="dd/MM/yyyy" locale={ptBr} selected={startDate} /><br />
                    Data Fim: <DatePicker onChange={e => setEndDate(e)} dateFormat="dd/MM/yyyy" locale={ptBr} selected={endDate} /><br />
                    Valor: <InputMoney
                        onChangeText={e => setValue(e)}
                        kind="money"
                        value={value} />
                    <div style={{ marginTop: '20px' }}>
                        <Button color="primary" onClick={() => setSalary(null)}>Cancelar</Button>
                        <Button variant="contained" color="primary"
                            onClick={() => saveSalary()}>Salvar</Button>
                    </div>
                    <ErrorMessages errors={errors} />
                </div>
                <br />
                <Divider />
                <SalaryEvolution salaries={salaries} />
            </div>
        </CardMain>
    )
}