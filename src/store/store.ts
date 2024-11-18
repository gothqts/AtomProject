import { makeAutoObservable } from 'mobx'
import { IUser } from '../models/User/User.ts'
import AuthService from '../services/Auth/AuthService.ts'
import { RegisterRequest } from '../models/Auth/request/authRequest.ts'

export default class Store {
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
  async register(email: string, password: string, fio: string, status: string, city: string) {
    try {
      const response = await AuthService.register({ email, password, fio, status, city })
      console.log(response)
      localStorage.setItem('token', response.data.accessToken)
      this.setAuth(true)
      this.setUserID(response.data.userId)
    } catch (err) {
      console.log(err)
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
