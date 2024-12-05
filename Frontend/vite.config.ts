import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import svgr from 'vite-plugin-svgr'
import * as path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), svgr()],
  server: {
    port: 5175,
  },
  resolve: {
    alias: {
      '@assets': path.resolve(__dirname, './src/assets'),
    },
  },
})
