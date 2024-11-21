import { makeAutoObservable } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import { IUser } from '../models/User/User.ts'
import { runInAction } from 'mobx'
import { http } from '../services/http'

interface IAuthState {
  accessToken: string
  user: IUser | null
  isAuth: boolean
}

export default class Store {
  AuthState: IAuthState = {
    accessToken: '',
    user: null,
    isAuth: false,
  }

  constructor() {
    makeAutoObservable(this)
  }

  resetAuthState() {
    this.AuthState = {
      accessToken: '',
      user: null,
      isAuth: false,
    }
  }

  async register(email, password, fio, city, status) {
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      console.log(response)
      runInAction(() => {
        this.AuthState.accessToken = response.data.accessToken
        this.AuthState.user = {
          id: response.data.userId,
        }
        this.AuthState.isAuth = true
      })
      console.log(this.AuthState)
    } catch (error) {
      if (!error.response) {
        console.error('Network error:', error)
      } else {
        console.error('Error response:', error.response)
      }
    }
  }

  async login(email: string, password: string) {
    if (this.AuthState.isAuth) {
      console.log('Пользователь уже авторизован')
      return
    }

    try {
      const response = await AuthService.login({ email, password })
      runInAction(() => {
        this.AuthState.accessToken = response.data.accessToken
        this.AuthState.user = {
          id: response.data.userId,
        }
        this.AuthState.isAuth = true
      })
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }

  async logout() {
    try {
      const response = await AuthService.logout(this.AuthState.accessToken) // Используем токен из AuthState
      this.resetAuthState()
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }

  async RefreshTokens() {
    try {
      const response = await http.post(`/api/auth/refresh`)
      this.AuthState.accessToken = response.data.accessToken
    } catch (err) {
      console.log(err)
    }
  }
}
