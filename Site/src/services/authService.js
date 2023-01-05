import httpService from './httpService'
import { STORAGE_KEYS } from '../helpers/storageKeys'

const updateToken = token => localStorage.setItem(STORAGE_KEYS.TOKEN, token)

const login = user =>
  httpService.postNotAuthenticated('/token', user)
    .then(res => {
      updateToken(res.token)
      return httpService.get('/account')
    })

const createAccount = account =>
  httpService.postNotAuthenticated('/account', account)
    .then(res => {
      updateToken(res.token)
      return httpService.get('/account')
    })

const getAccount = () => httpService.get('/account')

const updateSpendingCeiling = spendingCeiling =>
  httpService.put('/account/SpendingCeiling', { spendingCeiling })

const logout = () => updateToken(null)

export default {
  logout,
  login,
  createAccount,
  getAccount,
  updateSpendingCeiling
}