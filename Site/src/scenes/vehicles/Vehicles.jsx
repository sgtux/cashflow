import React, { useState, useEffect } from 'react'

import { VehicleTable, Container } from './styles'

import { vehicleService } from '../../services'

import {
    IconButton,
    Button,
    Tooltip
} from '@mui/material'

import {
    Delete as DeleteIcon,
    Edit as EditIcon,
    TwoWheeler as MotorcycleIcon,
    LocalGasStation
} from '@mui/icons-material'

import { MainContainer, IconTextInput, ConfirmModal } from '../../components/main'

import { EditVehicleModal } from './EditVehicleModal/EditVehicleModal'

export function Vehicles() {

    const [vehicles, setVehicles] = useState([])
    const [loading, setLoading] = useState(false)

    const [description, setDescription] = useState('')
    const [vehicle, setVehicle] = useState(null)
    const [removeId, setRemoveId] = useState(0)
    const [editVehicle, setEditVehicle] = useState(null)

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
                                <th>Percorrido (KM)</th>
                                <th>Média (KM/L)</th>
                                <th>Ações</th>
                            </tr>
                        </thead>
                        <tbody>
                            {vehicles.map((p, i) =>
                                <tr key={i}>
                                    <td>{p.id}</td>
                                    <td>{p.description}</td>
                                    <td>{p.miliageTraveled}</td>
                                    <td>{p.miliagePerLiter}</td>
                                    <td>
                                        <IconButton onClick={() => setEditVehicle(p)} color="primary" aria-label="Edit">
                                            <Tooltip title="Abastecimentos">
                                                <LocalGasStation />
                                            </Tooltip>
                                        </IconButton>
                                        <IconButton onClick={() => edit(p)} color="primary" aria-label="Edit">
                                            <EditIcon />
                                        </IconButton>
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
                text="Deseja realmente remover este veículo?" />
            <EditVehicleModal vehicle={editVehicle}
                onCancel={() => { setEditVehicle(null); refresh() }} />
        </MainContainer>
    )
}