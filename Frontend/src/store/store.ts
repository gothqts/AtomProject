import { makeAutoObservable } from 'mobx'
import AuthService from '../services/Auth/AuthService.ts'

class Store {
  userID: string = ''
  isAuth: boolean = false

  constructor() {
    makeAutoObservable(this)
  }

  setAuth(bool: boolean) {
    this.isAuth = bool
  }

  setUserID(userId: string) {
    this.userID = userId
  }

  async login(email: string, password: string) {
    try {
      const response = await AuthService.login({ email, password })
      console.log(response)
      localStorage.setItem('token', response.data.accessToken)
      this.setAuth(true)
      this.setUserID(response.data.userId)
    } catch (err) {
      console.log(err)
    }
  }
  async register(email, password, fio, city, status) {
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      console.log(response)
      localStorage.setItem('token', response.data.accessToken)
      this.setAuth(true)
      this.setUserID(response.data.userId)
    } catch (err) {
      console.error('Registration Error:', err)
    }
  }

  async logout(email: string, password: string) {
    try {
      const response = await AuthService.logout()
      localStorage.removeItem('token')
      this.setAuth(false)
      this.setUserID('')
    } catch (err) {
      console.log(err)
    }
  }
}
export default new Store()