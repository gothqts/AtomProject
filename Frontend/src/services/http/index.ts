import axios from 'axios'
import config from '../../config.ts'
import { urls } from '../../navigate/app.urls.ts'

export const http = axios.create({
  withCredentials: true,
  baseURL: config.API_URL,
})

http.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`
  return config
})

http.interceptors.response.use(
  (config) => {
    return config
  },

  async (error) => {
    const originalRequest = error.config

    if (error.config && error.response.status === 401 && !error.config._isRetry) {
      localStorage.removeItem('token')
      originalRequest._isRetry = true
      try {
        const resp = await http.post('http://localhost:8080/api/auth/refresh', {}, { withCredentials: true })
        localStorage.setItem('token', resp.data.accessToken)
        return http.request(originalRequest)
      } catch (error) {
        console.log('Не удалось обновить токен:', error)
        location.replace(urls.login)
      }
      throw error
    }
  }
)
