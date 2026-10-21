import httpService from './httpService'

const get = id => httpService.get(`/RecurringExpense/${id}`)
const getAll = showInactive => httpService.get(`/RecurringExpense?active=${showInactive ? 0 : 1}`)
const save = q => q.id ? httpService.put(`/RecurringExpense/${q.id}`, q) : httpService.post('/RecurringExpense', q)
const remove = id => httpService.delete(`/RecurringExpense/${id}`)
const saveHistory = q => q.id ? httpService.put(`/RecurringExpense/History/${q.id}`, q) : httpService.post('/RecurringExpense/History', q)
const removeHistory = (recurringExpenseId, id) => httpService.delete(`/RecurringExpense/${recurringExpenseId}/History/${id}`)

export default {
  get,
  getAll,
  save,
  remove,
  saveHistory,
  removeHistory
}