import React from 'react'
import { connect } from 'react-redux'
import List from '@material-ui/core/List'
import ListItem from '@material-ui/core/ListItem'
import ListItemIcon from '@material-ui/core/ListItemIcon'
import ListItemText from '@material-ui/core/ListItemText'
import Collapse from '@material-ui/core/Collapse'
import Divider from '@material-ui/core/Divider'
import QuestionAnswerIcon from '@material-ui/icons/QuestionAnswer'
import RoomIcon from '@material-ui/icons/RoomService'
import { Link, withRouter } from 'react-router-dom'

import { AppTexts } from '../../helpers/appTexts'
import { getCurrentPath } from './AppRouter'

const styles = {
  mainIcon: {
    color: 'white'
  },
  symbolDiv: {
    textAlign: 'center',
    width: '260px'
  },
  symbolSpan: {
    color: '#FFF',
    fontFamily: 'FrederickaGreat',
    fontSize: '60px'
  },
  mainText: {
    color: '#FFF',
    fontSize: '16px',
    fontWeight: 'bold'
  },
  subMainText: {
    color: '#FFF',
    marginLeft: '30px',
    fontSize: '14px'
  }
}

const MainText = (props) => (
  <ListItemText primary={<span style={styles.mainText}>{props.text}</span>} />
)

const SubMainText = (props) => (
  <ListItemText primary={<span style={styles.subMainText}>{props.text}</span>} />
)

const LinkListItem = (props) => {
  const clickHandle = (e) => {
    if (getCurrentPath() === props.to)
      e.preventDefault()
    props.onClick()
  }
  return (
    <Link to={props.to}
      onClick={e => clickHandle(e)}
      style={{ textDecoration: 'none' }}>
      <ListItem button >
        <ListItemText primary={<span style={styles.subMainText}>{props.text}</span>} />
      </ListItem>
    </Link>
  )
}

const {
  QuestionTexts,
  RoomTexts,
  ScoreTexts,
  NotificationTexts
} = AppTexts.MainComponent

class SidebarContent extends React.Component {

  constructor(props) {
    super(props)
    this.state = {}
    this.showMenu = this.showMenu.bind(this)
  }

  showMenu(menu) {
    menu = this.state.opened === menu ? '' : menu
    this.setState({ opened: menu })
  }

  render() {
    return (
      <div>
        <div style={styles.symbolDiv}>
          <span style={styles.symbolSpan}>{AppTexts.AppSymbol[this.props.language]}</span>
        </div>
        <List>
          <Divider />
          <ListItem button onClick={() => this.showMenu('question')}>
            <ListItemIcon>
              <QuestionAnswerIcon style={styles.mainIcon} />
            </ListItemIcon>
            <MainText text={QuestionTexts.Questions[this.props.language]} />
          </ListItem>
          <Collapse in={this.state.opened === 'question'} timeout={400} unmountOnExit>
            <List component="div" disablePadding>
              <LinkListItem onClick={() => this.props.closeSidebar()} to="/my-questions" text={QuestionTexts.My[this.props.language]} />
              <LinkListItem onClick={() => this.props.closeSidebar()} to="/shared-questions" text={QuestionTexts.Shared[this.props.language]} />
            </List>
          </Collapse>

          <Divider />
          <Link to="/rooms" style={{ textDecoration: 'none' }}>
            <ListItem button onClick={() => this.showMenu('room')}>
              <ListItemIcon>
                <RoomIcon style={styles.mainIcon} />
              </ListItemIcon>
              <MainText text={RoomTexts.Rooms[this.props.language]} />
            </ListItem>
          </Link>

          <Divider />
          <Link to="/score" style={{ textDecoration: 'none' }}>
            <ListItem button onClick={() => this.showMenu('score')}>
              <ListItemIcon>
                <RoomIcon style={styles.mainIcon} />
              </ListItemIcon>
              <MainText text={ScoreTexts.Scores[this.props.language]} />
            </ListItem>
          </Link>

          <Divider />
          <ListItem button onClick={() => this.showMenu('notification')}>
            <ListItemIcon>
              <RoomIcon style={styles.mainIcon} />
            </ListItemIcon>
            <MainText text={NotificationTexts.Notifications[this.props.language]} />
          </ListItem>
          <Collapse in={this.state.opened === 'notification'} timeout={400} unmountOnExit>
            <List component="div" disablePadding>
              <ListItem button >
                <SubMainText text={NotificationTexts.Notifications[this.props.language]} />
              </ListItem>
            </List>
          </Collapse>
          <Divider />
        </List>
      </div>
    )
  }
}

const mapStateToProps = state => ({ language: state.appState.language })

export default connect(mapStateToProps)(SidebarContent)