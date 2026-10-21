import httpService from './httpService'

const getChart = (month, year) => httpService.get(`/home?month=${month}&year=${year}`)
const getProjection = () => httpService.get('/projection')

export default {
  getChart,
  getProjection
}