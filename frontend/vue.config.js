const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  devServer: {
    proxy: {
      '/api': {
        target: 'https://universalgames.buk.no',
        changeOrigin: true,
        pathRewrite: { '^/api': '/api' },
      },
    },
  },
  transpileDependencies: true,
  devServer: {
    server: {
      type: 'http'
    }
  }
})
