import React from 'react'
import { Route, Routes } from 'react-router-dom'
import { createHashHistory } from 'history'
import {
  CreditCards,
  Payments,
  Projection,
  Salary,
  EditPayment,
  DailyExpenses,
  EditDailyExpenses,
  Vehicles
} from '../../scenes'

const history = createHashHistory()
const isAuthenticated = true
const freeRoutes = ['/', '/about']

let currentPath = location.hash.replace('#', '')

export const getCurrentPath = () => currentPath

history.listen((location, action) => {
  if (freeRoutes.indexOf(location.pathname) === -1 && !isAuthenticated) {
    window.location.hash = '#/'
    currentPath = '/'
  } else
    currentPath = location.pathname
})

export default function () {
  return (
    <Routes>
      <Route path="/" exact={true} element={<Projection />} />
      <Route path="/payments" element={<Payments />} />
      <Route path="/credit-cards" element={<CreditCards />} />
      <Route path="/projection" element={<Projection />} />
      <Route path="/salary" element={<Salary />} />
      <Route path="/edit-payment/:id" element={<EditPayment />} />
      <Route path="/daily-expenses" element={<DailyExpenses />} />
      <Route path="/edit-daily-expenses/:id" element={<EditDailyExpenses />} />
      <Route path="/vehicles" element={<Vehicles />} />
    </Routes>
  )
}