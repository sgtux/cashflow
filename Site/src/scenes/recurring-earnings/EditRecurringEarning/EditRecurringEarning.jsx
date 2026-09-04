import React, { useState, useEffect } from 'react'
import { useParams, Link } from 'react-router-dom'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import { Button } from '@mui/material'

import { InputMoney, DatePickerInput, DatePickerContainer } from '../../../components/inputs'

import { MainContainer, IconTextInput } from '../../../components/main'

import { recurringEarningService } from '../../../services'
import { toast, fromReal, toReal } from '../../../helpers'
import { RecurringEarningHistoryModal } from '../RecurringEarningHistoryModal/RecurringEarningHistoryModal'

export function EditRecurringEarning() {

    const [id, setId] = useState(0)
    const [description, setDescription] = useState('')
    const [value, setValue] = useState('')
    const [formIsValid, setFormIsValid] = useState(false)
    const [loading, setLoading] = useState(false)
    const [recurringEarning, setRecurringEarning] = useState(null)
    const [showModal, setShowModal] = useState(false)
    const [inactiveAt, setInactiveAt] = useState()

    const params = useParams()

    useEffect(() => refresh(), [])

    function refresh() {
        if (params.id > 0) {
            setLoading(true)
            setId(params.id)
            recurringEarningService.get(params.id)
                .then(res => {
                    if (res) {
                        setDescription(res.description)
                        setValue(toReal(res.value))
                        setRecurringEarning(res)
                        if (res.inactiveAt)
                            setInactiveAt(new Date(res.inactiveAt))
                    }
                })
                .catch(err => console.log(err))
                .finally(() => setLoading(false))
        }
    }

    useEffect(() => {
        setFormIsValid(description && fromReal(value) > 0)
    }, [description, value])

    function save() {
        setLoading(true)
        recurringEarningService.save({
            id,
            description,
            value: fromReal(value),
            inactiveAt
        }).then(() => toast.success('Salvo com sucesso.'))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    return (
        <MainContainer title="Ganho Recorrente" loading={loading}>
            <div style={{ fontFamily: 'GraphikRegular', fontSize: 14, color: 'var(--mui-palette-text-secondary)' }}>
                <IconTextInput
                    label="Descrição"
                    value={description}
                    onChange={e => setDescription(e.value)}
                />
                <br />
                <br />
                Valor: <InputMoney
                    onChangeValue={(event, value, maskedValue) => setValue(value)}
                    value={value} />
                <br />
                {
                    !!id && <div style={{ marginBottom: 10 }}>
                        <DatePickerContainer style={{ color: 'var(--mui-palette-text-secondary)' }}>
                            <span>Data Inativação:</span>
                            <DatePicker customInput={<DatePickerInput style={{ width: 115 }} />} onChange={e => setInactiveAt(e)}
                                dateFormat="dd/MM/yyyy" locale={ptBr} selected={inactiveAt} />
                        </DatePickerContainer>
                    </div>
                }
                <div style={{ margin: '10px', textAlign: 'right' }}>
                    {
                        !!id &&
                        <Button onClick={() => setShowModal(true)}
                            color="primary"
                            style={{ marginLeft: 20, marginRight: 20 }}
                            autoFocus>Histórico</Button>
                    }
                    <Link to="/recurring-earnings">
                        <Button variant="contained" autoFocus>lista de ganhos</Button>
                    </Link>
                    <Button onClick={() => save()}
                        disabled={!formIsValid}
                        variant="contained"
                        color="primary"
                        style={{ marginLeft: 20, marginRight: 20 }}
                        autoFocus>salvar</Button>
                </div>
                <RecurringEarningHistoryModal
                    recurringEarning={recurringEarning}
                    show={showModal}
                    requestRefresh={() => refresh()}
                    onCancel={() => setShowModal(false)} />
            </div>
        </MainContainer >
    )
}
