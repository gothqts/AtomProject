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
  (config) => {
    return config
  },
  async (error) => {
    const originalRequest = { ...error.config }
    originalRequest._isRetry = true
    if (
      error.response.status === 401 &&
      // проверим, что запрос не повторный
      error.config &&
      !error.config._isRetry
    ) {
      try {
        // запрос на обновление токенов
        const resp = await http.get('/api/auth/refresh')
        // сохраняем новый accessToken в localStorage
        localStorage.setItem('token', resp.data.accessToken)
        // переотправляем запрос с обновленным accessToken
        return http.request(originalRequest)
      } catch (error) {
        console.log('AUTH ERROR')
      }
    }
    throw error
  }
)
