import axios from 'axios'
import config from '../../config.ts'

export const http = axios.create({
  withCredentials: true,
  baseURL: config.API_URL,
})

http.interceptors.request.use((config) => {
  config.headers.Authorization = `Bearer ${localStorage.getItem('token')}`
  return config
})

http.interceptors.response.use(
  (response) => {
    return response
  },
  async (error) => {
    const originalRequest = error.config
    originalRequest._isRetry = false
    if (error.status === 401) {
      localStorage.removeItem('token')
      if (!originalRequest._isRetry) {
        originalRequest._isRetry = true
        try {
          const resp = await http.post('http://localhost:8080/api/auth/refresh')
          localStorage.setItem('token', resp.data.accessToken)
          return http.request(originalRequest)
        } catch (refreshError) {
          console.log('Не авторизован, ошибка авторизации')
        }
      }
    }

    throw error
  }
)
