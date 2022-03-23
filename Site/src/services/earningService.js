import httpService from './httpService'
import { buildQueryParameters } from '../helpers/utils'

const get = id => httpService.get(`/earning/${id}`)
const getAll = filter => httpService.get(`/earning?${buildQueryParameters(filter)}`)
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