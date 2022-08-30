import httpService from './httpService'

const getChart = (month, year) => httpService.get(`/home?month=${month}&year=${year}`)
const getProjection = (month, year) => httpService.get(`/projection?month=${month}&year=${year}`)

export default {
  getChart,
  getProjection
}