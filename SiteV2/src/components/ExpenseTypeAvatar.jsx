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
  Smartphone as PhoneIcon,
  SportsEsports as GamesIcon,
  VolunteerActivism as DonationIcon,
  LiveTv as StreamsIcon,
  AccountBalance as LoanIcon,
  RequestQuote as FinancingIcon,
  Computer as TechIcon,
  Lightbulb as BillsIcon,
  Home as HousingIcon,
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
  Phone: 18,
  Games: 19,
  Others: 20,
  Donation: 21,
  Streams: 22,
  Loan: 23,
  Financing: 24,
  Tech: 25,
  Bills: 26,
  Housing: 27
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
  [ExpenseTypes.Phone]: [PhoneIcon, '#2A9D8F'],
  [ExpenseTypes.Games]: [GamesIcon, '#6A4C93'],
  [ExpenseTypes.Donation]: [DonationIcon, '#E76F51'],
  [ExpenseTypes.Streams]: [StreamsIcon, '#C1121F'],
  [ExpenseTypes.Loan]: [LoanIcon, '#B5651D'],
  [ExpenseTypes.Financing]: [FinancingIcon, '#9A6324'],
  [ExpenseTypes.Tech]: [TechIcon, '#264653'],
  [ExpenseTypes.Bills]: [BillsIcon, '#E9B824'],
  [ExpenseTypes.Housing]: [HousingIcon, '#7B4B2A'],
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
