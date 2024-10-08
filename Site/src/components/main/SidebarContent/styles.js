import styled from 'styled-components'

export const MenuItemContainer = styled.div`
    text-align: center;
    margin: 0;
    display: flex;
    justify-content: center;
    padding: 10px;
    transition: 400ms;
    color: ${({ selected }) => selected ? 'rgb(75, 147, 114)' : '#fff'};
    background-color: ${({ selected }) => selected ? '#fff' : '#0000'};
    &:hover {
        cursor: pointer;
        background-color: ${({ selected }) => selected ? '#fff' : '#fff3'};
        transition: 400ms;
    }
    & > span {
        font-family: 'PermanentMarker';
        font-size: 20px;
        text-transform: uppercase;
        margin-left: 10px;
    }
`