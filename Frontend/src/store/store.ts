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
    accessToken:
      'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjcyZTFmYTM0LTg5ZDQtNGNhZi05ODBiLTcwMTMxNDc0YWY0YyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlVzZXIiLCJqdGkiOiI3ZmJjMWU0OC0wYWI4LTRlNTctOTYzOS1lODZkYWZhM2M5Y2MiLCJleHAiOjE3MzIxMjM3MDksImlzcyI6IlVuYmVhdGFibGVCb29raW5nU2VydmVyIiwiYXVkIjoiVW5iZWF0YWJsZUJvb2tpbmdDbGllbnQifQ.K_x18UeFj85uh1slg6ngWedhVXrKbjI07rnNMvjfcdM',
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
