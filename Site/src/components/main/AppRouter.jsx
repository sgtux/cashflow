import React from 'react'
import { Route, Routes } from 'react-router-dom'
import {
  CreditCards,
  Payments,
  Projection,
  Earnings,
  EditPayment,
  HouseholdExpenses,
  Vehicles,
  Home,
  RecurringExpenses,
  EditRecurringExpense,
  RemainingBalances,
  Account
} from '../../scenes'

export default function () {
  return (
    <Routes>
      <Route path="/" exact={true} element={<Home />} />
      <Route path="/payments" element={<Payments />} />
      <Route path="/credit-cards" element={<CreditCards />} />
      <Route path="/projection" element={<Projection />} />
      <Route path="/earnings" element={<Earnings />} />
      <Route path="/edit-payment/:id" element={<EditPayment />} />
      <Route path="/household-expenses" element={<HouseholdExpenses />} />
      <Route path="/vehicles" element={<Vehicles />} />
      <Route path="/recurring-expenses" element={<RecurringExpenses />} />
      <Route path="/edit-recurring-expenses/:id" element={<EditRecurringExpense />} />
      <Route path="/remaining-balance" element={<RemainingBalances />} />
      <Route path="/account" element={<Account />} />
    </Routes>
  )
}