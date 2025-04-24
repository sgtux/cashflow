const webpack = require('webpack')
const path = require('path')
const ProgressBar = require('progress-bar-webpack-plugin')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const CopyPlugin = require('copy-webpack-plugin')

const urlApi = process.env.URL_API || ''
const outputPath = path.join(__dirname, '..', 'Api', 'wwwroot')

module.exports = {
  entry: './src/App.jsx',
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: ['babel-loader']
      }, {
        test: /\.css$/,
        use: ['style-loader', 'css-loader']
      }, {
        test: /\.(eot|svg|ttf|woff|woff2)$/,
        use: 'file-loader?name=public/fonts/[name].[ext]'
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.jsx', '.ttf']
  },
  output: {
    path: outputPath,
    publicPath: '/',
    filename: `bundle${(new Date()).getTime()}.js`
  },
  plugins: [
    new ProgressBar(),
    new HtmlWebpackPlugin({
      template: path.join(__dirname, 'public', 'index.html'),
      title: 'Cashflow',
      favicon: path.join(__dirname, 'public', 'favicon.ico')
    }),
    new CleanWebpackPlugin(),
    new webpack.DefinePlugin({
      API_URL: JSON.stringify(urlApi),
    }),
    new CopyPlugin({
      patterns: [
        { from: path.join(__dirname, 'public', 'fonts'), to: path.join(outputPath, 'fonts') },
        { from: path.join(__dirname, 'public', 'images'), to: path.join(outputPath, 'images') }
      ]
    })
  ],
  devServer: {
    static: './public',
    proxy: [
      {
        context: ['/api'],
        target: 'http://localhost:5000'
      }
    ]
  }
}