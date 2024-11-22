import axios from 'axios'
import config from '../../config.ts'
import { AuthResponse } from '../../models/Auth/response/authResponse.ts'

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
    if (error.response.status == 401 && error.config && !error.config._isRetry) {
      originalRequest._isRetry = true
      try {
        const response = await axios.post(`${config.API_URL}/api/auth/refresh`, { withCredentials: true })
        localStorage.setItem('token', response.data.accessToken)
        return http.request(originalRequest)
      } catch (e) {
        console.log('НЕ АВТОРИЗОВАН')
      }
    }
    throw error
  }
)
