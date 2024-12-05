import AuthStore from './AuthStore.ts'
import UserStore from './UserStore.ts'
import EventStore from './EventStore.ts'
class RootStore {
  userStore: UserStore
  authStore: AuthStore
  eventStore: EventStore
  constructor() {
    this.authStore = new AuthStore(this)
    this.userStore = new UserStore(this)
    this.eventStore = new EventStore(this)
  }
}
export default RootStore
