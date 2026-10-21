import HomeRoundedIcon from '@mui/icons-material/HomeRounded'
import TrendingUpRoundedIcon from '@mui/icons-material/TrendingUpRounded'
import EventRepeatRoundedIcon from '@mui/icons-material/EventRepeatRounded'
import ShoppingBasketRoundedIcon from '@mui/icons-material/ShoppingBasketRounded'
import ReceiptLongRoundedIcon from '@mui/icons-material/ReceiptLongRounded'
import ViewWeekRoundedIcon from '@mui/icons-material/ViewWeekRounded'
import CreditCardRoundedIcon from '@mui/icons-material/CreditCardRounded'
import DirectionsCarRoundedIcon from '@mui/icons-material/DirectionsCarRounded'
import QueryStatsRoundedIcon from '@mui/icons-material/QueryStatsRounded'
import SavingsRoundedIcon from '@mui/icons-material/SavingsRounded'
import MenuBookRoundedIcon from '@mui/icons-material/MenuBookRounded'

// O menu segue a ordem de aprendizado: primeiro o que entra, depois o que sai, por fim planejar.
export const NavigationGroups = [
  {
    items: [
      { to: '/', label: 'Início', hint: 'Resumo do seu mês', icon: HomeRoundedIcon }
    ]
  },
  {
    step: 1,
    title: 'O que entra',
    items: [
      { to: '/earnings', label: 'Ganhos', hint: 'Freelas, vendas, bônus', icon: TrendingUpRoundedIcon },
      { to: '/recurring-earnings', label: 'Ganhos fixos', hint: 'Salário e afins', icon: EventRepeatRoundedIcon }
    ]
  },
  {
    step: 2,
    title: 'O que sai',
    items: [
      { to: '/household-expenses', label: 'Despesas do dia a dia', hint: 'Mercado, lanche, transporte', icon: ShoppingBasketRoundedIcon },
      { to: '/recurring-expenses', label: 'Contas fixas', hint: 'Aluguel, internet, luz', icon: ReceiptLongRoundedIcon },
      { to: '/payments', label: 'Parcelamentos', hint: 'Compras em várias vezes', icon: ViewWeekRoundedIcon },
      { to: '/credit-cards', label: 'Cartões e faturas', hint: 'O que vence no cartão', icon: CreditCardRoundedIcon },
      { to: '/vehicles', label: 'Veículos', hint: 'Combustível e consumo', icon: DirectionsCarRoundedIcon }
    ]
  },
  {
    step: 3,
    title: 'Planejar',
    items: [
      { to: '/projection', label: 'Projeção', hint: 'Os próximos meses', icon: QueryStatsRoundedIcon },
      { to: '/remaining-balance', label: 'Fechamento do mês', hint: 'Quanto sobrou em cada mês', icon: SavingsRoundedIcon }
    ]
  },
  {
    title: 'Aprender',
    items: [
      { to: '/glossary', label: 'Glossário', hint: 'Termos explicados', icon: MenuBookRoundedIcon }
    ]
  }
]
