import React, { useState, useEffect } from 'react'
import {
  InputLabel,
  Select,
  MenuItem
} from '@material-ui/core'

import { Months } from '../../helpers/utils'

export function InputMonth({ startYear, onChange, selectedMonth, selectedYear, label }) {

  const [years, setYears] = useState([])

  useEffect(() => {

    const currentYear = new Date().getFullYear()
    const temp = startYear && !isNaN(startYear) ? [Number(startYear)] : [currentYear]
    for (let i = 1; i < 5; i++)
      temp.push(temp[0] + i)
    setYears(temp)
  }, [])

  return (
    <span>
      <div>
        <InputLabel style={{ height: '8px', fontSize: '10px' }}>{label}</InputLabel>
      </div>
      <Select
        value={selectedMonth}
        style={{ width: '130px' }}
        onChange={e => onChange(e.target.value, selectedYear)}>
        {Months.map((p, i) => <MenuItem key={i} value={i + 1}>{p}</MenuItem>)}
      </Select>
      <Select
        value={selectedYear}
        style={{ width: '80px', marginLeft: '10px' }}
        onChange={e => onChange(selectedMonth, e.target.value)}>
        {years.map((p, i) => <MenuItem key={i} value={p}>{p}</MenuItem>)}
      </Select>
    </span>
  )
}