import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IUser } from '../../models/User/response/User.ts'
import { BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'
import { IEmail, UserRequest } from '../../models/User/request/userRequest.ts'

export default class UserInfoService {
  static async getUser(): Promise<AxiosResponse<IUser>> {
    return http.get(`/api/user`)
  }
  static async UpdateUserData(Params: UserRequest): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/user', Params)
  }
  static async UpdateUserTel(tel: string): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/user/phone', { phone: tel })
  }
  static async UpdateEmail(email: IEmail): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/user/email', { email: email })
  }
  static async UpdatePassword(currentPassword, newPassword): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/user/password', { currentPassword: currentPassword, newPassword: newPassword })
  }
}
