import React, { useEffect, useState } from 'react'

import {
    Button,
    Checkbox,
    Dialog,
    DialogContent,
    Zoom,
} from '@mui/material'

import {
    TwoWheeler as MotorcycleIcon,
} from '@mui/icons-material'

import { IconTextInput } from '../../../components/main'

export function EditVehicleModal({ vehicle, onSave, onCancel }) {

    const [description, setDescription] = useState('')
    const [active, setActive] = useState(true)

    useEffect(() => {
        if (vehicle) {
            setDescription(vehicle.description)
            setActive(vehicle.active)
        } else {
            setDescription('')
            setActive(true)
        }
    }, [vehicle])

    function save() {
        onSave({ id: vehicle.id, description, active })
    }

    return (
        <Dialog
            open={!!vehicle}
            onClose={onCancel}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"
            transitionDuration={250}
            TransitionComponent={Zoom}>
            <DialogContent>
                <div hidden={vehicle === null}>
                    <IconTextInput
                        style={{ marginTop: 0 }}
                        label="Descrição do veículo"
                        value={description}
                        onChange={e => setDescription(e.value)}
                        Icon={<MotorcycleIcon />}
                    />
                    <div style={{ textAlign: 'center', marginTop: 10 }}>
                        <span>Ativo:</span>
                        <Checkbox checked={active} onChange={p => setActive(p.target.checked)} />
                    </div>
                    <div style={{ marginTop: '20px', display: 'flex', justifyContent: 'space-evenly' }}>
                        <Button color="primary" onClick={() => onCancel()}>
                            Cancelar
                        </Button>
                        <Button variant="contained" color="primary"
                            onClick={() => save()}>
                            Salvar
                        </Button>
                    </div>
                </div>
            </DialogContent>
        </Dialog>
    )
}