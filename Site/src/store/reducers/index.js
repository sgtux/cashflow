import { combineReducers } from 'redux'
import { appReducer } from './appReducer'
import { modalReducer } from './modalReducer'

export const Reducers = combineReducers({
  appState: appReducer,
  modalState: modalReducer
})