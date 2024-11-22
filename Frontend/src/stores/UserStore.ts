import { makeAutoObservable, observable } from 'mobx'
import RootStore from './rootStore.ts'

export default class UserStore {
  rootStore: RootStore
  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }
  get userId() {
    return this.rootStore.authStore.AuthState.user?.id || ''
  }

  // Метод для получения полного объекта user (если надо)
  get user() {
    return this.rootStore.authStore.AuthState.user
  }
}
