import React, { useState, useEffect } from 'react'

import { VehicleTable, Container } from './styles'

import { vehicleService } from '../../services'

import {
    IconButton,
    Button,
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    Motorcycle as MotorcycleIcon
} from '@material-ui/icons'

import { MainContainer, IconTextInput, ConfirmModal } from '../../components/main'

export function Vehicles() {

    const [vehicles, setVehicles] = useState([])
    const [loading, setLoading] = useState(false)

    const [description, setDescription] = useState('')
    const [vehicle, setVehicle] = useState(null)
    const [removeId, setRemoveId] = useState(0)

    useEffect(() => {
        setLoading(true)
        vehicleService.getAll()
            .then(res => setVehicles(res))
            .finally(() => setLoading(false))
    }, [])

    function refresh() {
        setVehicle(null)
        setDescription('')
        setRemoveId(0)
        setLoading(true)
        vehicleService.getAll()
            .then(res => setVehicles(res))
            .finally(() => setLoading(false))
    }

    function cancel() {
        setDescription('')
        setVehicle(null)
    }

    function save() {
        setLoading(true)
        vehicleService.save({ id: vehicle.id, description })
            .then(() => refresh())
            .catch(() => setLoading(false))
    }

    function remove() {
        vehicleService.remove(removeId)
            .then(() => refresh())
            .catch(() => setLoading(false))
    }

    function edit(item) {
        setVehicle(item)
        setDescription(item.description)
    }

    return (
        <MainContainer title="Veículos" loading={loading}>
            <Container>
                <VehicleTable>
                    <table>
                        <thead>
                            <tr>
                                <th>Id</th>
                                <th>Descrição</th>
                            </tr>
                        </thead>
                        <tbody>
                            {vehicles.map((p, i) =>
                                <tr key={i}>
                                    <td>{p.id}</td>
                                    <td>{p.description}</td>
                                    <td>
                                        <IconButton onClick={() => edit(p)} color="primary" aria-label="Edit">
                                            <EditIcon />
                                        </IconButton>
                                    </td>
                                    <td>
                                        <IconButton onClick={() => setRemoveId(p.id)} color="secondary" aria-label="Delete">
                                            <DeleteIcon />
                                        </IconButton>
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </VehicleTable>
                <Button variant="text" color="primary" onClick={() => setVehicle({})}>
                    Adicionar Veículo
                </Button>
                <div style={{ marginTop: '20px' }} hidden={vehicle === null}>
                    <IconTextInput
                        label="Descrição do veículo"
                        value={description}
                        onChange={e => setDescription(e.value)}
                        Icon={<MotorcycleIcon />}
                    />
                    <br />
                    <div style={{ marginTop: '20px' }}>
                        <Button color="primary" onClick={() => cancel()}>
                            Cancelar
                        </Button>
                        <Button variant="contained" color="primary"
                            onClick={() => save()}>
                            Salvar
                        </Button>
                    </div>
                </div>
            </Container>
            <ConfirmModal show={!!removeId}
                onClose={() => setRemoveId(0)}
                onConfirm={() => remove()}
                text="Desejá realmente remover este veículo?" />
        </MainContainer>
    )
}