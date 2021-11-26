import httpService from './httpService'

const get = id => httpService.get(`/dailyExpenses/${id}`).then(p => p)
const getAll = (month, year) => httpService.get(`/dailyExpenses?month=${month}&year=${year}`).then(p => p)
const save = q => q.id ? httpService.put('/dailyExpenses', q) : httpService.post('/dailyExpenses', q)
const remove = id => httpService.delete(`/dailyExpenses/${id}`)

export default {
  get,
  getAll,
  save,
  remove
}