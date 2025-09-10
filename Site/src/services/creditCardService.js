import httpService from './httpService'

const get = () => httpService.get('/CreditCard')
const create = q => httpService.post('/CreditCard', q)
const update = q => httpService.put('/CreditCard', q)
const remove = id => httpService.delete(`/CreditCard/${id}`)
const payCurrentInvoicePayment = q => httpService.put('/CreditCard/PayCurrentInvoicePayment', q)

export default {
  get,
  create,
  update,
  remove,
  payCurrentInvoicePayment
}