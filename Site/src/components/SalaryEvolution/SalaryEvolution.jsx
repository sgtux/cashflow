import React, { useState, useEffect } from 'react';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer } from 'recharts';

import { dateToString } from '../../helpers'

export function SalaryEvolution({ salaries }) {

    const [source, setSource] = useState([])

    useEffect(() => {
        if ((salaries || []).length) {
            const temp = salaries.map(p => ({ date: dateToString(p.startDate), value: p.value }))
            setSource(temp.concat([{ date: dateToString(new Date()), value: salaries[salaries.length - 1].value }]))
        }
    }, [])

    return (
        <div>
            <LineChart
                width={800}
                height={300}
                data={source}
                style={{ fontSize: 12 }}
                margin={{
                    top: 5,
                    right: 30,
                    left: 20,
                    bottom: 5,
                }}
            >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis dataKey="date" />
                <YAxis />
                <Tooltip />
                <Legend />
                <Line type="monotone" name="Salário" width="20" dataKey="value" stroke="#4b9372" activeDot={{ r: 8 }} />
            </LineChart>
        </div>
    )
}