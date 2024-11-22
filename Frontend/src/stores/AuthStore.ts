import { makeAutoObservable } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import { IUser } from '../models/User/User.ts'
import { runInAction } from 'mobx'
import RootStore from './rootStore.ts'
import { Root } from 'react-dom/client'

interface IAuthState {
  accessToken: string
  user: IUser | null
  isAuth: boolean
}

export default class AuthStore {
  rootStore: RootStore
  AuthState: IAuthState = {
    accessToken: '',
    user: null,
    isAuth: false,
  }

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }

  resetAuthState = () => {
    this.AuthState = {
      accessToken: '',
      user: null,
      isAuth: false,
    }
  }

  updateAuthState(accessToken: string, userId: string) {
    runInAction(() => {
      this.AuthState.accessToken = accessToken
      this.AuthState.user = { id: userId }
      this.AuthState.isAuth = true
    })
  }

  async register(email, password, fio, city, status) {
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      console.log('Response data:', response.data)
      this.updateAuthState(response.data.accessToken, response.data.userId)
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (error) {
      console.log(error)
      console.log(this.AuthState)
    }
  }

  async login(email: string, password: string) {
    if (this.AuthState.isAuth) {
      console.log('Пользователь уже авторизован')
      return
    }
    try {
      const response = await AuthService.login({ email, password })
      this.updateAuthState(response.data.accessToken, response.data.userId)
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }

  async logout() {
    try {
      await this.RefreshTokens()
      const response = await AuthService.logout(this.AuthState.accessToken)
      this.resetAuthState()
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }

  async RefreshTokens() {
    try {
      const response = await AuthService.refreshTokens(this.AuthState.accessToken)
      this.AuthState.accessToken = response.data.accessToken
      console.log(response.data.accessToken)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }
}
