import React from 'react'

export function ErrorMessages({ errors }) {
    if (!(errors || []).length)
        return null
    return (
        <ul>
            {errors.map((p, i) => <li key={i} style={{ color: '#d55', marginTop: '10px', fontSize: 14 }}>{p}</li>)}
        </ul>
    )
}