import React from 'react'
import { Chip } from '@mui/material'
import CheckCircleRoundedIcon from '@mui/icons-material/CheckCircleRounded'
import ScheduleRoundedIcon from '@mui/icons-material/ScheduleRounded'
import PauseCircleRoundedIcon from '@mui/icons-material/PauseCircleRounded'
import DoNotDisturbOnRoundedIcon from '@mui/icons-material/DoNotDisturbOnRounded'

const variants = {
  done: { color: 'success', icon: <CheckCircleRoundedIcon /> },
  pending: { color: 'warning', icon: <ScheduleRoundedIcon /> },
  inactive: { color: 'default', icon: <PauseCircleRoundedIcon /> },
  exempt: { color: 'default', icon: <DoNotDisturbOnRoundedIcon /> }
}

export function StatusChip({ status, label }) {
  const v = variants[status] || variants.pending
  return <Chip size="small" variant="outlined" color={v.color} icon={v.icon} label={label} />
}
