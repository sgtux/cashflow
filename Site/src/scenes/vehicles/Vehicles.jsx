import React, { useState, useEffect } from 'react'

import { VehicleTable, Container } from './styles'

import { vehicleService } from '../../services'

import {
    IconButton,

} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon
} from '@material-ui/icons'

import { MainContainer } from '../../components/main'

export function Vehicles() {

    const [vehicles, setVehicles] = useState([])
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        setLoading(true)
        vehicleService.getAll()
            .then(res => setVehicles(res))
            .finally(() => setLoading(false))
    }, [])

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
                                        <IconButton onClick={() => { }} color="primary" aria-label="Delete">
                                            <EditIcon />
                                        </IconButton>
                                    </td>
                                    <td>
                                        <IconButton onClick={() => { }} color="secondary" aria-label="Edit">
                                            <DeleteIcon />
                                        </IconButton>
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </VehicleTable>
            </Container>
        </MainContainer>
    )
}