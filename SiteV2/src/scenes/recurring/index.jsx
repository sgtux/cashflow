import React from 'react'
import EventRepeatRoundedIcon from '@mui/icons-material/EventRepeatRounded'
import ReceiptLongRoundedIcon from '@mui/icons-material/ReceiptLongRounded'

import { RecurringConfigs } from './config'
import { RecurringList } from './RecurringList'
import { RecurringEdit } from './RecurringEdit'

export const RecurringEarnings = () => <RecurringList config={RecurringConfigs.earning} icon={<EventRepeatRoundedIcon />} />
export const EditRecurringEarning = () => <RecurringEdit config={RecurringConfigs.earning} icon={<EventRepeatRoundedIcon />} />
export const RecurringExpenses = () => <RecurringList config={RecurringConfigs.expense} icon={<ReceiptLongRoundedIcon />} />
export const EditRecurringExpense = () => <RecurringEdit config={RecurringConfigs.expense} icon={<ReceiptLongRoundedIcon />} />
