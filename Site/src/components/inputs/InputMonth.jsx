import React, { useState, useEffect } from 'react'
import {
  Select,
  MenuItem
} from '@mui/material'

import { Months } from '../../helpers/utils'

export function InputMonth({ startYear, onChange, selectedMonth, selectedYear, countYears }) {

  const [years, setYears] = useState([])

  useEffect(() => {

    const currentYear = new Date().getFullYear()
    const temp = startYear && !isNaN(startYear) ? [Number(startYear)] : [currentYear]
    for (let i = 1; i < (countYears || 5); i++)
      temp.push(temp[0] + i)
    setYears(temp)
  }, [])

  return (
    <span>
      <Select
        value={selectedMonth}
        style={{ width: 150 }}
        onChange={e => onChange(e.target.value, selectedYear)}>
        {Months.map((p, i) => <MenuItem key={i} value={i + 1}>{p}</MenuItem>)}
      </Select>
      <Select
        value={selectedYear}
        style={{ width: 100, marginLeft: 10 }}
        onChange={e => onChange(selectedMonth, e.target.value)}>
        {years.map((p, i) => <MenuItem key={i} value={p}>{p}</MenuItem>)}
      </Select>
    </span>
  )
}