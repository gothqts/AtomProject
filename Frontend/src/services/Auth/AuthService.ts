import { AxiosResponse } from 'axios'
import { http } from '../http'
import { RegisterRequest, LoginRequest } from '../../models/Auth/request/authRequest.ts'
import { AuthResponse, BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'

export default class AuthService {
  static async register(regParams: RegisterRequest): Promise<AxiosResponse<AuthResponse>> {
    return http.post<AuthResponse>('/api/auth/register', regParams)
  }

  static async login(loginParams: LoginRequest): Promise<AxiosResponse<AuthResponse>> {
    return http.post<AuthResponse>('/api/auth/login', loginParams)
  }

  static async logout(): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.post<BaseStatusResponse>('/api/auth/logout')
  }
}
