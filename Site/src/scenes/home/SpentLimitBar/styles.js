import styled from '@emotion/styled'
import { Colors } from '../../../helpers/themes'

export const Container = styled.div`
    margin: 20px;
    text-align: center;
`

export const ContainerBar = styled.div`    
    display: flex;
    justify-content: right;
    height: 20px;
    border-radius: 10px;
    width: 90%;
    margin: 0 auto;
    margin-bottom: 10;
    overflow: hidden;
    background-image: linear-gradient( to right, ${Colors.AppGreen} 60%, ${Colors.AppYellow} 80%, ${Colors.AppRed} 95%, ${Colors.AppRedDark});
`

export const FillBar = styled.div`
    height: 100%;
    width: ${p => `${100 - p.percent}%`};
    background-color: #ccc;
`

export const Label = styled.span`
    font-size: 16px;
    font-weight: bold;
    color: #555;
    font-family: "Roboto","Helvetica","Arial",sans-serif;
`