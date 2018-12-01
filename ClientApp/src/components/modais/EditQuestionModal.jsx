import React from 'react'
import {
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Zoom,
  MobileStepper,
  FormControl,
  InputLabel,
  Select,
  MenuItem
} from '@material-ui/core'
import { KeyboardArrowLeft, KeyboardArrowRight } from '@material-ui/icons'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import IconTextInput from '../main/IconTextInput'
import Stars from '../question/Stars'
import EditQuestionAlternatives from '../question/EditQuestionAlternatives'
import { questionService } from '../../services'
import { showError, showSuccess } from '../../actions'

const styles = {
  legend: {
    color: '#666',
    border: 'solid 1px #ccc',
    fontSize: '20px',
    fontFamily: 'sans-serif',
    textTransform: 'uppercase'
  }
}

const tutorialSteps = [
  { title: 'Questão' },
  { title: 'Alternativas' }
]

const OUTRAS = 'OUTRA...'

class EditQuestionModal extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      activeStep: 0,
      difficulty: 1,
      areas: [],
      areaSelected: OUTRAS,
      areaCustomName: '',
      question: {},
      description: '',
      showAlertModal: false
    }
    this.onEnter = this.onEnter.bind(this)
  }

  onEnter() {
    const { areas } = this.state
    const question = Object.assign({}, this.props.question)
    let selected = areas[0]

    if (question.area) {
      selected = question.area.toUpperCase()
      if (areas.indexOf(selected) === -1)
        areas.push(selected)
    }

    const alternatives = question.answers ? question.answers.map(p => Object.assign({}, p)) : []

    this.setState({
      alternativesValid: alternatives.length > 1 && alternatives.filter(p => !p.description).length === 0,
      alternatives: alternatives
    })

    this.setState({
      areas: areas,
      areaSelected: selected,
      difficulty: question.difficulty || 1,
      description: question.description || '',
      areaCustomName: '',
      activeStep: 0,
      alternatives: alternatives,
      question: question
    })
  }

  componentDidMount() {
    questionService.getAreas().then(res => {
      const areas = res.map(p => p.toUpperCase()).concat(this.state.areas)
      areas.push(OUTRAS)
      this.setState({ areas: areas })
    })
  }

  alternativeChanged(alternatives) {
    this.setState({
      alternativesValid: alternatives.filter(p => !p.description).length === 0,
      alternatives: alternatives
    })
  }

  save() {
    const { description, difficulty, areaCustomName, areaSelected, alternatives } = this.state
    const question = {
      description,
      difficulty,
      area: areaSelected === OUTRAS ? areaCustomName.toUpperCase() : areaSelected,
      answers: alternatives,
      id: this.props.question.id
    }

    const caller = (promise) => {
      promise.then(res => {
        this.props.showSuccess(res.message)
        this.props.close(true)
      }).catch(err => this.props.showError(err.message))
    }

    caller(question.id ? questionService.update(question) : questionService.create(question))
  }

  render() {
    const { activeStep, difficulty } = this.state
    return (
      <Dialog
        open={this.props.open}
        onClose={() => this.props.close()}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
        transitionDuration={300}
        onEnter={this.onEnter}
        TransitionComponent={Zoom}>
        <DialogTitle id="alert-dialog-title">
          {this.props.question.id > 0 ? 'Edição' : 'Nova'}</DialogTitle>
        <DialogContent>
          <fieldset style={styles.legend}>
            <legend>
              {tutorialSteps[activeStep].title}
            </legend>
            {activeStep === 0 ?
              <div>
                <FormControl>
                  <InputLabel>Área</InputLabel>
                  <Select
                    style={{ width: '250px' }}
                    value={this.state.areaSelected}
                    onChange={(e) => this.setState({ areaSelected: e.target.value })}
                    displayEmpty>
                    {this.state.areas.map((p, i) => <MenuItem key={i} value={p}>{p}</MenuItem>)}
                  </Select>
                  <div hidden={this.state.areaSelected !== OUTRAS}>
                    <IconTextInput
                      value={this.state.areaCustomName}
                      defaultValue={this.state.areaCustomName}
                      minlength={2}
                      required
                      onChange={(e) => this.setState({ areaCustomName: e.value })}
                      label="Área" />
                  </div>
                </FormControl>
                <Stars style={{ marginTop: '20px' }}
                  filled={difficulty}
                  label="Dificuldade"
                  onClick={(i) => this.setState({ difficulty: i })} />
                <IconTextInput
                  style={{ marginTop: '20px' }}
                  required
                  multiline={true}
                  rowsMax="3"
                  name="teste"
                  rows="3"
                  maxlength={100}
                  value={this.state.description}
                  onChange={t => this.setState({ description: t.value, descriptionValid: t.valid })}
                  label="Descrição" />
              </div>
              : <EditQuestionAlternatives
                alternatives={this.state.alternatives}
                onAlternativeChange={(e) => this.alternativeChanged(e)} />
            }
          </fieldset>
          <MobileStepper
            steps={2}
            position="static"
            activeStep={activeStep}
            backButton={
              <Button size="small" color="primary"
                onClick={() => this.setState({ activeStep: 0 })}
                disabled={activeStep === 0}>
                {<KeyboardArrowLeft />}anterior</Button>
            }
            nextButton={
              <Button size="small" color="primary"
                onClick={() => this.setState({ activeStep: 1 })}
                disabled={activeStep === 1 || !this.state.description || (this.state.areaSelected === OUTRAS && !this.state.areaCustomName)}>
                próximo{<KeyboardArrowRight />}
              </Button>
            }
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => this.props.close()} variant="raised" autoFocus>cancelar</Button>
          <Button disabled={!this.state.description || !this.state.alternativesValid || (this.state.areaSelected === OUTRAS && !this.state.areaCustomName)}
            onClick={() => this.save()} color="primary" variant="raised" autoFocus>Salvar</Button>
        </DialogActions>
      </Dialog>
    )
  }
}

const mapDispatchToProps = dispatch => bindActionCreators({ showError, showSuccess }, dispatch)

export default connect(null, mapDispatchToProps)(EditQuestionModal)