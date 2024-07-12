import React from 'react'
import { Fab } from '@mui/material'
import {
    Restaurant as FoodIcon,
    LocalGroceryStore as MarketIcon,
    Fastfood as SnackIcon,
    AutoFixHigh as AestheticsIcon,
    School as EducationIcon,
    TimeToLeave as VehicleIcon,
    Pets as PetsIcon,
    Kitesurfing as LeisureIcon,
    Liquor as HardDrinkIcon,
    Celebration as PartyIcon,
    AirportShuttle as TransportIcon,
    Percent as LateFeesIcon,
    DryCleaning as ClothesIcon,
    Handyman as UtilitiesIcon,
    Sanitizer as CleaningIcon,
    CurrencyExchange as ExchangeIcon,
    More as OthersIcon
} from '@mui/icons-material'

const ExpenseTypes = {
    Food: 1,
    Market: 2,
    Snack: 3,
    Aesthetics: 4,
    Education: 5,
    Vehicle: 6,
    Pets: 7,
    Leisure: 8,
    HardDrink: 9,
    Party: 10,
    Transport: 11,
    LateFees: 12,
    Clothes: 13,
    Utilities: 14,
    Cleaning: 15,
    Exchange: 16,
    Investiment: 17,
    Others: 20
}

const ICON_FONT_SIZE = 20

const FoodFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='error'>
        <FoodIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const MarketFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='error'>
        <MarketIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const SnackFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='error'>
        <SnackIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const AestheticsFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <AestheticsIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const EducationFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='error'>
        <EducationIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const VehicleFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <VehicleIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const PetsFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='success'>
        <PetsIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const LeisureFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='primary'>
        <LeisureIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const HardDrinkFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='secondary'>
        <HardDrinkIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const PartyFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='secondary'>
        <PartyIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const TransportFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <TransportIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const LateFeesFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='error'>
        <LateFeesIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const ClothesFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <ClothesIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const UtilitiesFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <UtilitiesIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const CleaningFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='info'>
        <CleaningIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const ExchangeFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='success'>
        <ExchangeIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const InvestmentFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='success'>
        <ExchangeIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

const OthersFabIcon = () => (
    <Fab size='small' style={{ marginRight: 10, }} color='default'>
        <OthersIcon style={{ fontSize: ICON_FONT_SIZE }} />
    </Fab>
)

export const getFabIconByExpenseType = type => {
    if (type === ExpenseTypes.Food)
        return <FoodFabIcon />
    if (type === ExpenseTypes.Market)
        return <MarketFabIcon />
    if (type === ExpenseTypes.Snack)
        return <SnackFabIcon />
    if (type === ExpenseTypes.Aesthetics)
        return <AestheticsFabIcon />
    if (type === ExpenseTypes.Education)
        return <EducationFabIcon />
    if (type === ExpenseTypes.Vehicle)
        return <VehicleFabIcon />
    if (type === ExpenseTypes.Pets)
        return <PetsFabIcon />
    if (type === ExpenseTypes.Leisure)
        return <LeisureFabIcon />
    if (type === ExpenseTypes.HardDrink)
        return <HardDrinkFabIcon />
    if (type === ExpenseTypes.Party)
        return <PartyFabIcon />
    if (type === ExpenseTypes.Transport)
        return <TransportFabIcon />
    if (type === ExpenseTypes.LateFees)
        return <LateFeesFabIcon />
    if (type === ExpenseTypes.Clothes)
        return <ClothesFabIcon />
    if (type === ExpenseTypes.Utilities)
        return <UtilitiesFabIcon />
    if (type === ExpenseTypes.Cleaning)
        return <CleaningFabIcon />
    if (type === ExpenseTypes.Exchange)
        return <ExchangeFabIcon />
    if (type === ExpenseTypes.Investiment)
        return <InvestmentFabIcon />
    if (type === ExpenseTypes.Others)
        return <OthersFabIcon />
}