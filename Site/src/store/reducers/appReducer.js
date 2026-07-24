import { ActionTypes } from '../actions'
import { StorageService } from '../../services'

const initialState = {
  user: StorageService.getUser(),
  selectedMenu: location.hash.replace('#', ''),
  globalLoader: false,
  themeMode: StorageService.getThemeMode()
}

export const appReducer = (state = initialState, action) => {
  switch (action.type) {
    case ActionTypes.USER_CHANGED:
      StorageService.setUser(action.payload)
      return { ...state, user: action.payload }
    case ActionTypes.MENU_CHANGED:
      return { ...state, selectedMenu: action.payload }
    case ActionTypes.GLOBAL_LOADER_CHANGED:
      return { ...state, globalLoader: action.payload }
    case ActionTypes.THEME_MODE_CHANGED:
      StorageService.setThemeMode(action.payload)
      return { ...state, themeMode: action.payload }
    default:
      return state;
  }
}