export const Months = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro']

export const dateToString = date => {
  if (!date)
    date = new Date()
  else if (typeof (date) === 'string')
    date = new Date(date)

  let day = date.getDate()
  day = day > 9 ? day : '0' + day

  let month = date.getMonth() + 1
  month = month > 9 ? month : '0' + month

  return `${day}/${month}/${date.getFullYear()}`
}

export const getMonthName = monthIndex => (isNaN(monthIndex) || !Months[monthIndex]) ? '' : Months[monthIndex]

// "10/2026" -> "Outubro/2026"
export const getMonthYear = date => {
  const d = (date || '').split('/')
  return `${Months[Number(d[0]) - 1]}/${d[1]}`
}

export const toReal = val => {
  if (val === null || val === undefined || val === '' || isNaN(val))
    return val
  return Number(val).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

export const toThousandFormat = number => {
  return !number || isNaN(number) ? number : number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
}

export const toDecimalText = number => (number === null || number === undefined) ? '' : number.toString().replace('.', ',')

export const fromReal = val => typeof (val) === 'string' ? Number((val || '').replace(/[^0-9,]/g, '').replace(',', '.') || 0) : (val || 0)

export const isSameMonth = (d1, d2) => {
  const date1 = new Date(d1)
  const date2 = new Date(d2)
  if (isNaN(date1) || isNaN(date2))
    return false
  return date1.getMonth() === date2.getMonth()
    && date1.getFullYear() === date2.getFullYear()
}

export const buildQueryParameters = obj => {
  if (!obj) {
    return ''
  }
  let result = ''
  for (const i in obj) {
    if (obj[i] !== undefined && obj[i] !== null) {
      if (typeof (obj[i]) === 'object' && obj[i].toISOString)
        result += `${i}=${obj[i].toISOString()}&`
      else
        result += `${i}=${obj[i]}&`
    }
  }
  return result
}

export const sum = (list, selector = p => p) => (list || []).reduce((total, item) => total + (Number(selector(item)) || 0), 0)
