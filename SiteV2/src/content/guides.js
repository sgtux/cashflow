// Textos do bloco "Lição do Bê-a-bá" exibido no topo de cada página.
export const Guides = {
  home: {
    title: 'Como ler o seu mês',
    points: [
      'Entradas é tudo que entrou; Saídas é tudo que saiu. A diferença é o que sobra.',
      'Pendências são contas deste mês que você ainda não marcou como pagas.',
      'As barras de limite mostram quanto do seu orçamento já foi usado.'
    ],
    tip: 'Regra de ouro: gaste menos do que ganha. Se o saldo do mês está positivo, você está no caminho certo.'
  },
  earnings: {
    title: 'Por que registrar cada ganho?',
    points: [
      'Saber exatamente quanto entra é o primeiro passo para decidir quanto pode gastar.',
      'Use esta tela para ganhos avulsos (freela, venda, bônus).',
      'Ganhos que se repetem todo mês, como o salário, ficam melhor em "Ganhos fixos".'
    ],
    tip: 'Recebeu algo parecido no mês passado? Use o botão de copiar para lançar de novo em um clique.'
  },
  recurringEarnings: {
    title: 'Ganhos que chegam todo mês',
    points: [
      'Cadastre uma única vez o salário, a aposentadoria ou um aluguel que você recebe.',
      'Quando o dinheiro cair na conta, clique em "Recebi" para registrar o mês.',
      'Se o ganho acabou (mudou de emprego, por exemplo), defina uma data de encerramento.'
    ],
    tip: 'O valor base é uma estimativa. Se em algum mês veio diferente, ajuste no histórico.'
  },
  expenses: {
    title: 'Para onde vai o seu dinheiro?',
    points: [
      'Anote os gastos do dia a dia: mercado, lanche, transporte, farmácia, lazer.',
      'Escolher o tipo de cada gasto mostra em que você mais gasta no mês.',
      'Se pagou no cartão, informe qual: o valor entra na fatura certa.'
    ],
    tip: 'Pequenos gastos somam muito. Um cafezinho de R$ 8 por dia útil passa de R$ 170 no mês.'
  },
  recurringExpenses: {
    title: 'Suas contas fixas',
    points: [
      'Aluguel, internet, luz, celular e assinaturas se repetem todo mês.',
      'Cadastre uma vez e, quando pagar, clique em "Paguei".',
      'Enquanto não for paga, a conta aparece como pendência na tela inicial.'
    ],
    tip: 'Revise suas assinaturas de vez em quando: é comum pagar por serviços que você nem usa mais.'
  },
  payments: {
    title: 'Compras parceladas e empréstimos',
    points: [
      'Informe o valor total, a data da compra e em quantas vezes foi dividido.',
      'O sistema cria as parcelas mês a mês; depois é só marcar as que forem pagas.',
      'Se foi no cartão, as parcelas são encaixadas nas faturas certas.'
    ],
    tip: 'Antes de parcelar, some a parcela nova às que você já tem. Parcelas pequenas juntas viram uma conta grande.'
  },
  creditCards: {
    title: 'Entendendo o cartão de crédito',
    points: [
      'Fatura atual: o que você vai pagar neste mês.',
      'Crédito devedor: tudo que ainda falta pagar, incluindo parcelas futuras.',
      'Abra um cartão (seta) para ver cada compra e pagar a parte deste mês.'
    ],
    tip: 'Sempre que puder, pague a fatura inteira. Pagar só o mínimo gera juros altíssimos.'
  },
  vehicles: {
    title: 'Quanto custa o seu veículo?',
    points: [
      'Cadastre seus veículos e registre cada abastecimento com a quilometragem do painel.',
      'Com isso calculamos quantos km você roda e a média de consumo (km/l).',
      'Gastos como manutenção podem ser lançados em Despesas, com o tipo "Veículo".'
    ],
    tip: 'Se a média (km/l) cair, o veículo está gastando mais combustível para rodar a mesma distância. Pode ser pneu murcho ou revisão atrasada.'
  },
  projection: {
    title: 'Olhando para o futuro',
    points: [
      'Cada linha é um mês. Juntamos ganhos, contas fixas, parcelas e limites cadastrados.',
      'Saldo do mês = ganhos − gastos daquele mês.',
      'Saldo acumulado = a sobra que vai se somando de um mês para o outro.'
    ],
    tip: 'Viu um mês no vermelho lá na frente? Comece a guardar um pouco agora para cobrir.'
  },
  remainingBalance: {
    title: 'Fechando o caixa de cada mês',
    points: [
      'Aqui fica o resultado final de cada mês: quanto sobrou ou faltou.',
      'O valor do mês atual é uma simulação e muda conforme você lança coisas.',
      'Se o valor não bate com o seu banco, ajuste manualmente ou peça para recalcular.'
    ],
    tip: 'Compare com o saldo da sua conta bancária no fim do mês: se bater, seus registros estão em dia.'
  },
  account: {
    title: 'Defina seus limites',
    points: [
      'Limite para despesas: quanto você aceita gastar no dia a dia em um mês.',
      'Limite para combustível: quanto pretende gastar abastecendo.',
      'Esses limites aparecem como barras na tela inicial e entram na projeção.'
    ],
    tip: 'Não sabe por onde começar? Registre seus gastos por um mês e use o total como primeiro limite.'
  }
}
