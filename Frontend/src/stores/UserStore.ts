import { makeAutoObservable } from 'mobx'
import RootStore from './rootStore.ts'
import { IUser } from '../models/User/response/User.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import { UserRequest } from '../models/User/request/userRequest.ts'

export default class UserStore {
  rootStore: RootStore
  user: IUser

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }
  setUserFields(field: string, value: string) {
    this.user = { ...this.user, [field]: value }
  }
  async UpdateData(InputValue: UserRequest) {
    try {
      const response = await UserInfoService.asyncUpdateUserData(InputValue)
      await this.rootStore.authStore.fetchUser()
      if (response.status == 200) {
        alert('Данные успешно изменены')
      }
    } catch (error) {
      console.log(error.status, 'Ошибка изменения данных пользователя')
    }
  }
}
