import { AxiosResponse } from 'axios'
import { http } from '../http'
import { LoginRequest, RegisterRequestParams } from '../../models/Auth/request/authRequest.ts'
import { AuthResponse, BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'

export default class AuthService {
  static async register(regParams: RegisterRequestParams): Promise<AxiosResponse<AuthResponse>> {
    return http.post('/api/auth/register', regParams)
  }

  static async login(loginParams: LoginRequest): Promise<AxiosResponse<AuthResponse>> {
    return http.post<AuthResponse>('/api/auth/login', loginParams)
  }

  static async logout(token: string): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.post<BaseStatusResponse>(
      '/api/auth/logout',
      {},
      {
        headers: {
          Authorization: `Bearer ${token}`, // Добавляем токен в заголовок
        },
      }
    )
  }
}
