import httpService from './httpService'

const get = id => httpService.get(`/vehicle/${id}`).then(p => p.data)
const getAll = () => httpService.get('/vehicle').then(p => p.data)
const save = q => q.id ? httpService.put(`/vehicle/${q.id}`, q) : httpService.post('/vehicle', q)
const remove = id => httpService.delete(`/vehicle/${id}`)

export default {
  get,
  getAll,
  save,
  remove
}