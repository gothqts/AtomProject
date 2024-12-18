import { makeAutoObservable, runInAction } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'
import { IUser } from '../models/User/response/User.ts'
import RootStore from './rootStore.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import { urls } from '../navigate/app.urls.ts'

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

  updateAuthState(userId: string, accessToken: string) {
    const expiresIn = 720 // Время жизни токена в секундах (12 минут)
    const expiryTime = Date.now() + expiresIn * 1000 // Переводим в миллисекунды

    runInAction(() => {
      this.AuthState.user = {
        id: userId,
        phone: '',
        email: '',
        fio: '',
        city: '',
        roleTitle: '',
        description: '',
        status: '',
        avatarImage: '',
        createdEvents: null,
        CurrentPassword: '',
        NewPassword: '',
      }
      this.AuthState.isAuth = true
      localStorage.setItem('token', accessToken)
      localStorage.setItem('tokenExpiry', expiryTime.toString()) // Сохраняем время жизни токена
    })
  }

  async register(email, password, fio, city, status) {
    this.setLoading(true)
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      await this.updateAuthState(response.data.userId, response.data.accessToken)
      await this.fetchUser()
      location.replace(urls.events)
      console.log(response.data.message)
      console.log(this.AuthState, localStorage.getItem('token'))
    } catch (error) {
      console.log('register error', error)
    } finally {
      this.setLoading(false)
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
      location.replace(urls.events)
      console.log(response)
      console.log(this.AuthState, localStorage.getItem('token'))
    } catch (err) {
      if (err.status === 400) {
        alert('Неверный логин или пароль')
      }
    } finally {
      this.setLoading(false)
    }
  }

  async logout() {
    this.setLoading(true)
    try {
      const response = await AuthService.logout()
      localStorage.removeItem('token')
      localStorage.removeItem('tokenExpiry')
      this.resetAuthState()
      console.log(response.data.message)
      location.replace(urls.login)
    } catch (err) {
      console.log('logout err', err)
    } finally {
      this.setLoading(false)
    }
  }

  async fetchUser() {
    try {
      const response = await UserInfoService.getUser()
      const userStore = this.rootStore.userStore
      runInAction(() => {
        this.AuthState.user = {
          ...response.data,
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
      const tokenExpiry = localStorage.getItem('tokenExpiry')
      if (tokenExpiry && Date.now() > Number(tokenExpiry)) {
        await this.logout()
        location.replace(urls.login)
        return
      }
      await this.fetchUser()
      this.AuthState.isAuth = true
    } catch (error) {
      console.log(error, 'Ошибка при аутентификации')
    } finally {
      this.setLoading(false)
    }
  }
  // async checkAuth() {
  //   this.setLoading(true)
  //   try {
  //     const response = await http.post('http://localhost:8080/api/auth/refresh', { withCredentials: true })
  //     console.log(response)
  //     localStorage.setItem('token', response.data.accessToken)
  //     await this.fetchUser()
  //     this.AuthState.isAuth = true
  //     console.log(this.AuthState.isAuth)
  //   } catch (e) {
  //     console.log(e.response?.data?.message)
  //   } finally {
  //     this.setLoading(false)
  //   }
  // }
}
