import { StorageKeys } from '../helpers/storageKeys'

const getUser = () => JSON.parse(localStorage.getItem(StorageKeys.USER))

const setUser = user => user ? localStorage.setItem(StorageKeys.USER, JSON.stringify(user)) : localStorage.removeItem(StorageKeys.USER)

const getToken = () => localStorage.getItem(StorageKeys.TOKEN)

const setToken = token => token ? localStorage.setItem(StorageKeys.TOKEN, token) : localStorage.removeItem(StorageKeys.TOKEN)

const getPicture = () => localStorage.getItem(StorageKeys.PICTURE)

const setPicture = picture => picture ? localStorage.setItem(StorageKeys.PICTURE, picture) : localStorage.removeItem(StorageKeys.StorageKeys.PICTURE)

export const StorageService = {
    getUser,
    setUser,
    getToken,
    setToken,
    getPicture,
    setPicture
}