import httpService from './httpService'

const get = id => httpService.get(`/HouseholdExpense/${id}`)
const getAll = (month, year) => httpService.get(`/HouseholdExpense?month=${month}&year=${year}`)
const save = q => q.id ? httpService.put('/HouseholdExpense', q) : httpService.post('/HouseholdExpense', q)
const remove = id => httpService.delete(`/HouseholdExpense/${id}`)

export default {
  get,
  getAll,
  save,
  remove
}