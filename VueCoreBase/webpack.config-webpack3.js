const path = require("path");
const webpack = require("webpack");
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const bundleOutputDir = "./wwwroot/dist";
const { VueLoaderPlugin } = require('vue-loader')
const UglifyJSPlugin = require("uglifyjs-webpack-plugin");

module.exports = env => {
    const isDevBuild = !(env && env.prod);

    return [
        {
            stats: { modules: false },
            context: path.resolve(__dirname, ""),
            resolve: {
                alias: {
                    'vue$': 'vue/dist/vue.esm.js'
                },
                extensions: ['*', '.js', '.vue', '.json']
            },
            entry: { main: "./ClientApp/boot.js" },
			output: {
                path: path.resolve(__dirname, bundleOutputDir),
                filename: "[name].js",
                publicPath: "dist/"
            },
            module: {
                rules: [
                    {
                        test: /\.vue$/,
                        include: /ClientApp/,
                        loader: "vue-loader",
                        options: {
                            loaders: {
                                scss: ["vue-style-loader","css-loader","sass-loader"],
                                sass: ["vue-style-loader","css-loader","sass-loader?indentedSyntax"]
                            }
                        }
                    },
                    {
                        test: /\.css$/,
                        use: [
                            'vue-style-loader',
                            'css-loader'
                        ]
                    },
                    { 
						test: /\.(png|jpg|jpeg|gif|svg)$/, 
						use: "url-loader?limit=25000" 
					}
                ]
            },
            
            plugins: [
                new webpack.DefinePlugin({
                    "process.env": {
                        NODE_ENV: JSON.stringify(isDevBuild ? "development" : "production")
                    }
                }),
                new webpack.DllReferencePlugin({
                    context: __dirname,
                    manifest: require("./wwwroot/dist/vendor-manifest.json")
                })
            ].concat(
                isDevBuild
                    ? [
                        // Plugins that apply in development builds only
                        new webpack.SourceMapDevToolPlugin({
                            filename: "[file].map", // Remove this line if you prefer inline source maps
                            moduleFilenameTemplate: path.relative(
                                bundleOutputDir,
                                "[resourcePath]"
                            ) // Point sourcemap entries to the original file locations on disk
                        })
                    ]
                    : [
                        // Plugins that apply in production builds only
                        //new UglifyJSPlugin(),
                        new ExtractTextPlugin("site.css")
                    ]
            )
        }
    ];
};