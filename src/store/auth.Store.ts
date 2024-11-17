import { makeAutoObservable } from 'mobx'
import { http } from '../services/http'

class AuthStore {
  isAuth = false
  isAuthInProgress = false
  accessToken = ''

  constructor() {
    makeAutoObservable(this)
  }

  async register(email, password, fio, city, status) {
    this.isAuthInProgress = true
    try {
      const response = await http.post(`/api/auth/register`, { email, password, fio, city, status })
      this.accessToken = response.data.accessToken
      this.isAuth = true
    } catch (err) {
      console.log('register error')
    } finally {
      this.isAuthInProgress = false
    }
  }
}

export default new AuthStore()
