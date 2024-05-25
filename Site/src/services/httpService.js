import axios from 'axios'
import { toast } from '../helpers'

import { StorageService } from './storage.service'

let callbackTokenExpired = null

export const registerCallbackUnauthorized = callback => callbackTokenExpired = callback

axios.interceptors.response.use(response => response, err => {
  const { request, status } = err.response
  if (status === 401 && !request.responseURL.endsWith('/api/token'))
    callbackTokenExpired()
  return Promise.reject(err)
})

const getToken = () => StorageService.getToken()

const sendRequest = (method, url, headers, data) => {
  return axios({
    method: method,
    headers: headers,
    url: API_URL + url,
    data: data
  }).then(res => {
    if (res.data.requestElapsedTime > 500)
      toast.warning(`${url} is too slow around ${res.data.requestElapsedTime}ms`)
    if (res.data.fromCache)
      toast.info('Obtido do cache.')
    return res.data.data
  })
    .catch(err => {
      const result = {
        message: err.response.data.message,
        status: err.response.status,
        messages: ((err.response || {}).data || {}).errors || []
      }

      result.messages.forEach(p => toast.error(p))

      throw result
    })
}

const getHeaders = () => ({ Authorization: `Bearer ${getToken()}`, 'Content-Type': 'application/json' })

export default {
  getNotAuthenticated: url => sendRequest('get', `/api${url}`),
  postNotAuthenticated: (url, body) => sendRequest('post', `/api${url}`, { 'Content-Type': 'application/json' }, body),
  get: url => sendRequest('get', `/api${url}`, getHeaders()),
  post: (url, body) => sendRequest('post', `/api${url}`, getHeaders(), body),
  put: (url, body) => sendRequest('put', `/api${url}`, getHeaders(), body),
  delete: (url, body) => sendRequest('delete', `/api${url}`, getHeaders(), body)
}