import React, { useState, useEffect } from 'react'
import { useSelector, useDispatch } from 'react-redux'
import Sidebar from 'react-sidebar'
import { HashRouter } from 'react-router-dom'

import { ToastContainer } from 'react-toastify'

import { Colors } from '../../helpers/themes'
import { AppToolbar } from './Toolbar'
import { SidebarContent } from './'
import AppRouter from './AppRouter'
import { Auth } from '../../scenes'
import { AlertModal } from '../main/Modal'
import { userChanged } from '../../actions'
import { registerCallbackUnauthorized } from '../../services/httpService'

export function MainComponent() {

  const [sidebarDocked, setSidebarDocked] = useState(false)
  const [sidebarIsOpen, setSidebarIsOpen] = useState(false)
  const [showModal, setShowModal] = useState(false)
  let mql = null

  const { user } = useSelector(state => state.appState)

  const dispatch = useDispatch()

  useEffect(() => {
    if (mql === null)
      mql = window.matchMedia('(min-width: 1024px)')
    mediaQueryChanged()
    mql.addListener(mediaQueryChanged)
    registerCallbackUnauthorized(() => setShowModal(true))
    return () => mql.removeListener(mediaQueryChanged)
  }, [])

  function mediaQueryChanged() {
    setSidebarDocked(mql.matches)
  }

  function logout() {
    setShowModal(false)
    dispatch(userChanged(null))
  }

  return (
    <div>
      {user ?
        <HashRouter>
          <Sidebar
            sidebar={<SidebarContent closeSidebar={() => setSidebarIsOpen(false)} />}
            open={sidebarIsOpen}
            onSetOpen={open => setSidebarIsOpen(open)}
            docked={sidebarDocked}
            styles={{ sidebar: { background: Colors.AppGreen } }}>
            <AppToolbar
              dockedMenu={sidebarDocked}
              openSideBar={() => setSidebarIsOpen(true)}
            />
            <AppRouter />
          </Sidebar>
        </HashRouter>
        :
        <Auth />
      }
      <ToastContainer />
      <AlertModal
        text='Sessão Expirada!'
        show={showModal}
        onClose={() => logout()} />
    </div>
  )
}