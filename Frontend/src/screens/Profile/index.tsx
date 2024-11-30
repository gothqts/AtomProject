import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'

const Profile = () => {
  const { userStore } = useStores()
  const { user } = userStore
  return (
    <div>
      <div>OLOL</div>
    </div>
  )
}

export default Profile
