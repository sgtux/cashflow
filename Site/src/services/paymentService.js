import httpService from './httpService'

const get = id => httpService.get(`/payment/${id}`).then(p => p.data)
const getAll = () => httpService.get('/payment').then(p => p.data)
const getTypes = () => httpService.get('/payment/types').then(p => p.data)
const getFuture = (startDate, endDate) => httpService.get(`/payment/projection?startDate=${startDate}&endDate=${endDate}`)
const save = p => p.id ? httpService.put('/payment', p) : httpService.post('/payment', p)
const remove = id => httpService.delete(`/payment/${id}`)

export default {
  get,
  getAll,
  getTypes,
  getFuture,
  save,
  remove
}