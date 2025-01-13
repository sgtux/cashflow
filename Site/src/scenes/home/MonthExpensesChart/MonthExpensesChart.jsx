import React, { useState, useEffect } from 'react'
import { PieChart, Pie, Cell } from 'recharts'

import { toReal } from '../../../helpers'

const COLORS = [
    '#ff6b6b',
    '#FF8042',
    '#FFBB28',
    '#0aceff',
    '#0088FE',
    '#179c34',
    '#196b1d',
    '#00c42a'
]

const RADIAN = Math.PI / 180;
const renderCustomizedLabel = ({
    cx,
    cy,
    midAngle,
    innerRadius,
    outerRadius,
    percent,
    index
}) => {
    const radius = innerRadius + (outerRadius - innerRadius) * 0.5;
    const x = cx + radius * Math.cos(-midAngle * RADIAN);
    const y = cy + radius * Math.sin(-midAngle * RADIAN);

    return (
        <text
            x={x}
            y={y}
            style={{ fontSize: 16, fontFamily: 'GraphikRegular' }}
            fill="white"
            textAnchor={x > cx ? "start" : "end"}
            dominantBaseline="central"
        >
            {`${(percent * 100).toFixed(0)}%`}
        </text>
    );
}

function Legend({ item }) {
    return (
        <div style={{ color: COLORS[item.index], fontSize: 14, fontFamily: 'GraphikRegular' }}>
            <span>{item.description}: </span>
            <span style={{ fontWeight: 'bold' }}>{toReal(item.value)}</span>
        </div>
    )
}

export function MonthExpensesChart({ data }) {

    const [total, setTotal] = useState(0)

    useEffect(() => {
        let sum = 0
        data.forEach(e => sum += e.value)
        setTotal(sum)
    }, [data])

    return (
        <div style={{ marginTop: 20 }}>
            {data.map((p, i) => <Legend key={i} item={p} />)}
            <span style={{ fontSize: 20, marginTop: 10 }}>Total: {toReal(total)}</span>
            <PieChart width={300} height={300}>
                <Pie
                    data={data}
                    labelLine={false}
                    label={renderCustomizedLabel}
                    outerRadius={120}
                    fill="#8884d8"
                    width="600"
                    dataKey="value"
                >
                    {data.map(p => <Cell key={`cell-${p.index}`} fill={COLORS[p.index]} />)}
                </Pie>
            </PieChart>
        </div>
    );
}
