const webpack = require('webpack')
const path = require('path')
const HtmlWebpackPlugin = require('html-webpack-plugin')
const { CleanWebpackPlugin } = require('clean-webpack-plugin')
const CopyPlugin = require('copy-webpack-plugin')

const urlApi = process.env.URL_API || ''
// Endereço público do site, usado nas tags de compartilhamento (og:image precisa de URL absoluta).
const siteUrl = (process.env.SITE_URL || '').replace(/\/$/, '')
// Por padrão o SiteV2 gera em SiteV2/dist para não sobrescrever o bundle do Site (Api/wwwroot).
const outputPath = process.env.OUTPUT_PATH || path.join(__dirname, 'dist')
// Caminho em que o site é servido. Em produção o SiteV2 fica em /v2/ do mesmo App Service do Site.
const publicPath = (process.env.PUBLIC_PATH || '/').replace(/\/?$/, '/')

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
      }
    ]
  },
  resolve: {
    extensions: ['.js', '.jsx']
  },
  output: {
    path: outputPath,
    publicPath,
    filename: 'bundle.[contenthash].js'
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: path.join(__dirname, 'public', 'index.html'),
      siteUrl,
      basePath: publicPath
    }),
    new CleanWebpackPlugin(),
    new webpack.DefinePlugin({
      API_URL: JSON.stringify(urlApi),
    }),
    new CopyPlugin({
      patterns: [
        { from: path.join(__dirname, 'public', 'images'), to: path.join(outputPath, 'images') },
        { from: path.join(__dirname, 'public', 'brand'), to: path.join(outputPath, 'brand') },
        { from: path.join(__dirname, 'public', 'manifest.webmanifest'), to: outputPath }
      ]
    })
  ],
  devServer: {
    port: 8081,
    static: './public',
    proxy: [
      {
        context: ['/api'],
        target: 'http://localhost:56666'
      }
    ]
  }
}
