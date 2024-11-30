import AuthStore from './AuthStore.ts'
import UserStore from './UserStore.ts'
class RootStore {
  userStore: UserStore
  authStore: AuthStore
  constructor() {
    this.authStore = new AuthStore(this)
    this.userStore = new UserStore(this)
  }
}
export default RootStore
