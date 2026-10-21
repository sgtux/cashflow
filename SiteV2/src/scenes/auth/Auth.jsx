import React, { useState } from 'react'
import { Box, Card, Typography } from '@mui/material'
import TrendingUpRoundedIcon from '@mui/icons-material/TrendingUpRounded'
import ShoppingBasketRoundedIcon from '@mui/icons-material/ShoppingBasketRounded'
import QueryStatsRoundedIcon from '@mui/icons-material/QueryStatsRounded'

import { SignInScreen } from './SignIn'
import { SignUpScreen } from './SignUp'
import { Palette } from '../../theme'
import { BrandMark } from '../../components/BrandMark'

const steps = [
  { icon: TrendingUpRoundedIcon, title: 'Registre o que entra', text: 'Salário, freelas e qualquer dinheiro que chega.' },
  { icon: ShoppingBasketRoundedIcon, title: 'Anote o que sai', text: 'Gastos do dia a dia, contas fixas, cartão e parcelas.' },
  { icon: QueryStatsRoundedIcon, title: 'Veja o futuro', text: 'Saiba hoje se vai sobrar ou faltar nos próximos meses.' }
]

export function Auth() {
  const [isLogin, setIsLogin] = useState(true)

  return (
    <Box sx={{ minHeight: '100vh', display: 'grid', gridTemplateColumns: { xs: '1fr', md: '1.1fr 1fr' }, bgcolor: 'background.default' }}>
      <Box sx={{
        display: 'flex', flexDirection: 'column', justifyContent: 'center',
        px: { xs: 3, sm: 6, lg: 10 }, py: { xs: 5, md: 8 },
        color: '#fff',
        background: `linear-gradient(150deg, ${Palette.greenDark} 0%, ${Palette.green} 60%, #2C8F6B 100%)`
      }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mb: 4 }}>
          <BrandMark size={48} />
          <Typography sx={{ fontWeight: 800, fontSize: 26 }}>Bê-a-bá da Grana</Typography>
        </Box>
        <Typography variant="h3" component="h1" sx={{ fontSize: { xs: 30, sm: 40 }, lineHeight: 1.15, maxWidth: 560 }}>
          Aprenda o básico do dinheiro, um passo de cada vez.
        </Typography>
        <Typography sx={{ mt: 1.5, fontSize: 20, fontWeight: 800, color: Palette.goldLight }}>
          cada tostão conta
        </Typography>
        <Typography sx={{ mt: 2, opacity: 0.9, fontSize: 18, maxWidth: 520 }}>
          Feito para quem está dando os primeiros passos: explicamos cada termo pelo caminho.
        </Typography>
        <Box sx={{ display: 'grid', gap: 2, mt: 5, maxWidth: 520 }}>
          {steps.map((s, i) =>
            <Box key={i} sx={{ display: 'flex', gap: 2, alignItems: 'center', p: 2, borderRadius: 3, bgcolor: 'rgba(255,255,255,0.10)' }}>
              <Box sx={{ width: 32, height: 32, flexShrink: 0, borderRadius: '50%', bgcolor: Palette.goldLight, color: '#1E2A27', fontWeight: 800, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                {i + 1}
              </Box>
              <s.icon sx={{ opacity: 0.9 }} />
              <Box>
                <Typography sx={{ fontWeight: 800 }}>{s.title}</Typography>
                <Typography variant="body2" sx={{ opacity: 0.85 }}>{s.text}</Typography>
              </Box>
            </Box>
          )}
        </Box>
      </Box>
      <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center', px: 2, py: 5 }}>
        <Card sx={{ width: '100%', maxWidth: 420, p: { xs: 3, sm: 4 } }}>
          {isLogin ?
            <SignInScreen changeScene={() => setIsLogin(false)} />
            :
            <SignUpScreen changeScene={() => setIsLogin(true)} />
          }
        </Card>
      </Box>
    </Box>
  )
}
