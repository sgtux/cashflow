import React from 'react'
import Sidebar from 'react-sidebar'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { HashRouter } from 'react-router-dom'

import { Colors } from '../../helpers/themes'
import Toobar from './Toolbar'
import SidebarContent from './SidebarContent'
import AppRouter from './AppRouter'
import Auth from '../../scenes/auth/Auth'
import { AlertModal } from '../main/Modal'
import { hideAlert } from '../../actions'

const mql = window.matchMedia(`(min-width: 1024px)`)

class MainComponent extends React.Component {
  constructor(props) {
    super(props)
    this.state = {
      sidebarDocked: mql.matches,
      sidebarIsOpen: false,
      showModal: false
    }
    this.mediaQueryChanged = this.mediaQueryChanged.bind(this)
  }

  componentWillMount() {
    mql.addListener(this.mediaQueryChanged)
  }

  componentWillUnmount() {
    mql.removeListener(this.mediaQueryChanged)
  }

  mediaQueryChanged() {
    this.setState({ sidebarDocked: mql.matches })
  }

  componentDidMount() {
    // axios.interceptors.response.use(response => response, error => {
    //   console.log(error.response)
    //   if (error.response && error.response.status === 401)
    //     this.setState({ showModal: true })
    //   return Promise.reject(error.response)
    // })
  }

  render() {
    return (
      <div>
        {this.props.user ?
          <HashRouter>
            <Sidebar
              sidebar={<SidebarContent closeSidebar={() => this.setState({ sidebarIsOpen: false })} />}
              open={this.state.sidebarIsOpen}
              onSetOpen={open => this.setState({ sidebarIsOpen: open })}
              docked={this.state.sidebarDocked}
              styles={{ sidebar: { background: Colors.AppGreen } }}>
              <Toobar
                dockedMenu={this.state.sidebarDocked}
                openSideBar={() => this.setState({ sidebarIsOpen: true })}
              />
              <AppRouter />
            </Sidebar>
          </HashRouter>
          :
          <Auth />
        }
        <AlertModal type={this.props.modal.type}
          text={this.props.modal.message}
          show={this.props.modal.show}
          onClose={() => this.props.hideAlert()} />
      </div>
    )
  }
}

const mapStateToProps = state => ({ user: state.appState.user, modal: state.modalState })
const mapDispatchToProps = dispatch => bindActionCreators({ hideAlert }, dispatch)

export default connect(mapStateToProps, mapDispatchToProps)(MainComponent)