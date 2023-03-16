import httpService from './httpService'
import {StorageService} from './storage.service'

const login = user =>
  httpService.postNotAuthenticated('/token', user)
    .then(res => {
      StorageService.setToken(res.token)
      return httpService.get('/account')
    })

const createAccount = account =>
  httpService.postNotAuthenticated('/account', account)
    .then(res => {
      StorageService.setToken(res.token)
      return httpService.get('/account')
    })

const getAccount = () => httpService.get('/account')

const updateSpendingCeiling = spendingCeiling =>
  httpService.put('/account/SpendingCeiling', { spendingCeiling })

const logout = () => StorageService.setToken(null)

export default {
  logout,
  login,
  createAccount,
  getAccount,
  updateSpendingCeiling
}