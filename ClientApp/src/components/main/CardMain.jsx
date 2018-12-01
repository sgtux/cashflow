import React from 'react'
import { Paper } from '@material-ui/core';

const styles = {
  legend: {
    color: '#666',
    border: 'inset 2px #ccc',
    fontSize: '30px',
    fontFamily: 'Roboto Helvetica Arial sans-serif',
    textTransform: 'uppercase'
  },
  paper: {
    marginTop: '50px',
    marginLeft: '20px',
    marginRight: '20px',
    padding: '10px'
  }
}

export default (props) => {
  return (
    <Paper style={styles.paper}>
      <fieldset style={styles.legend}>
        <legend>{props.title}</legend>
        {props.children}
      </fieldset>
    </Paper>
  )
}