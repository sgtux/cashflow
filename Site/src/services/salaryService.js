import httpService from './httpService'

const get = () => httpService.get('/salary')
const save = q => q.id ? httpService.put('/salary', q) : httpService.post('/salary', q)
const remove = id => httpService.delete(`/salary/${id}`)

export default {
  get,
  save,
  remove
}