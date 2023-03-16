import { ActionTypes } from '../actions'

const initialState = {
  show: false,
  message: '',
  type: 'success'
}

export const modalReducer = (state = initialState, action) => {
  if (action.type === ActionTypes.CHANGE_VISIBLE_ALERT) {
    return action.payload
  }
  return state
}