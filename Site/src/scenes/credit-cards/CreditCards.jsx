import React, { useState, useEffect } from 'react'
import { useDispatch } from 'react-redux'

import {
	Paper,
	List,
	ListItem,
	ListItemAvatar,
	Avatar,
	ListItemSecondaryAction,
	IconButton,
	ListItemText,
	Tooltip,
	Button
} from '@mui/material'

import {
	Delete as DeleteIcon,
	EditOutlined as EditIcon,
	CreditCardOutlined as CardIcon
} from '@mui/icons-material'

import { MainContainer } from '../../components/main'
import { CreditCardDetailModal } from './CreditCardEditModal/CreditCardEditModal'

import { creditCardService } from '../../services'
import { showGlobalLoader, hideGlobalLoader } from '../../store/actions'

const styles = {
	noRecords: {
		textTransform: 'none',
		fontSize: '18px',
		textAlign: 'center'
	},
	divNewCard: {
		textTransform: 'none',
		fontSize: '18px',
		textAlign: 'center',
		marginTop: '20px'
	},
	errorMessage: {
		color: 'red'
	}
}

export function CreditCards() {

	const [cards, setCards] = useState([])
	const [card, setCard] = useState(null)

	const dispatch = useDispatch()

	useEffect(() => { refresh() }, [])

	async function refresh() {
		dispatch(showGlobalLoader())
		setCard(null)
		try {
			const res = await creditCardService.get()
			setCards(res)
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
				<Paper>
					<List dense={true}>
						{cards.map(p =>
							<ListItem key={p.id}>
								<ListItemAvatar>
									<Avatar>
										<CardIcon />
									</Avatar>
								</ListItemAvatar>
								<ListItemText
									style={{ width: 160 }}
									primary={p.name}
									secondary=""
								/>
								<ListItemText
									style={{ width: 160, textAlign: 'center' }}
									primary="Fechamento da fatura"
									secondary={p.invoiceClosingDay}
								/>
								<ListItemText
									style={{ width: 160, textAlign: 'center' }}
									primary="Vencimento da fatura"
									secondary={p.invoiceDueDay}
								/>
								<ListItemSecondaryAction>
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
								</ListItemSecondaryAction>
							</ListItem>
						)}
					</List>
				</Paper>
				:
				<div style={styles.noRecords}>
					<span>Você ainda não adicionou cartões.</span>
				</div>
			}
			<div style={styles.divNewCard}>
				<Button variant="text" color="primary" onClick={() => setCard({})}>
					Adicionar Cartão
				</Button>
			</div>
			<CreditCardDetailModal onSave={c => saveCard(c)} onClose={() => refresh()} card={card} />
		</MainContainer>
	)
}