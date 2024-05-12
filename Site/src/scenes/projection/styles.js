import styled from 'styled-components'

const Arrow = styled.i`
    border: solid grey;
    border-width: 0 3px 3px 0;
    display: inline-block;
    width: 2px;
    height: 2px;
    &:hover {
        cursor: pointer;
    }
`

export const ArrowLeft = styled(Arrow)`
    transform: rotate(135deg);
    -webkit-transform: rotate(135deg);
`

export const ArrowRight = styled(Arrow)`
    transform: rotate(-45deg);
    -webkit-transform: rotate(-45deg);
`

export const ArrowUp = styled(Arrow)`
    transform: rotate(-135deg);
    -webkit-transform: rotate(-135deg);
`

export const ArrowDown = styled(Arrow)`
    transform: rotate(45deg);
    -webkit-transform: rotate(45deg);
`

export const ContainerCosts = styled.div`
    display: flex;
    flex-direction: column;
    margin-top: 20px;
    padding-right: 10px; 
`

export const BoxCosts = styled.div`
    display: flex;
    justify-content: space-between;
    width: 200px;
    align-content: end;
    justify-self: end;
    align-items: end;
    align-self: end;
`

export const PaidSpan = styled.span`
    color: #fff;
    font-size: 10px;
    margin-left: 6px;
    background-color: #bbb;
    padding: 4px;
    border-radius: 6px;
`