import httpService from './httpService'

const get = id => httpService.get(`/RecurringEarning/${id}`)
const getAll = showInactive => httpService.get(`/RecurringEarning?active=${showInactive ? 0 : 1}`)
const save = q => q.id ? httpService.put(`/RecurringEarning/${q.id}`, q) : httpService.post('/RecurringEarning', q)
const remove = id => httpService.delete(`/RecurringEarning/${id}`)
const saveHistory = q => q.id ? httpService.put(`/RecurringEarning/History/${q.id}`, q) : httpService.post('/RecurringEarning/History', q)
const removeHistory = (recurringEarningId, id) => httpService.delete(`/RecurringEarning/${recurringEarningId}/History/${id}`)

export default {
  get,
  getAll,
  save,
  remove,
  saveHistory,
  removeHistory
}
