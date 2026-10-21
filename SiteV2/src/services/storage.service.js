import { StorageKeys } from '../helpers/storageKeys'

const read = key => {
  try { return localStorage.getItem(key) } catch { return null }
}

const write = (key, value) => {
  try {
    if (value === null || value === undefined)
      localStorage.removeItem(key)
    else
      localStorage.setItem(key, value)
  } catch { /* armazenamento indisponível (aba anônima, bloqueio) */ }
}

const getUser = () => {
  try { return JSON.parse(read(StorageKeys.USER)) } catch { return null }
}

const setUser = user => write(StorageKeys.USER, user ? JSON.stringify(user) : null)

const getToken = () => read(StorageKeys.TOKEN)

const setToken = token => write(StorageKeys.TOKEN, token || null)

const getPicture = () => read(StorageKeys.PICTURE)

const setPicture = picture => write(StorageKeys.PICTURE, picture || null)

const getThemeMode = () => read(StorageKeys.THEME_MODE_V2) || 'light'

const setThemeMode = mode => write(StorageKeys.THEME_MODE_V2, mode || null)

const isGuideDismissed = key => read(`${StorageKeys.GUIDE_DISMISSED}_${key}`) === '1'

const setGuideDismissed = (key, dismissed) => write(`${StorageKeys.GUIDE_DISMISSED}_${key}`, dismissed ? '1' : null)

export const StorageService = {
  getUser,
  setUser,
  getToken,
  setToken,
  getPicture,
  setPicture,
  getThemeMode,
  setThemeMode,
  isGuideDismissed,
  setGuideDismissed
}
