import React from 'react'
import { Navigate, Route, Routes } from 'react-router-dom'

import {
  Home,
  Earnings,
  RecurringEarnings,
  EditRecurringEarning,
  HouseholdExpenses,
  RecurringExpenses,
  EditRecurringExpense,
  Payments,
  EditPayment,
  CreditCards,
  Vehicles,
  Projection,
  RemainingBalances,
  Account,
  Glossary
} from '../scenes'

// Mesmas rotas do Site, para que links antigos continuem funcionando, mais o Glossário.
export function AppRouter() {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/earnings" element={<Earnings />} />
      <Route path="/recurring-earnings" element={<RecurringEarnings />} />
      <Route path="/edit-recurring-earnings/:id" element={<EditRecurringEarning />} />
      <Route path="/household-expenses" element={<HouseholdExpenses />} />
      <Route path="/recurring-expenses" element={<RecurringExpenses />} />
      <Route path="/edit-recurring-expenses/:id" element={<EditRecurringExpense />} />
      <Route path="/payments" element={<Payments />} />
      <Route path="/edit-payment/:id" element={<EditPayment />} />
      <Route path="/credit-cards" element={<CreditCards />} />
      <Route path="/vehicles" element={<Vehicles />} />
      <Route path="/projection" element={<Projection />} />
      <Route path="/remaining-balance" element={<RemainingBalances />} />
      <Route path="/account" element={<Account />} />
      <Route path="/glossary" element={<Glossary />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  )
}
