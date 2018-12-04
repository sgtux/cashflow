import axios from 'axios'
import { STORAGE_KEYS } from '../helpers/storageKeys'

const getToken = () => localStorage.getItem(STORAGE_KEYS.TOKEN)

const sendRequest = (method, url, headers, data) => {
  return axios({
    method: method,
    headers: headers,
    url: url,
    data: data
  }).then(res => res.data)
    .catch(err => { throw err.response.data })
}

const getHeaders = () => ({ Authorization: `Bearer ${getToken()}` })

export default {
  getNotAuthenticated: (url) => sendRequest('get', `/api${url}`),
  postNotAuthenticated: (url, body) => sendRequest('post', `/api${url}`, null, body),
  get: (url) => sendRequest('get', `/api${url}`, getHeaders()),
  post: (url, body) => sendRequest('post', `/api${url}`, getHeaders(), body),
  put: (url, body) => sendRequest('put', `/api${url}`, getHeaders(), body),
  delete: (url, body) => sendRequest('delete', `/api${url}`, getHeaders(), body)
}