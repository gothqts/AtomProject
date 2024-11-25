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
