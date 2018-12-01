import React from 'react'

import { questionService } from '../../services'
import {
  TableBody,
  TableRow,
  Table,
  TableCell,
  TableHead,
  TablePagination,
  Paper,
  Button
} from '@material-ui/core'
import IconDownload from '@material-ui/icons/Archive'

import Stars from '../../components/question/Stars'

export default class SharedQuestion extends React.Component {

  constructor(props) {
    super(props)
    this.state = {
      questions: [],
      emptyRows: 0,
      rowsPerPage: 5,
      page: 5
    }
  }

  componentDidMount() {
    questionService.getOthers().then(res => this.setState({ questions: res }))
  }

  getShared(id) {
    questionService.getShared(id).then(() => {
      this.setState({ questions: this.state.questions.filter(p => p.id !== id) })
    })
  }

  render() {
    const { questions } = this.state
    return (
      <div>
        <h2>Shared Questions</h2>
        <br />
        <Paper style={{ marginLeft: '20px', marginRight: '20px' }}>
          <div>
            <Table aria-labelledby="tableTitle">
              <TableHead>
                <TableRow>
                  <TableCell style={{ color: '#AAA', fontWeight: 'bold', textAlign: 'center' }}>Área</TableCell>
                  <TableCell style={{ textAlign: 'center' }}>Dificuldade</TableCell>
                  <TableCell style={{ textAlign: 'center' }}>Descrição</TableCell>
                  <TableCell style={{ textAlign: 'center' }}>Respostas</TableCell>
                  <TableCell style={{ textAlign: 'center' }}>Adquirir</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {questions
                  .map(n => (
                    <TableRow
                      tabIndex={-1}
                      key={n.id}>
                      <TableCell style={{ textAlign: 'center' }} component="th" scope="row" padding="none">
                        {n.area}
                      </TableCell>
                      <TableCell style={{ textAlign: 'center' }}>
                        <Stars filled={4} />
                      </TableCell>
                      <TableCell style={{ textAlign: 'center' }} numeric>{n.description}</TableCell>
                      <TableCell style={{ textAlign: 'center' }} numeric>{n.answers.length}</TableCell>
                      <TableCell style={{ textAlign: 'center' }}>
                        <Button onClick={() => this.getShared(n.id)} variant="fab">
                          <IconDownload color="primary" />
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))}
                {this.state.emptyRows > 0 && (
                  <TableRow style={{ height: 49 * this.state.emptyRows }}>
                    <TableCell colSpan={6} />
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </div>
          <TablePagination
            component="div"
            count={this.state.questions.length}
            rowsPerPage={this.state.rowsPerPage}
            page={this.state.page}
            backIconButtonProps={{
              'aria-label': 'Previous Page',
            }}
            nextIconButtonProps={{
              'aria-label': 'Next Page',
            }}
            onChangePage={(e, p) => this.setState({ page: p })}
            onChangeRowsPerPage={e => this.setState({ rowsPerPage: e.target.value })}
          />
        </Paper>
      </div>
    )
  }
}