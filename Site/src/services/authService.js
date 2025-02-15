import httpService from './httpService'
import { StorageService } from './storage.service'

const login = user =>
  httpService.postNotAuthenticated('/token', user)
    .then(res => {
      StorageService.setToken(res.token)
      return httpService.get('/account')
    })

const getGoogleClientId = () => httpService.getNotAuthenticated('/account/GoogleClientId')
const googleSignIn = token => httpService.postNotAuthenticated('/account/GoogleSignIn', { idToken: token })
  .then(res => {
    StorageService.setToken(res.token)
    if (res.picture)
      StorageService.setPicture(res.picture)
    return httpService.get('/account')
  })

const createAccount = account =>
  httpService.postNotAuthenticated('/account', account)
    .then(res => {
      StorageService.setToken(res.token)
      return httpService.get('/account')
    })

const getAccount = () => httpService.get('/account')

const update = account => httpService.put('/account', account)

const logout = () => StorageService.setToken(null)

export default {
  logout,
  login,
  createAccount,
  getAccount,
  update,
  getGoogleClientId,
  googleSignIn
}