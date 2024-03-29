const webpack = require('webpack');
const path = require('path');

const environmentName = process.env.NODE_ENV || 'development';

const isDevelpoment = () => {
    return environmentName !== 'production';
};

module.exports = {
    mode: isDevelpoment() ? 'development' : environmentName,
    devtool: isDevelpoment() ? 'inline-source-map' : 'hidden-source-map',
    resolve: {
        extensions: ['.ts', '.tsx', '.js', '.jsx'],
    },
    entry: {
        fileApp: path.resolve('src/FileApp/index'),
    },
    module: {
        rules: [
            {
                test: /\.m?js/,
                resolve: {
                    fullySpecified: false,
                },
            },
            {
                test: /\.tsx?$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                    },
                    'ts-loader',
                ],
            },
            {
                test: /\.css$/,
                use: [{ loader: 'style-loader' }, { loader: 'css-loader' }],
            },
        ],
    },
    plugins: [
        new webpack.LoaderOptionsPlugin({ dev: isDevelpoment() }),
        new webpack.EnvironmentPlugin({
            NODE_ENV: process.env.NODE_ENV || 'development',
        }),
    ],
    output: {
        filename: '[name]/[name].bundle.js',
        path: path.join(path.resolve(__dirname, '..'), 'wwwroot', 'bundles'),
        publicPath: '/bundles/',
        // clean: true,
    },
};
