import httpService from './httpService'

const get = id => httpService.get(`/earning/${id}`)
const getAll = () => httpService.get('/earning')
const getTypes = () => httpService.get('/earning/types')
const save = q => q.id ? httpService.put('/earning', q) : httpService.post('/earning', q)
const remove = id => httpService.delete(`/earning/${id}`)

export default {
  get,
  getAll,
  getTypes,
  save,
  remove
}