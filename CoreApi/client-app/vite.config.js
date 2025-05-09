import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        open: true, // Auto-opens browser on start
        proxy: {
            "/api": "http://localhost:7267",    // Proxy API requests to the backend server
        },
    },
})
