import { makeAutoObservable } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import { IUser } from '../models/User/User.ts'
import { runInAction } from 'mobx'
import RootStore from './rootStore.ts'

interface IAuthState {
  user: IUser | null
  isAuth: boolean
}

export default class AuthStore {
  rootStore: RootStore
  AuthState: IAuthState = {
    user: null,
    isAuth: false,
  }

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }

  resetAuthState = () => {
    this.AuthState = {
      user: null,
      isAuth: false,
    }
  }

  updateAuthState(userId: string, accessToken: string) {
    runInAction(() => {
      this.AuthState.user = { id: userId }
      this.AuthState.isAuth = true
      localStorage.setItem('token', accessToken)
    })
  }

  async register(email, password, fio, city, status) {
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      console.log('Response data:', response.data)
      this.updateAuthState(response.data.userId, response.data.accessToken)
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (error) {
      console.log(error)
      console.log(this.AuthState)
    }
  }

  async login(email: string, password: string) {
    if (localStorage.getItem('token')) {
      console.log('Пользователь уже авторизован')
      return
    }
    try {
      const response = await AuthService.login({ email, password })
      localStorage.setItem('token', response.data.accessToken)
      this.updateAuthState(response.data.userId, response.data.accessToken)
      console.log(response.data.message)
      console.log(this.AuthState, localStorage.getItem('token'))
    } catch (err) {
      console.log(err)
    }
  }

  async logout() {
    try {
      await this.RefreshTokens()
      const response = await AuthService.logout()
      localStorage.removeItem('token')
      this.resetAuthState()
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }

  async RefreshTokens() {
    try {
      const response = await AuthService.refreshTokens(localStorage.getItem('token'))
      console.log(response.data.accessToken)
      console.log(this.AuthState)
    } catch (err) {
      console.log(err)
    }
  }
}
