import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import styled from '@emotion/styled'
import { tableCellClasses } from '@mui/material/TableCell'

import {
	Avatar,
	IconButton,
	Tooltip,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow
} from '@mui/material'

import {
	Delete as DeleteIcon,
	EditOutlined as EditIcon,
	CreditCardOutlined as CardIcon
} from '@mui/icons-material'

import { MainContainer, AddFloatingButton, MoneySpan } from '../../components'
import { CreditCardDetailModal } from './CreditCardEditModal/CreditCardEditModal'

import { creditCardService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'
import { NoRecordsContainer } from './styles'
import { toReal } from '../../helpers'

const StyledTableRow = styled(TableRow)(() => ({
	'&:nth-of-type(odd)': {
		backgroundColor: '#eee'
	},
	'&:last-child td, &:last-child th': {
		border: 0,
	},
}))

const StyledTableCell = styled(TableCell)(({ theme }) => ({
	[`&.${tableCellClasses.head}`]: {
		backgroundColor: '#999',
		color: theme.palette.common.white,
	},
	[`&.${tableCellClasses.body}`]: {
		fontSize: 14,
	},
	textAlign: 'center'
}))

export function CreditCards() {

	const [cards, setCards] = useState([])
	const [card, setCard] = useState(null)

	const dispatch = useDispatch()

	useEffect(() => { refresh() }, [])

	async function refresh() {
		dispatch(showGlobalLoader())
		setCard(null)
		try {
			const creditCards = await creditCardService.get()
			setCards(creditCards)
			console.log(creditCards)
		} catch (ex) {
			console.log(ex)
		} finally {
			dispatch(hideGlobalLoader())
		}
	}

	async function removeCard(id) {
		dispatch(showGlobalLoader())
		try {
			await creditCardService.remove(id)
			await refresh()
		} catch (err) {
			console.log(err)
		} finally {
			dispatch(hideGlobalLoader())
		}
	}

	async function saveCard(c) {
		dispatch(showGlobalLoader())
		try {
			if (c.id)
				await creditCardService.update(c)
			else
				await creditCardService.create(c)
			await refresh()
		} catch (err) {
			console.log(err)
		} finally {
			dispatch(hideGlobalLoader())
		}
	}

	return (
		<MainContainer title="Cartões de crédito">
			{cards.length > 0 ?
				<Table sx={{ minWidth: 700 }}>
					<TableHead>
						<TableRow>
							<StyledTableCell />
							<StyledTableCell>Descrição</StyledTableCell>
							<StyledTableCell>Saldo Devedor</StyledTableCell>
							<StyledTableCell>Dia Fechamento</StyledTableCell>
							<StyledTableCell>Dia Vencimento</StyledTableCell>
							<StyledTableCell>Ações</StyledTableCell>
						</TableRow>
					</TableHead>

					<TableBody>
						{cards.map(p =>
							<StyledTableRow key={p.id}>
								<StyledTableCell>
									<Avatar>
										<CardIcon />
									</Avatar>
								</StyledTableCell>
								<StyledTableCell>{p.name}</StyledTableCell>
								<StyledTableCell><MoneySpan $gain={p.outstandingDebt <= 0}>{toReal(p.outstandingDebt)}</MoneySpan></StyledTableCell>
								<StyledTableCell>{p.invoiceClosingDay}</StyledTableCell>
								<StyledTableCell>{p.invoiceDueDay}</StyledTableCell>
								<StyledTableCell onClick={e => e.stopPropagation()}>
									<Tooltip title="Editar este cartão">
										<IconButton color="primary" aria-label="Edit"
											onClick={() => setCard(p)}>
											<EditIcon />
										</IconButton>
									</Tooltip>
									<Tooltip title="Remover este cartão">
										<IconButton color="secondary" aria-label="Delete"
											onClick={() => removeCard(p.id)}>
											<DeleteIcon />
										</IconButton>
									</Tooltip>
								</StyledTableCell>
							</StyledTableRow>
						)}
					</TableBody>
				</Table>
				:
				<NoRecordsContainer>
					<span>Você ainda não adicionou cartões.</span>
				</NoRecordsContainer>
			}
			<CreditCardDetailModal onSave={c => saveCard(c)} onClose={() => refresh()} card={card} />
			<AddFloatingButton onClick={() => setCard({})} />
		</MainContainer>
	)
}