import { makeAutoObservable, observable, runInAction } from 'mobx'
import RootStore from './rootStore.ts'
import { useStores } from './rootStoreContext.ts'
import UserInfoService from '../services/UserInfo/UserInfoService.ts'
import rootStore from './rootStore.ts'
import { IUser } from '../models/User/User.ts'

export default class UserStore {
  rootStore: RootStore
  user
  constructor(rootStore: RootStore) {
    makeAutoObservable(this, { rootStore: false })
    this.rootStore = rootStore
  }
}
