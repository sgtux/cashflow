import React from 'react'

import HomeRoundedIcon from '@mui/icons-material/HomeRounded'
import PieChartRoundedIcon from '@mui/icons-material/PieChartRounded'
import QueryStatsRoundedIcon from '@mui/icons-material/QueryStatsRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'
import TrendingUpRoundedIcon from '@mui/icons-material/TrendingUpRounded'
import ShoppingCartRoundedIcon from '@mui/icons-material/ShoppingCartRounded'
import DirectionsCarRoundedIcon from '@mui/icons-material/DirectionsCarRounded'
import AutorenewRoundedIcon from '@mui/icons-material/AutorenewRounded'
import CurrencyExchangeRoundedIcon from '@mui/icons-material/CurrencyExchangeRounded'
import AccountBalanceWalletRoundedIcon from '@mui/icons-material/AccountBalanceWalletRounded'

export { getFabIconByExpenseType } from './ExpenseTypeIcon'

const iconSx = { fontSize: 26, verticalAlign: 'middle', color: 'inherit' }

export const HomeIcon = () => <HomeRoundedIcon sx={iconSx} />
export const ChartPieIcon = () => <PieChartRoundedIcon sx={iconSx} />
export const AnalysisIcon = () => <QueryStatsRoundedIcon sx={iconSx} />
export const CreditCardIcon = () => <CreditCardRoundedIcon sx={iconSx} />
export const MoneyIncomeIcon = () => <TrendingUpRoundedIcon sx={iconSx} />
export const MoneyExpenseIcon = () => <ShoppingCartRoundedIcon sx={iconSx} />
export const VehicleIcon = () => <DirectionsCarRoundedIcon sx={iconSx} />
export const RecurringExpenseIcon = () => <AutorenewRoundedIcon sx={iconSx} />
export const RecurringEarningIcon = () => <CurrencyExchangeRoundedIcon sx={iconSx} />
export const MoneyBagIcon = () => <AccountBalanceWalletRoundedIcon sx={iconSx} />
