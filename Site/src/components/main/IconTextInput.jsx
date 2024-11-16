import React, { useState, useEffect } from 'react'

import {
	TextField,
	InputAdornment,
	IconButton,
	FormControl,
	FormHelperText
} from '@mui/material'

const emailRegex = /^[a-zA-Z0-9!#$.%&*()]{3,20}@[a-zA-Z0-9]{1,20}[.][a-zA-Z0-9]{1,20}([.][a-zA-Z0-9]{1,20})?$/

export default function IconTextInput(props) {

	const [hasError, setHasError] = useState(false)
	const [errorMessage, setErrorMessage] = useState('Este campo é obrigatório.')
	const [lostFocus, setLostFocus] = useState(false)
	const [text, setText] = useState('')

	useEffect(() => {
		setHasError(props.required)
		setText(props.defaultValue || '')
	}, [])

	useEffect(() => setText(props.value), [props.value])

	function onChange(t) {
		let hasError = false
		let errorMessage = ''

		if (props.required && !t.trim()) {
			hasError = true
			errorMessage = 'Este campo é obrigatório.'
		} else if (props.minlength && t.length < props.minlength) {
			hasError = true
			errorMessage = `É preciso informar pelo menos ${props.minlength} caracteres.`
		} else if (props.maxlength && t.length > props.maxlength) {
			hasError = true
			errorMessage = `Este campo excedeu o limite de ${props.maxlength} caracteres.`
		} else if (props.email && !emailRegex.test(t)) {
			hasError = true
			errorMessage = 'Entre com um email válido.'
		} else if (props.pattern && !RegExp(props.pattern).test(t)) {
			hasError = true
			errorMessage = props.patternMessage ? props.patternMessage : 'Verifique o valor deste campo.'
		}

		setText(t)
		setHasError(hasError)
		setErrorMessage(errorMessage)

		if (props.onChange)
			props.onChange({ valid: !hasError, value: t, name: props.name })
	}

	return (
		<FormControl style={props.style}>
			<TextField error={hasError && lostFocus}
				disabled={props.disabled}
				style={{ marginTop: (props.style || {}).marginTop || '10px' }}
				value={text}
				multiline={props.multiline}
				rows={props.rows}
				variant={props.variant || 'standard'}
				className="teste"
				label={props.label}
				type={props.type || 'text'}
				onChange={e => onChange(e.target.value)}
				onBlur={() => setLostFocus(true)}
				InputProps={{
					endAdornment: (
						<InputAdornment position="end">
							<IconButton
								disabled={props.disabled}
								tabIndex={99}
								onClick={props.iconClick}>
								{props.Icon}
							</IconButton>
						</InputAdornment>
					)
				}}
			/>
			<FormHelperText hidden={!lostFocus || !hasError} error={hasError}>
				{errorMessage}
			</FormHelperText>
		</FormControl>
	)
}