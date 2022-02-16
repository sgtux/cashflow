import httpService from './httpService'

const getAll = () => httpService.get('/RemainingBalance')
const update = q => httpService.put('/RemainingBalance', q)
const recalculate = q => httpService.put('/RemainingBalance/Recalculate', q)

export default {
  getAll,
  update,
  recalculate
}