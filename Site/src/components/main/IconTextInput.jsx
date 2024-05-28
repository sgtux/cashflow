import React from 'react'
import {
  TextField,
  InputAdornment,
  IconButton,
  FormControl,
  FormHelperText
} from '@mui/material'

const emailRegex = /^[a-zA-Z0-9!#$.%&*()]{3,20}@[a-zA-Z0-9]{1,20}[.][a-zA-Z0-9]{1,20}([.][a-zA-Z0-9]{1,20})?$/

class IconTextInput extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      hasError: props.required && !props.defaultValue,
      errorMessage: 'Este campo é obrigatório.',
      lostFocus: false,
      text: props.defaultValue || ''
    }
  }

  componentDidMount() {
    this.onChange(this.props.value || this.state.text)
  }

  onChange(t) {
    let hasError = false
    let errorMessage = ''

    if (this.props.required && !t.trim()) {
      hasError = true
      errorMessage = 'Este campo é obrigatório.'
    } else if (this.props.minlength && t.length < this.props.minlength) {
      hasError = true
      errorMessage = `É preciso informar pelo menos ${this.props.minlength} caracteres.`
    } else if (this.props.maxlength && t.length > this.props.maxlength) {
      hasError = true
      errorMessage = `Este campo excedeu o limite de ${this.props.maxlength} caracteres.`
    } else if (this.props.email && !emailRegex.test(t)) {
      hasError = true
      errorMessage = 'Entre com um email válido.'
    } else if (this.props.pattern && !RegExp(this.props.pattern).test(t)) {
      hasError = true
      errorMessage = this.props.patternMessage ? this.props.patternMessage : 'Verifique o valor deste campo.'
    }

    this.setState({
      text: t,
      hasError: hasError,
      errorMessage: errorMessage
    })
    if (this.props.onChange)
      this.props.onChange({ valid: !hasError, value: t, name: this.props.name })
  }

  componentDidUpdate() {
    if ((this.props.value || this.props.value === '') && this.state.text !== this.props.value)
      this.onChange(this.props.value)
  }

  render() {
    return (
      <FormControl style={this.props.style}>
        <TextField error={this.state.hasError && this.state.lostFocus}
          disabled={this.props.disabled}
          style={{ marginTop: (this.props.style || {}).marginTop || '10px' }}
          value={this.state.text}
          multiline={this.props.multiline}
          rows={this.props.rows}
          variant={this.props.variant || 'standard'}
          className="teste"
          label={this.props.label}
          type={this.props.type || 'text'}
          onChange={e => this.onChange(e.target.value)}
          onBlur={() => this.setState({ lostFocus: true })}
          InputProps={{
            endAdornment: (
              <InputAdornment position="end">
                <IconButton
                  disabled={this.props.disabled}
                  tabIndex={99}
                  onClick={this.props.iconClick}>
                  {this.props.Icon}
                </IconButton>
              </InputAdornment>
            )
          }}
        />
        <FormHelperText hidden={!this.state.lostFocus || !this.state.hasError} error={this.state.hasError}>
          {this.state.errorMessage}
        </FormHelperText>
      </FormControl>
    )
  }
}

export default IconTextInput