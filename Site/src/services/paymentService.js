import httpService from './httpService'

const get = () => httpService.get('/payment').then(p => p.data)
const getTypes = () => httpService.get('/payment/types').then(p => p.data)
const getFuture = (startDate, endDate) => httpService.get(`/payment/projection?startDate=${startDate}&endDate=${endDate}`)
const save = p => p.id ? httpService.put('/payment', p) : httpService.post('/payment', p)
const remove = id => httpService.delete(`/payment/${id}`)

export default {
  get,
  getTypes,
  getFuture,
  save,
  remove
}