import React from 'react'
import { Paper, CircularProgress } from '@mui/material';

import { ContainerLoader } from './styles'

const styles = {
  legend: {
    color: '#666',
    border: 'inset 2px #ccc',
    fontSize: '30px',
    fontFamily: 'GraphikRegular',
    textTransform: 'uppercase'
  },
  paper: {
    marginTop: '20px',
    marginBottom: '50px',
    marginLeft: '20px',
    marginRight: '20px',
    padding: '10px',
    fontFamily: 'GraphikMedium'
  }
}

export function MainContainer({ title, loading, children }) {
  return (
    <Paper style={styles.paper}>
      <fieldset style={styles.legend}>
        <legend style={{ fontFamily: 'GraphikMedium' }}>{title}</legend>
        {
          loading &&
          <ContainerLoader>
            <div>
              <CircularProgress size={50} />
            </div>
          </ContainerLoader>
        }
        {children}
      </fieldset>
    </Paper>
  )
}