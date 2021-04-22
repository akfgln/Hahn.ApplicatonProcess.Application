﻿const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

const bundleFileName = 'bundle';
const dirName = 'wwwroot/dist';

module.exports = (env, argv) => {
    return {
        mode: argv.mode === "production" ? "production" : "development",
        entry: ['./src/app.ts'],
        devtool: 'inline-source-map',
        output: {
            filename: bundleFileName + '.js',
            path: path.resolve(__dirname, dirName)
        },
        module: {
            rules: [
                {
                test: /\.tsx?$/,
                use: [
                    "ts-loader"
                ],
                exclude: /node_modules/
            },
                {
                    test: /\.s[c|a]ss$/,
                    use:
                        [
                            'style-loader',
                            {
                                loader: MiniCssExtractPlugin.loader,
                                options: {
                                    esModule: false,
                                },
                            },
                            'css-loader',
                            "postcss-loader",
                            'sass-loader'
                        ]
                }
            ]
        },
        plugins: [
            new CleanWebpackPlugin(),
            new MiniCssExtractPlugin({
                filename: bundleFileName + '.css'
            })
        ]
    };
};