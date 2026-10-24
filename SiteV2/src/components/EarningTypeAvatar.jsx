import React from 'react'
import { Avatar } from '@mui/material'
import {
  Work as SalaryIcon,
  LaptopMac as FreelanceIcon,
  EmojiEvents as BonusIcon,
  Elderly as RetirementIcon,
  House as RentIcon,
  ShowChart as DividendsIcon,
  Savings as InterestIcon,
  Storefront as BusinessIcon,
  Copyright as RoyaltiesIcon,
  Sell as SaleIcon,
  CardGiftcard as GiftIcon,
  Replay as RefundIcon,
  More as OthersIcon
} from '@mui/icons-material'

// Ids dos tipos de ganho (mesmos da API).
export const EarningTypes = {
  Salary: 1,
  Freelance: 2,
  Bonus: 3,
  Retirement: 4,
  Rent: 5,
  Dividends: 6,
  Interest: 7,
  Business: 8,
  Royalties: 9,
  Sale: 10,
  Gift: 11,
  Refund: 12,
  Others: 20
}

const config = {
  [EarningTypes.Salary]: [SalaryIcon, '#2F6FB2'],
  [EarningTypes.Freelance]: [FreelanceIcon, '#4A6FA5'],
  [EarningTypes.Bonus]: [BonusIcon, '#F4A259'],
  [EarningTypes.Retirement]: [RetirementIcon, '#8D6E63'],
  [EarningTypes.Rent]: [RentIcon, '#3D9970'],
  [EarningTypes.Dividends]: [DividendsIcon, '#1F7A5A'],
  [EarningTypes.Interest]: [InterestIcon, '#00A6A6'],
  [EarningTypes.Business]: [BusinessIcon, '#7E6BC4'],
  [EarningTypes.Royalties]: [RoyaltiesIcon, '#B565A7'],
  [EarningTypes.Sale]: [SaleIcon, '#E07A5F'],
  [EarningTypes.Gift]: [GiftIcon, '#E85D75'],
  [EarningTypes.Refund]: [RefundIcon, '#5B7C99'],
  [EarningTypes.Others]: [OthersIcon, '#8A8F98']
}

export function EarningTypeAvatar({ type, size = 40 }) {
  const [Icon, color] = config[type] || config[EarningTypes.Others]
  return (
    <Avatar sx={{ bgcolor: color, color: '#fff', width: size, height: size }}>
      <Icon sx={{ fontSize: size * 0.55 }} />
    </Avatar>
  )
}
