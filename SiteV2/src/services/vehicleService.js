import httpService from './httpService'

const get = id => httpService.get(`/vehicle/${id}`)
const getAll = showInactives => httpService.get(`/vehicle?showInactives=${showInactives}`)
const save = q => q.id ? httpService.put(`/vehicle/${q.id}`, q) : httpService.post('/vehicle', q)
const remove = id => httpService.delete(`/vehicle/${id}`)
const saveFuelExpense = q => q.id ? httpService.put(`/fuelExpense/${q.id}`, q) : httpService.post('/fuelExpense', q)
const removeFuelExpense = id => httpService.delete(`/fuelExpense/${id}`)

export default {
  get,
  getAll,
  save,
  remove,
  saveFuelExpense,
  removeFuelExpense
}