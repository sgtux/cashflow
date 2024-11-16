import styled from 'styled-components'

export const UserPicture = styled.img`
    height: 50px;
    border-radius: 50%;
    box-shadow: 1px 1px 6px 0px #777;
    &:hover {
        cursor: pointer;
    }
`

export const ToolbarMenuContainer = styled.div`
    text-decoration: none;
    color: white;
    background-color: white;
    box-shadow: 1px 1px 10px #ccc;
    position: fixed;
    top: 70px;
    right: 6px;
    width: 150px;
    height: ${props => props.$show ? '75px' : '0'};
    transition: 300ms;
    overflow: hidden;
    border-radius: 4px;
    & > button {
        text-align: center;
        width: 100%;
    }
    & > button:hover {
        background-color: #ccc;
    }
`