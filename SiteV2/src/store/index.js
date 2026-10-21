import { createStore, combineReducers } from 'redux'

import { USER_CHANGED, GLOBAL_LOADER_CHANGED, THEME_MODE_CHANGED } from './actions'
import { StorageService } from '../services/storage.service'

const initialState = {
  user: StorageService.getUser(),
  loaderCount: 0,
  themeMode: StorageService.getThemeMode()
}

const appReducer = (state = initialState, action) => {
  switch (action.type) {
    case USER_CHANGED:
      StorageService.setUser(action.payload)
      return { ...state, user: action.payload }
    case GLOBAL_LOADER_CHANGED:
      return { ...state, loaderCount: Math.max(0, state.loaderCount + action.payload) }
    case THEME_MODE_CHANGED:
      StorageService.setThemeMode(action.payload)
      return { ...state, themeMode: action.payload }
    default:
      return state
  }
}

export const Store = createStore(combineReducers({ appState: appReducer }))
