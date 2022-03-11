import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'

import {
    IconButton,
    Button,
    Tooltip
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    AddCircle as AddCircleIcon
} from '@material-ui/icons'

import { EarningTable, Container } from './styles'

import { earningService } from '../../services'

import { MainContainer, ConfirmModal } from '../../components/main'
import { toReal, toast, dateToString } from '../../helpers'

export function Earnings() {

    const [earnings, setEarnings] = useState([])
    const [loading, setLoading] = useState(false)
    const [removeItem, setRemoveItem] = useState(null)

    useEffect(() => refresh(), [])

    function refresh() {
        setLoading(true)
        earningService.getAll()
            .then(res => setEarnings(res))
            .finally(() => setLoading(false))
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

    return (
        <MainContainer title="Ganhos/Benefícios" loading={loading}>
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
                text={`Deseja realmente remover este ganho? (${(removeItem || {}).description})`} />
        </MainContainer>
    )
}