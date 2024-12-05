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
// http.interceptors.response.use(
//   (config) => {
//     return config
//   },
//   async (error) => {
//     // предотвращаем зацикленный запрос, добавляя свойство _isRetry
//     const originalRequest = { ...error.config }
//     originalRequest._isRetry = true
//     if (
//       // проверим, что ошибка именно из-за невалидного accessToken
//       error.response.status === 401 &&
//       // проверим, что запрос не повторный
//       error.config &&
//       !error.config._isRetry
//     ) {
//       try {
//         const resp = await http.post('http://localhost:8080/api/auth/refresh', { withCredentials: true })
//         localStorage.setItem('token', resp.data.accessToken)
//         // переотправляем запрос с обновленным accessToken
//         return http.request(originalRequest)
//       } catch (error) {
//         console.log('AUTH ERROR')
//       }
//     }
//     // на случай, если возникла другая ошибка (не связанная с авторизацией)
//     throw error
//   }
// )
