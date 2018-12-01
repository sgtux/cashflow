import React from 'react'
import { StarBorderOutlined, Star } from '@material-ui/icons/'

const color = '#ffb950'

const styles = {
  default: {
    color: color
  },
  clickable: {
    color: color,
    cursor: 'pointer'
  }
}

export default (props) => {

  const starClicked = (index) => {
    if (props.onClick)
      props.onClick(index)
  }

  return (
    <div style={props.style}>
      <div>
        <label style={{ fontSize: 12 }}>{props.label}</label>
      </div>
      {[1, 2, 3, 4, 5].map(n => n <= props.filled ?
        <Star key={n}
          onClick={() => starClicked(n)}
          style={props.onClick ? styles.clickable : styles.default} />
        :
        <StarBorderOutlined key={n}
          onClick={() => starClicked(n)}
          style={props.onClick ? styles.clickable : styles.default} />)}
    </div>
  )
}