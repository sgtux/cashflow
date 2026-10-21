import axios from 'axios'
import { toast } from '../helpers'

import { StorageService } from './storage.service'

const isDevelopment = process.env.NODE_ENV !== 'production'

let callbackTokenExpired = null

export const registerCallbackUnauthorized = callback => callbackTokenExpired = callback

axios.interceptors.response.use(response => response, err => {
  const { request, status } = err.response || {}
  if (status === 401 && request && !request.responseURL.endsWith('/api/token') && callbackTokenExpired)
    callbackTokenExpired()
  return Promise.reject(err)
})

const getToken = () => StorageService.getToken()

const sendRequest = async (method, url, headers, data) => {
  return axios({
    method: method,
    headers: headers,
    url: url.startsWith('http') ? url : `${API_URL}/api${url}`,
    data: data
  }).then(res => {
    // Avisos técnicos só fazem sentido para quem desenvolve; o usuário final não precisa vê-los.
    if (isDevelopment && res.data.requestElapsedTime > 500)
      toast.warning(`${url} is too slow around ${res.data.requestElapsedTime}ms`)
    if (isDevelopment && res.data.fromCache)
      toast.info('Obtido do cache.')
    return res.data.data
  })
    .catch(err => {
      const response = err.response || {}
      const result = {
        message: (response.data || {}).message,
        status: response.status,
        messages: (response.data || {}).errors || []
      }

      if (!err.response)
        toast.error('Não foi possível falar com o servidor. Verifique sua conexão e tente de novo.')

      result.messages.forEach(p => toast.error(p))

      throw result
    })
}

const getHeaders = () => ({ Authorization: `Bearer ${getToken()}`, 'Content-Type': 'application/json' })

const configs = await sendRequest('get', '/Config').catch(() => ({}))

export const appConfig = {
  googleClientId: configs.googleClientId,
  authApiUrl: configs.authApiUrl,
  microsoftClientId: configs.microsoftClientId,
  microsoftTenantId: configs.microsoftTenantId,
  microsoftRedirectUri: configs.microsoftRedirectUri,
  microsoftAuthority: configs.microsoftAuthority
}

export default {
  getNotAuthenticated: url => sendRequest('get', url),
  postNotAuthenticated: (url, body) => sendRequest('post', url, { 'Content-Type': 'application/json' }, body),
  get: url => sendRequest('get', url, getHeaders()),
  post: (url, body) => sendRequest('post', url, getHeaders(), body),
  put: (url, body) => sendRequest('put', url, getHeaders(), body),
  delete: (url, body) => sendRequest('delete', url, getHeaders(), body)
}
