import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'
import styled from '@emotion/styled'
import { tableCellClasses } from '@mui/material/TableCell'

import {
	Box,
	IconButton,
	Tooltip,
	Table,
	TableBody,
	TableCell,
	TableHead,
	TableRow,
	Collapse,
	Card
} from '@mui/material'

import {
	Delete as DeleteIcon,
	EditOutlined as EditIcon,
	KeyboardArrowDown,
	KeyboardArrowUp
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

const StyledSubTableCell = styled(TableCell)(({ theme }) => ({
	[`&.${tableCellClasses.head}`]: {
		backgroundColor: '#999',
		color: theme.palette.common.white,
	},
	[`&.${tableCellClasses.body}`]: {
		fontSize: 12,
	},
}))

export function CreditCards() {

	const [cards, setCards] = useState([])
	const [card, setCard] = useState(null)
	const [open, setOpen] = useState({})

	const dispatch = useDispatch()

	useEffect(() => { refresh() }, [])

	async function refresh() {
		dispatch(showGlobalLoader())
		setCard(null)
		try {
			const creditCards = await creditCardService.get()
			setCards(creditCards)
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
							<StyledTableCell>Crédito Devedor</StyledTableCell>
							<StyledTableCell>Dia Fechamento Fatura</StyledTableCell>
							<StyledTableCell>Dia Vencimento Fatura</StyledTableCell>
							<StyledTableCell>Ações</StyledTableCell>
						</TableRow>
					</TableHead>

					<TableBody>
						{cards.map((p, i) =>
							<React.Fragment key={p.id}>
								<TableRow key={p.id}>
									<StyledTableCell>
										<IconButton
											aria-label="expand row"
											size="small"
											onClick={() => setOpen({ ...open, [i]: !open[i] })}>
											{open[i] ? <KeyboardArrowUp /> : <KeyboardArrowDown />}
										</IconButton>
									</StyledTableCell>
									<StyledTableCell>{p.name}</StyledTableCell>
									<StyledTableCell><MoneySpan $gain={p.outstandingDebt <= 0}>{toReal(p.outstandingDebtTotal)}</MoneySpan></StyledTableCell>
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
								</TableRow>
								<StyledTableRow>
									<StyledTableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={6}>
										<Collapse in={open[i]} timeout="auto">
											<Box sx={{ margin: 1 }}>
												<Card>
													<Table size='small'>
														<TableHead>
															<StyledTableRow>
																<StyledSubTableCell align='left'>Descrição</StyledSubTableCell>
																<StyledTableCell>Parcelas</StyledTableCell>
																<StyledSubTableCell align='right'>Valor Pendente</StyledSubTableCell>
																<StyledSubTableCell align='right'>Valor Total</StyledSubTableCell>
															</StyledTableRow>
														</TableHead>
														<TableBody>
															{p.items.map((h, j) =>
																<StyledTableRow key={j}>
																	<StyledSubTableCell align="left">{toReal(h.description)}</StyledSubTableCell>
																	<StyledTableCell>{h.plots || '-'}</StyledTableCell>
																	<StyledSubTableCell align="right">
																		<MoneySpan style={{ fontSize: 12 }}>{toReal(h.outstandingDebt)}</MoneySpan>
																	</StyledSubTableCell>
																	<StyledSubTableCell align="right">{h.total ? toReal(h.total) : '-'}</StyledSubTableCell>
																</StyledTableRow>
															)}
														</TableBody>
													</Table>
												</Card>
											</Box>
										</Collapse>
									</StyledTableCell>
								</StyledTableRow>
							</React.Fragment>
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