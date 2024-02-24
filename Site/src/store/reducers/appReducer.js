import { ActionTypes } from '../actions'
import { StorageService } from '../../services'

const initialState = {
  user: StorageService.getUser(),
  selectedMenu: location.hash.replace('#', '')
}

export const appReducer = (state = initialState, action) => {
  switch (action.type) {
    case ActionTypes.USER_CHANGED:
      StorageService.setUser(action.payload)
      return { ...state, user: action.payload }
    case ActionTypes.MENU_CHANGED:
      return { ...state, selectedMenu: action.payload }
    default:
      return state;
  }
}