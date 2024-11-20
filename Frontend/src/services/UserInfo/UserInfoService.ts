import { AxiosResponse } from 'axios'
import { http } from '../http'
import { IUser } from '../../models/User/User.ts'

export default class UserInfoService {
  static async getUserById(id: string): Promise<AxiosResponse<IUser>> {
    return http.get(`/api/user/${id}`)
  }
}
