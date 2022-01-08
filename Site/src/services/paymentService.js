import httpService from './httpService'
import { buildQueryParameters } from '../helpers/utils'

const get = id => httpService.get(`/payment/${id}`)
const getAll = filter => httpService.get(`/payment?${buildQueryParameters(filter)}`)
const getTypes = () => httpService.get('/payment/types')
const save = p => p.id ? httpService.put('/payment', p) : httpService.post('/payment', p)
const remove = id => httpService.delete(`/payment/${id}`)

export default {
  get,
  getAll,
  getTypes,
  save,
  remove
}