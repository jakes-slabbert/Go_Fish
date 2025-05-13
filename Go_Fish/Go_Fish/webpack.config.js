const { VueLoaderPlugin } = require('vue-loader');
var path = require('path');

module.exports = {
    entry: "./wwwroot/js/build/devextreme-vue-config.js",
    output: {
        path: path.join(__dirname, 'wwwroot/js'),
        filename: "./devextreme-vue.js",
    },
    externals: { vue: "Vue" },

    resolve: {
        extensions: [".webpack.js", ".web.js", ".ts", ".vue", ".js"],
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    },
    mode: "production",
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    esModule: true
                }
            },
            {
                test: /\.css$/,
                use: [
                    { loader: "style-loader" },
                    { loader: "css-loader" }
                ]
            },
            {
                test: /\.(eot|svg|ttf|woff|woff2)$/,
                use: "url-loader?name=[name].[ext]"
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin()
    ]
};
