import React from 'react'
import { Radio, IconButton, Button } from '@material-ui/core'
import { Remove, Delete, Add } from '@material-ui/icons'

import IconTextInput from '../main/IconTextInput'

const classifications = ['A', 'B', 'C', 'D', 'E', 'F']

export default class EditQuestionAlternatives extends React.Component {

  constructor(props) {
    super(props)
    const alternatives = Array.isArray(props.alternatives)
      && props.alternatives.length > 0 ? props.alternatives : [
        { correct: true, classification: 'A' },
        { correct: false, classification: 'B' }
      ]
    this.state = {
      alternatives: alternatives,
      correct: alternatives.filter(p => p.correct)[0].classification
    }
  }

  componentDidMount(){
    this.props.onAlternativeChange(this.state.alternatives)
  }

  removeAlternative(alt) {
    const alts = this.state.alternatives.filter(p => p.classification !== alt.classification)
    alts.forEach((v, i) => v.classification = classifications[i])
    this.setState({
      alternatives: alts,
      correct: classifications[0]
    })
    this.props.onAlternativeChange(alts)
  }

  addAlternative() {
    const alt = {}
    alt.classification = classifications[this.state.alternatives.length]
    const alts = this.state.alternatives.concat([alt])
    this.setState({
      alternatives: alts
    })
    this.props.onAlternativeChange(alts)
  }

  descriptionChanged(alternative, value) {
    alternative.description = value
    this.props.onAlternativeChange(this.state.alternatives)
  }

  render() {
    return (
      <div>
        <div style={{ textAlign: 'right', fontSize: '8px', fontWeight: 'bold' }}>
          <span>CORRETA?</span>
          <span style={{ marginLeft: '5px' }}>EXCLUIR</span>
        </div>
        {this.state.alternatives.map((p, i) => (
          <div key={i}>
            <IconTextInput required label={p.classification} defaultValue={p.description}
              onChange={e => this.descriptionChanged(p, e.value)} />
            <Radio color="primary" style={{ marginTop: '15px' }} checked={p.classification === this.state.correct}
              onChange={() => this.setState({ correct: p.classification })} />
            <IconButton color="secondary" style={{ marginTop: '15px' }} size="small" aria-label="Delete"
              onClick={() => this.removeAlternative(p)}
              disabled={this.state.alternatives.length < 3}>
              <Delete />
            </IconButton>
          </div>))
        }
        <div style={{ textAlign: 'center', marginTop: '10px' }}
          hidden={this.state.alternatives.length > 5}>
          <IconButton color="primary" aria-label="Add" onClick={() => this.addAlternative()}>
            <Add />
          </IconButton>
        </div>
      </div>
    )
  }
}