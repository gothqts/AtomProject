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
      const response = await UserInfoService.UpdateUserData(InputValue)
      await this.rootStore.authStore.fetchUser()
      if (response.status == 200) {
        alert('Данные успешно изменены')
      }
    } catch (error) {
      console.log(error.status, 'Ошибка изменения данных пользователя')
    }
  }

  async UpdateTel(tel: string) {
    try {
      const response = await UserInfoService.UpdateUserTel(tel)
      await this.rootStore.authStore.fetchUser()
      if (response.status == 200) {
        alert('Данные успешно изменены')
      }
    } catch (error) {
      console.log(error.status, 'Ошибка изменения телефона пользователя')
    }
  }

  async UpdatePsw(currentPsw, newPsw) {
    try {
      const response = await UserInfoService.UpdatePassword(currentPsw, newPsw)
      await this.rootStore.authStore.fetchUser()
      if (response.status == 200) {
        alert('Данные успешно изменены')
      }
    } catch (error) {
      console.log(error.status, 'Ошибка изменения пароля пользователя')
    }
  }

  async UpdateEmail(email) {
    try {
      const response = await UserInfoService.UpdateEmail(email)
      await this.rootStore.authStore.fetchUser()
      if (response.status == 200) {
        alert('Email успешно изменен')
      }
    } catch (error) {
      console.log(error.status, 'Ошибка изменения email пользователя')
    }
  }
}
