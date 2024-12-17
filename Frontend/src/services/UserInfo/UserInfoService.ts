import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IUser } from '../../models/User/response/User.ts'
import { BaseStatusResponse } from '../../models/Auth/response/authResponse.ts'
import { UserRequest } from '../../models/User/request/userRequest.ts'

export default class UserInfoService {
  static async getUser(): Promise<AxiosResponse<IUser>> {
    return http.get(`/api/user`)
  }
  static asyncUpdateUserData(Params: UserRequest): Promise<AxiosResponse<BaseStatusResponse>> {
    return http.put('/api/user', Params)
  }
}
