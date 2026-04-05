// ...existing code...
const HTMLWebpackPlugin = require("html-webpack-plugin")

module.exports = (env) => {
  const proxyTarget =
    process.env.services__kong__proxy__0 ||
    process.env.services__kong__http__0 ||
    process.env.services__abcmanagementapi__https__0 ||
    process.env.services__abcmanagementapi__http__0;

  return {
    entry: "./src/index.tsx",
    devServer: {
      port: env.PORT || 4001,
      allowedHosts: "all",
      historyApiFallback: true,
      proxy: [
        {
          context: ["/api"],
          target: proxyTarget,
          pathRewrite: { "^/api": "" },
          secure: false,
          onProxyReq: (proxyReq) => {
            const apiKey = process.env.KONG_API_KEY;
            if (apiKey) {
              proxyReq.setHeader("apikey", apiKey);
            }
          },
        },
      ],
    },
    output: {
      path: `${__dirname}/dist`,
      filename: "bundle.js",
    },
    plugins: [
      new HTMLWebpackPlugin({
        template: "./public/index.html",
        favicon: "./public/favicon.ico",
      }),
    ],
    resolve: {
      extensions: [".tsx", ".ts", ".js", ".jsx"]
    },
    module: {
      rules: [
        {
          test: /\.(js|jsx)$/,
          exclude: /node_modules/,
          use: {
            loader: "babel-loader",
            options: {
              presets: [
                "@babel/preset-env",
                ["@babel/preset-react", { runtime: "automatic" }],
              ],
            },
          },
        },
        {
          test: /\.(ts|tsx)$/,
          exclude: /node_modules/,
          use: [
            {
              loader: "babel-loader",
              options: {
                presets: [
                  "@babel/preset-env",
                  "@babel/preset-typescript",
                  ["@babel/preset-react", { runtime: "automatic" }],
                ],
              },
            },
          ],
        },
        {
          test: /\.svg$/,
          use: [
            {
              loader: '@svgr/webpack',
              options: {
                svgo: true,
              },
            },
            {
              loader: 'file-loader',
              options: {
                name: 'static/media/[name].[hash].[ext]'
              }
            }
          ]
        },
        {
          test: /\.css$/,
          use: ["style-loader", "css-loader", "postcss-loader"],
        },
        {
          test: /\.scss$/,
          use: ["style-loader", "css-loader", "postcss-loader", "sass-loader"],
        },
      ],
    },
  }
}
