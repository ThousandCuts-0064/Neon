const path = require('path');

module.exports = {
  entry: './wwwroot/ts/gameplay.ts',
  module: {
    rules: [
      {
        test: /\.ts$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
    ],
  },
  resolve: {
    extensions: ['.ts', '.js'],
  },
  output: {
    filename: 'gameplay.js',
    path: path.resolve(__dirname, 'wwwroot/js'),
  },
};