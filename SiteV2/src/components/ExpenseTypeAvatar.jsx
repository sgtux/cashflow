import React from 'react'
import { Avatar } from '@mui/material'
import {
  Restaurant as FoodIcon,
  LocalGroceryStore as MarketIcon,
  Fastfood as SnackIcon,
  AutoFixHigh as AestheticsIcon,
  School as EducationIcon,
  TimeToLeave as VehicleIcon,
  Pets as PetsIcon,
  Kitesurfing as LeisureIcon,
  Liquor as HardDrinkIcon,
  Celebration as PartyIcon,
  AirportShuttle as TransportIcon,
  Percent as LateFeesIcon,
  DryCleaning as ClothesIcon,
  Handyman as UtilitiesIcon,
  Sanitizer as CleaningIcon,
  CurrencyExchange as ExchangeIcon,
  Savings as InvestmentIcon,
  More as OthersIcon
} from '@mui/icons-material'

// Ids dos tipos de despesa doméstica (mesmos da API).
export const ExpenseTypes = {
  Food: 1,
  Market: 2,
  Snack: 3,
  Aesthetics: 4,
  Education: 5,
  Vehicle: 6,
  Pets: 7,
  Leisure: 8,
  HardDrink: 9,
  Party: 10,
  Transport: 11,
  LateFees: 12,
  Clothes: 13,
  Utilities: 14,
  Cleaning: 15,
  Exchange: 16,
  Investiment: 17,
  Others: 20
}

const config = {
  [ExpenseTypes.Food]: [FoodIcon, '#E07A5F'],
  [ExpenseTypes.Market]: [MarketIcon, '#3D9970'],
  [ExpenseTypes.Snack]: [SnackIcon, '#F4A259'],
  [ExpenseTypes.Aesthetics]: [AestheticsIcon, '#B565A7'],
  [ExpenseTypes.Education]: [EducationIcon, '#2F6FB2'],
  [ExpenseTypes.Vehicle]: [VehicleIcon, '#5B7C99'],
  [ExpenseTypes.Pets]: [PetsIcon, '#8D6E63'],
  [ExpenseTypes.Leisure]: [LeisureIcon, '#00A6A6'],
  [ExpenseTypes.HardDrink]: [HardDrinkIcon, '#9C4F96'],
  [ExpenseTypes.Party]: [PartyIcon, '#E85D75'],
  [ExpenseTypes.Transport]: [TransportIcon, '#4A6FA5'],
  [ExpenseTypes.LateFees]: [LateFeesIcon, '#D1495B'],
  [ExpenseTypes.Clothes]: [ClothesIcon, '#7E6BC4'],
  [ExpenseTypes.Utilities]: [UtilitiesIcon, '#6C757D'],
  [ExpenseTypes.Cleaning]: [CleaningIcon, '#4FB0C6'],
  [ExpenseTypes.Exchange]: [ExchangeIcon, '#1F7A5A'],
  [ExpenseTypes.Investiment]: [InvestmentIcon, '#1F7A5A'],
  [ExpenseTypes.Others]: [OthersIcon, '#8A8F98']
}

export const getExpenseTypeColor = type => (config[type] || config[ExpenseTypes.Others])[1]

export function ExpenseTypeAvatar({ type, size = 40 }) {
  const [Icon, color] = config[type] || config[ExpenseTypes.Others]
  return (
    <Avatar sx={{ bgcolor: color, color: '#fff', width: size, height: size }}>
      <Icon sx={{ fontSize: size * 0.55 }} />
    </Avatar>
  )
}
