export const LANGUAGE_CHANGED = 'LANGUAGE_CHANGED'
export const USER_CHANGED = 'USER_CHANGED'
export const CHANGE_VISIBLE_ALERT = 'CHANGE_VISIBLE_ALERT'
export const MENU_CHANGED = 'MENU_CHANGED'

export const ActionTypes = {
  LANGUAGE_CHANGED: LANGUAGE_CHANGED,
  USER_CHANGED: USER_CHANGED,
  CHANGE_VISIBLE_ALERT: CHANGE_VISIBLE_ALERT,
  MENU_CHANGED: MENU_CHANGED
}

export const languageChanged = newLanguage => ({
  type: LANGUAGE_CHANGED,
  payload: newLanguage
})

export const userChanged = user => ({
  type: USER_CHANGED,
  payload: user
})

export const showError = (message) => ({
  type: CHANGE_VISIBLE_ALERT,
  payload: { message, type: 'error', show: true }
})

export const showSuccess = (message) => ({
  type: CHANGE_VISIBLE_ALERT,
  payload: { message, type: 'success', show: true }
})

export const hideAlert = () => ({
  type: CHANGE_VISIBLE_ALERT,
  payload: { message: '', show: false }
})

export const menuChanged = newMenu => ({
  type: MENU_CHANGED,
  payload: newMenu
})