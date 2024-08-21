const path = require("path");
const glob = require("glob");
const TerserPlugin = require("terser-webpack-plugin");

module.exports = {
	entry: glob.sync('./wwwroot/ts/*.js').reduce((obj, el) => {
		obj[path.parse(el).name] = el;

		return obj;
	}, {}),
	module: {
		rules: [
			{
				test: /\.ts$/,
				use: "ts-loader",
				exclude: /node_modules/
			},
			{
				test: /\.html$/,
				use: "raw-loader"
			}
		]
	},
	output: {
		filename: "[name].js",
		path: path.resolve(__dirname, "wwwroot/js")
	},
	optimization: {
		minimize: true,
		minimizer: [
			new TerserPlugin({
				parallel: true,
				extractComments: false,
				terserOptions: {
					format: {
						comments: false,
					},
				},
			}),
		],
	},
};