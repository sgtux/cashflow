import React from 'react'
import { Route, Routes } from 'react-router-dom'
import { createHashHistory } from 'history'
import {
  CreditCards,
  Payments,
  Projection,
  Salary,
  EditPayment,
  HouseholdExpenses,
  EditHouseholdExpense,
  Vehicles,
  Home,
  RecurringExpenses,
  EditRecurringExpense
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
      <Route path="/" exact={true} element={<Home />} />
      <Route path="/payments" element={<Payments />} />
      <Route path="/credit-cards" element={<CreditCards />} />
      <Route path="/projection" element={<Projection />} />
      <Route path="/salary" element={<Salary />} />
      <Route path="/edit-payment/:id" element={<EditPayment />} />
      <Route path="/household-expenses" element={<HouseholdExpenses />} />
      <Route path="/edit-household-expense/:id" element={<EditHouseholdExpense />} />
      <Route path="/vehicles" element={<Vehicles />} />
      <Route path="/recurring-expenses" element={<RecurringExpenses />} />
      <Route path="/edit-recurring-expenses/:id" element={<EditRecurringExpense />} />
    </Routes>
  )
}