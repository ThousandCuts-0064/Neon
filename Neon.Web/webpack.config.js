const path = require("path");
const glob = require("glob");
const TerserPlugin = require("terser-webpack-plugin");

module.exports = [
	{
		entry: glob.sync("./wwwroot/ts/*.ts")
			.concat(glob.sync("./wwwroot/tsx/*.tsx"))
			.reduce((all, file) => {
				all[path.parse(file).name] = file;

				return all;
			}, {}),
		module: {
			rules: [
				{
					test: /\.tsx?$/,
					use: "ts-loader",
					exclude: /node_modules/
				},
				{
					test: /\.svg$/,
					use: "html-loader"
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
		}
	}
];