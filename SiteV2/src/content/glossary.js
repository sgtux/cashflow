// Dicionário usado nas dicas (ícone "?") e na página de Glossário.
// Linguagem simples, sem jargão, pensando em quem está organizando as finanças pela primeira vez.
export const Glossary = {
  ganho: {
    term: 'Ganho (provento)',
    short: 'Todo dinheiro que entra para você: salário, um freela, uma venda, um presente em dinheiro.',
    example: 'Recebeu R$ 300 por um trabalho extra no dia 12? Isso é um ganho do dia 12.'
  },
  ganhoRecorrente: {
    term: 'Ganho recorrente',
    short: 'Um ganho que se repete todo mês com valor parecido, como o salário ou uma aposentadoria.',
    example: 'Cadastre o salário uma vez e, a cada mês, só confirme que recebeu.'
  },
  despesa: {
    term: 'Despesa',
    short: 'Todo dinheiro que sai: mercado, lanche, farmácia, transporte, lazer.',
    example: 'Gastou R$ 45 na padaria hoje? Registre como despesa do tipo "Lanche".'
  },
  despesaRecorrente: {
    term: 'Despesa recorrente (conta fixa)',
    short: 'Uma conta que chega todo mês: aluguel, internet, luz, academia, streaming.',
    example: 'Cadastre a internet de R$ 100 uma vez e marque como paga a cada mês.'
  },
  parcelamento: {
    term: 'Parcelamento',
    short: 'Uma compra (ou empréstimo) dividida em várias parcelas que vencem mês a mês.',
    example: 'Uma TV de R$ 2.400 em 12x vira 12 parcelas de R$ 200, uma por mês.'
  },
  parcela: {
    term: 'Parcela',
    short: 'Cada pedacinho de um parcelamento. Cada parcela tem um valor e uma data de vencimento.',
    example: 'Na compra em 10x, a "parcela 3/10" é a terceira das dez.'
  },
  cartao: {
    term: 'Cartão de crédito',
    short: 'Você compra agora e paga depois, tudo junto, em uma conta mensal chamada fatura.',
    example: 'O cartão não é dinheiro extra: é dinheiro do seu mês que vem.'
  },
  fatura: {
    term: 'Fatura',
    short: 'A "conta" mensal do cartão: soma tudo o que você comprou nele naquele período.',
    example: 'Se a fatura de outubro deu R$ 800, esse é o valor a pagar no vencimento.'
  },
  fechamento: {
    term: 'Dia de fechamento',
    short: 'Dia em que o cartão "fecha a conta" do mês. Compras feitas depois desse dia vão para a fatura seguinte.',
    example: 'Fechamento dia 5: uma compra no dia 6 só será cobrada na fatura do mês seguinte.'
  },
  vencimento: {
    term: 'Dia de vencimento',
    short: 'Último dia para pagar sem juros. Atrasar gera multa e juros altos.',
    example: 'Vencimento dia 15: pague até o dia 15 para não ter juros.'
  },
  creditoDevedor: {
    term: 'Crédito devedor',
    short: 'Tudo que você ainda deve no cartão, somando as parcelas futuras que já estão comprometidas.',
    example: 'Comprou um celular em 10x e pagou 2? As 8 restantes ainda contam aqui.'
  },
  isentar: {
    term: 'Isentar parcela',
    short: 'Marcar que uma parcela não precisa ser paga (foi perdoada, cancelada ou estornada).',
    example: 'A loja cancelou a última parcela? Marque-a como isenta.'
  },
  limite: {
    term: 'Limite de gastos (orçamento)',
    short: 'Um teto que você mesmo define para um tipo de gasto no mês. Ajuda a perceber quando está exagerando.',
    example: 'Limite de R$ 1.200 para despesas do dia a dia: a barra fica amarela perto de 80%.'
  },
  pendencia: {
    term: 'Pendência',
    short: 'Conta do mês que ainda não foi marcada como paga.',
    example: 'A parcela do notebook vence este mês e ainda não foi paga? Ela aparece em Pendências.'
  },
  entradas: {
    term: 'Entradas',
    short: 'Soma de tudo que entrou (ou vai entrar) no mês.',
    example: 'Salário + freela = entradas do mês.'
  },
  saidas: {
    term: 'Saídas',
    short: 'Soma de tudo que saiu (ou vai sair) no mês: despesas, contas fixas, parcelas e combustível.',
    example: 'Quando as saídas ficam maiores que as entradas, o mês fecha no vermelho.'
  },
  liquido: {
    term: 'Saldo do mês (líquido)',
    short: 'Entradas menos saídas. Positivo: sobrou dinheiro. Negativo: gastou mais do que ganhou.',
    example: 'Entrou R$ 3.000 e saiu R$ 2.700? O saldo do mês é R$ 300.'
  },
  projecao: {
    term: 'Projeção',
    short: 'Uma previsão de como seu dinheiro vai estar nos próximos meses, com base no que você já cadastrou.',
    example: 'Se a projeção mostra saldo negativo em março, dá tempo de se planejar agora.'
  },
  acumulado: {
    term: 'Saldo acumulado',
    short: 'O que sobra somando mês após mês. A sobra de um mês "passa" para o próximo.',
    example: 'Sobrou R$ 200 em janeiro e R$ 300 em fevereiro? O acumulado é R$ 500.'
  },
  saldoRemanescente: {
    term: 'Saldo remanescente',
    short: 'Quanto realmente sobrou (ou faltou) ao final de cada mês, já contando a sobra do mês anterior.',
    example: 'É como "fechar o caixa" do mês e anotar o resultado.'
  },
  devedor: {
    term: 'Devedor',
    short: 'Quando o mês terminou com saldo negativo, ou seja, faltou dinheiro.',
    example: 'Fechou o mês devendo R$ 150 no cheque especial? Marque como devedor.'
  },
  residual: {
    term: 'Valor residual (estimado)',
    short: 'Quando você ainda não lançou gastos, a projeção usa o seu limite como estimativa.',
    example: 'Limite de combustível R$ 400 e nada lançado? A projeção considera R$ 400.'
  },
  abastecimento: {
    term: 'Abastecimento',
    short: 'Cada vez que você coloca combustível no veículo. Com a quilometragem, calculamos o consumo.',
    example: 'Abasteceu R$ 200 a R$ 5,80 o litro com o painel marcando 45.300 km.'
  },
  kmPorLitro: {
    term: 'Média (km/l)',
    short: 'Quantos quilômetros o veículo anda com 1 litro. Quanto maior, mais econômico.',
    example: '12 km/l significa que 10 litros levam você por cerca de 120 km.'
  },
  emergencia: {
    term: 'Reserva de emergência',
    short: 'Dinheiro guardado para imprevistos (saúde, conserto, desemprego). Uma meta comum é juntar de 3 a 6 meses de gastos.',
    example: 'Gasta R$ 2.000 por mês? Uma boa reserva fica entre R$ 6.000 e R$ 12.000.'
  }
}

export const GlossaryGroups = [
  { title: 'O básico', ids: ['ganho', 'despesa', 'entradas', 'saidas', 'liquido'] },
  { title: 'Contas que se repetem', ids: ['ganhoRecorrente', 'despesaRecorrente', 'pendencia'] },
  { title: 'Cartão e parcelas', ids: ['cartao', 'fatura', 'fechamento', 'vencimento', 'creditoDevedor', 'parcelamento', 'parcela', 'isentar'] },
  { title: 'Planejamento', ids: ['limite', 'projecao', 'acumulado', 'saldoRemanescente', 'devedor', 'residual', 'emergencia'] },
  { title: 'Veículos', ids: ['abastecimento', 'kmPorLitro'] }
]
