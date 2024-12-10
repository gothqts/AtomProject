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
    if (error.response.status === 401 && error.config && !error.config._isRetry) {
      try {
        localStorage.removeItem('token')
        const resp = await http.post('http://localhost:8080/api/auth/refresh', { withCredentials: true })
        localStorage.setItem('token', resp.data.accessToken)
        return http.request(originalRequest)
      } catch (error) {
        console.log('AUTH ERROR')
      }
    }
    throw error
  }
)
http.interceptors.response.use()
