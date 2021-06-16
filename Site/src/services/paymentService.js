import httpService from './httpService'

const get = () => httpService.get('/payment').then(p => p.data)
const getFuture = (startDate, endDate) => httpService.get(`/payment/FuturePayments?startDate=${startDate}&endDate=${endDate}`)
const create = q => httpService.post('/payment', q)
const update = q => httpService.put('/payment', q)
const remove = id => httpService.delete(`/payment/${id}`)

export default {
  get,
  getFuture,
  create,
  update,
  remove
}