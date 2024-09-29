const path = require("path");
const glob = require("glob");
const TerserPlugin = require("terser-webpack-plugin");

module.exports = [
    {
        stats: {
            errorDetails: true, // npm run build --stats-error-details
        },
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
                    use: {
                        loader: "babel-loader",
                        options: {
                            presets: [
                                "@babel/preset-typescript",
                                "babel-preset-solid"
                            ]
                        }
                    },
                    exclude: /node_modules/
                },
                {
                    test: /\.svg$/,
                    use: "html-loader"
                },
            ]
        },
        resolve: {
            modules: [
                path.resolve(__dirname, "wwwroot/ts/modules"),
                path.resolve(__dirname, "wwwroot/svg"),
                "node_modules"
            ],
            extensions: [".tsx", ".ts", ".svg", ".js"], // Order matters!
        },
        output: {
            filename: "[name].js",
            path: path.resolve(__dirname, "wwwroot/js")
        },
        optimization: {
            minimize: true,
            minimizer: [
                new TerserPlugin({
                    parallel: 6,
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