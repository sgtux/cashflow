import httpService from './httpService'

const get = () => httpService.get(`/payment`)
const create = (q) => httpService.post('/payment', q)
const update = (q) => httpService.put('/payment', q)
const remove = (id) => httpService.delete(`/payment/${id}`)

export default {
  get,
  create,
  update,
  remove
}