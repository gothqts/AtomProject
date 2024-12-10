import { makeAutoObservable, runInAction } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import { IUser } from '../models/User/User.ts'
import RootStore from './rootStore.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import { AuthResponse } from '../models/Auth/response/authResponse.ts'
import { http } from '../services/http'

interface IAuthState {
  user: IUser | null
  isAuth: boolean
  isLoading: boolean
}

export default class AuthStore {
  rootStore: RootStore
  AuthState: IAuthState = {
    user: null,
    isAuth: false,
    isLoading: false,
  }

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }

  setLoading(bool: boolean) {
    this.AuthState.isLoading = bool
  }

  resetAuthState = () => {
    this.AuthState = {
      user: null,
      isAuth: false,
      isLoading: false,
    }
  }
  // refreshTokenInterval = null // Для хранения идентификатора интервала
  // startTokenRefreshInterval() {
  //   this.refreshTokenInterval = setInterval(async () => {
  //     await this.RefreshTokens()
  //   }, 10000) // Обновление токена каждые 8 минут
  // }

  // stopTokenRefreshInterval() {
  //   if (this.refreshTokenInterval) {
  //     clearInterval(this.refreshTokenInterval)
  //     this.refreshTokenInterval = null
  //   }
  // }

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
      await this.updateAuthState(response.data.userId, response.data.accessToken)
      await this.fetchUser()
      // this.startTokenRefreshInterval()
      console.log(response.data.message)
      console.log(this.AuthState)
    } catch (error) {
      console.log(error)
      this.resetAuthState()
    }
  }

  async login(email: string, password: string) {
    if (this.AuthState.isAuth) {
      console.log('Пользователь уже авторизован')
      return
    }
    this.setLoading(true)
    try {
      const response = await AuthService.login({ email, password })
      await this.updateAuthState(response.data.userId, response.data.accessToken)
      await this.fetchUser()
      console.log(response.data.message)
      console.log(this.AuthState, localStorage.getItem('token'))
    } catch (err) {
      console.log('login error', err)
    } finally {
      this.setLoading(false)
    }
  }

  async logout() {
    this.setLoading(true)
    try {
      const response = await AuthService.logout()
      localStorage.removeItem('token')
      this.resetAuthState()
      console.log(response.data.message)
    } catch (err) {
      console.log('logout err', err)
    } finally {
      this.setLoading(false)
    }
  }

  // async RefreshTokens() {
  //   try {
  //     const response = await AuthService.refreshTokens()
  //     console.log(response.data.accessToken)
  //     console.log(this.AuthState)
  //   } catch (err) {
  //     console.log(err)
  //   }
  // }

  async fetchUser() {
    try {
      const response = await UserInfoService.getUser()
      console.log('User data:', response.data)
      const userStore = this.rootStore.userStore
      runInAction(() => {
        this.AuthState.user = {
          ...response.data,
          fio: this.refactorFio(response.data.fio),
        }
        userStore.user = this.AuthState.user
      })
      console.log(this.AuthState.user)
    } catch (err) {
      console.log(err)
    }
  }

  async checkAuth() {
    this.setLoading(true)
    try {
      const response = await http.post('http://localhost:8080/api/auth/refresh', { withCredentials: true })
      localStorage.setItem('token', response.data.accessToken)
      await this.fetchUser()
      this.AuthState.isAuth = true
    } catch (e) {
      console.log(e.response?.data?.message)
    } finally {
      this.setLoading(false)
    }
  }
}
