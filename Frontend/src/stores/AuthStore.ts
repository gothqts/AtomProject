import { makeAutoObservable, runInAction } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import { IUser } from '../models/User/User.ts'
import RootStore from './rootStore.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'

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

  setUser(user: IUser) {
    this.AuthState.user = user
  }
  resetAuthState = () => {
    this.AuthState = {
      user: null,
      isAuth: false,
    }
  }
  refreshTokenInterval = null // Для хранения идентификатора интервала
  startTokenRefreshInterval() {
    this.refreshTokenInterval = setInterval(async () => {
      await this.RefreshTokens()
    }, 80000) // Обновление токена каждые 10 минут
  }

  stopTokenRefreshInterval() {
    if (this.refreshTokenInterval) {
      clearInterval(this.refreshTokenInterval)
      this.refreshTokenInterval = null
    }
  }
  refactorFio(fio) {
    if (fio && fio.length > 0) {
      const refactoredFio = fio.split(' ')
      return refactoredFio.length >= 2 ? refactoredFio.slice(0, 2).join(' ') : fio
    }
    return fio
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
      await this.updateAuthState(response.data.userId, response.data.accessToken)
      await this.fetchUser()
      this.startTokenRefreshInterval()
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
      await this.updateAuthState(response.data.userId, response.data.accessToken)
      await this.fetchUser()
      this.startTokenRefreshInterval()
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
      this.stopTokenRefreshInterval()
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

  async fetchUser() {
    try {
      const response = await UserInfoService.getUser()
      console.log('User data:', response.data)
      runInAction(() => {
        this.AuthState.user = {
          ...response.data,
          fio: this.refactorFio(response.data.fio),
        }
      })
      console.log(this.AuthState.user)
    } catch (err) {
      console.log(err)
    }
  }
}
