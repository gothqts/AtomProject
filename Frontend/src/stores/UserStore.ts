import { makeAutoObservable } from 'mobx'
import RootStore from './rootStore.ts'
import { IUser } from '../models/User/User.ts'

export default class UserStore {
  rootStore: RootStore
  user: IUser

  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }
  setUserFields(field: string, value: string) {
    this.user = { ...this.user, [field]: value }
  }
}
