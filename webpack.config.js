var path = require('path');

const distPath = path.join(__dirname, 'wwwroot');

const modulesPath = path.join(__dirname, 'node_modules');

module.exports = {
  entry: './ClientApp/App.js',
  output: {
    publicPath: "/js/",
    path: path.join(__dirname, '/wwwroot/js/'),
    filename: 'bundle.js'
  },
  devServer: {
    inline: true,
    contentBase: distPath,
    port: 3333
  },
  module: {
    loaders: [{
      test: /\.js$/,
      exclude: modulesPath,
      loader: 'babel-loader',
      query: {
        presets: ['es2015', 'react']
      }
    }]
  }
}