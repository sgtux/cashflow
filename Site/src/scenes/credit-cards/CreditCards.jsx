import React, { useState, useEffect } from 'react'

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

import { creditCardService } from '../../services/index'

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

	const [loading, setLoading] = useState(false)
	const [cards, setCards] = useState([])
	const [card, setCard] = useState(null)

	useEffect(() => refresh(), [])

	function refresh() {
		setLoading(true)
		setCard(null)
		creditCardService.get().then(res => {
			setLoading(false)
			setCards(res)
		})
	}

	function removeCard(id) {
		creditCardService.remove(id)
			.then(() => refresh())
			.catch(() => setLoading(false))
	}

	function saveCard(c) {
		setLoading(true)
		if (c.id)
			creditCardService.update(c)
				.then(() => refresh())
				.catch(() => setLoading(false))
		else
			creditCardService.create(c)
				.then(() => refresh())
				.catch(() => setLoading(false))
	}

	return (
		<MainContainer title="Cartões de crédito" loading={loading}>
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