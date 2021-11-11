import React, { useState, useEffect } from 'react'
import DatePicker from 'react-datepicker'
import ptBr from 'date-fns/locale/pt-BR'
import { Link, useParams, useHistory } from 'react-router-dom'

import {
    Paper,
    List,
    ListItem,
    ListItemSecondaryAction,
    IconButton,
    ListItemText,
    Tooltip,
    Button
} from '@material-ui/core'

import {
    Delete as DeleteIcon,
    Edit as EditIcon
} from '@material-ui/icons'

import { MainContainer } from '../../../components/main'
import IconTextInput from '../../../components/main/IconTextInput'
import { InputMoney } from '../../../components/inputs'
import { toReal, fromReal } from '../../../helpers'

import { dailyExpensesService } from '../../../services'

export function EditDailyExpenses() {

    const [id, setId] = useState(0)
    const [loading, setLoading] = useState(false)
    const [shopName, setShopName] = useState('')
    const [date, setDate] = useState('')
    const [itemName, setItemName] = useState('')
    const [itemPrice, setItemPrice] = useState('')
    const [itemAmount, setItemAmount] = useState('')
    const [items, setItems] = useState([])
    const [itemIsValid, setItemIsValid] = useState(false)
    const [formIsValid, setFormIsValid] = useState(false)

    const params = useParams()
    const history = useHistory()

    useEffect(() => {
        if (Number(params.id) > 0) {
            setLoading(true)
            dailyExpensesService.get(params.id)
                .then(res => {
                    if (!res) {
                        history.push('/daily-expenses')
                        return
                    }
                    setId(res.id)
                    setShopName(res.shopName)
                    setDate(new Date(res.date))
                    setItems(res.items)
                })
                .catch(err => console.log(err))
                .finally(() => setLoading(false))
        }
    }, [])

    useEffect(() => {
        setItemIsValid(itemName && fromReal(itemPrice) > 0 && itemAmount)
    }, [itemName, itemPrice, itemAmount])

    useEffect(() => {
        setFormIsValid(shopName && date && items.length)
    }, [shopName, date, items])

    function save() {
        setLoading(true)
        dailyExpensesService.save({ id, shopName, date, items })
            .then(() => history.push('/daily-expenses'))
            .catch(err => console.log(err))
            .finally(() => setLoading(false))
    }

    function addItem() {
        let temp = items.concat([{ itemName, price: fromReal(itemPrice), amount: parseInt(itemAmount) }])
        let i = 1
        temp.forEach(p => p.id = i++)
        setItems(temp)
        setItemName('')
        setItemPrice('')
        setItemAmount('')
    }

    function removeItem(id) {
        let temp = items.filter(p => p.id !== id)
        let i = 1
        temp.forEach(p => p.id = i++)
        setItems(temp)
    }

    function editItem(item) {
        let temp = items.filter(p => p.id !== item.id)
        setItems(temp)
        setItemName(item.itemName)
        setItemPrice(toReal(item.price))
        setItemAmount(item.amount)
    }

    return (
        <MainContainer title="Despesas Diárias" loading={loading}>
            <IconTextInput
                label="Estabelecimento"
                value={shopName}
                onChange={e => setShopName(e.value)}
            />
            <div style={{ marginTop: 20 }}>
                <span style={{ fontSize: 16, marginRight: 10 }}>Data:</span>
                <DatePicker onChange={e => setDate(e)}
                    dateFormat="dd/MM/yyyy" locale={ptBr} selected={date} />
            </div>
            <Paper style={{ marginTop: 20 }}>
                <fieldset>
                    <legend style={{ fontSize: 14, color: 'gray' }}>Itens:</legend>

                    <IconTextInput
                        label="Descrição"
                        value={itemName}
                        onChange={e => setItemName(e.value)}
                    />
                    <IconTextInput
                        style={{ marginLeft: 10 }}
                        label="Quantidade"
                        value={itemAmount}
                        onChange={e => setItemAmount((e.value || '').replace(/[^0-9]/g, ''))}
                    />
                    <div style={{ marginTop: 20 }}>
                        <span style={{ fontSize: 16 }}>Valor:</span>
                        <InputMoney
                            onChangeText={e => setItemPrice(e)}
                            kind="money"
                            value={itemPrice} />

                        <Button
                            style={{ marginLeft: 10 }}
                            onClick={() => addItem()}
                            color="primary"
                            disabled={!itemIsValid}
                            variant="contained" autoFocus>Adicionar Item</Button>
                    </div>
                    <List dense={true}>
                        {items.map(p =>
                            <ListItem key={p.id}>
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={p.itemName}
                                />
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={toReal(p.price)}
                                />
                                <ListItemText
                                    style={{ width: '100px' }}
                                    secondary={p.amount}
                                />
                                <ListItemSecondaryAction>
                                    <Tooltip title="Editar este item">
                                        <IconButton color="primary" aria-label="Edit"
                                            onClick={() => editItem(p)}>
                                            <EditIcon />
                                        </IconButton>
                                    </Tooltip>
                                    <Tooltip title="Remover este item">
                                        <IconButton color="secondary" aria-label="Delete"
                                            onClick={() => removeItem(p.id)}>
                                            <DeleteIcon />
                                        </IconButton>
                                    </Tooltip>
                                </ListItemSecondaryAction>
                            </ListItem>
                        )}
                    </List>
                </fieldset>
            </Paper>
            <div style={{ display: 'flex', justifyContent: 'end' }}>
                <Link to="/daily-expenses">
                    <Button onClick={() => { }} variant="contained" autoFocus>Lista de Despesas</Button>
                </Link>

                <Button
                    style={{ marginLeft: 10 }}
                    disabled={loading || !formIsValid}
                    onClick={() => save()}
                    color="primary"
                    variant="contained" autoFocus>salvar</Button>
            </div>
        </MainContainer>
    )
}