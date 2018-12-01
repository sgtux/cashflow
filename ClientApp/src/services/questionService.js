import httpService from './httpService'

const get = (id) => httpService.get(`/question/${id}`)
const getMy = () => httpService.get('/question')
const getOthers = () => httpService.get('/question-others')
const getAreas = () => httpService.get('/areas')
const getShared = (id) => httpService.get(`/question-get-shared/${id}`)
const create = (q) => httpService.post('/question', q)
const update = (q) => httpService.put('/question', q)
const share = (id, shared) => httpService.put('/question-share', { id, shared })
const remove = (id) => httpService.delete(`/question/${id}`)

export default {
  get,
  getMy,
  getOthers,
  getAreas,
  create,
  update,
  share,
  getShared,
  remove
}