export const USER_CHANGED = 'USER_CHANGED'
export const GLOBAL_LOADER_CHANGED = 'GLOBAL_LOADER_CHANGED'
export const THEME_MODE_CHANGED = 'THEME_MODE_CHANGED'

export const userChanged = user => ({ type: USER_CHANGED, payload: user })

// Contador em vez de booleano: várias requisições em paralelo não escondem o loader antes da hora.
export const showGlobalLoader = () => ({ type: GLOBAL_LOADER_CHANGED, payload: 1 })

export const hideGlobalLoader = () => ({ type: GLOBAL_LOADER_CHANGED, payload: -1 })

export const themeModeChanged = mode => ({ type: THEME_MODE_CHANGED, payload: mode })
