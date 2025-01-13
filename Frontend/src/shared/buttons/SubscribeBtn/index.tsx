import styles from './SubscriveBtn.module.css'
import { useStores } from '../../../stores/rootStoreContext.ts'
import { urls } from '../../../navigate/app.urls.ts'

const SubscribeBtn: React.FC = ({ eventId, setModalActive }) => {
  const { eventStore } = useStores()
  const handleClick = async () => {
    if (localStorage.getItem('token')) {
      await eventStore.FetchSignUpWindows(eventId)
      setModalActive(true)
    } else {
      location.replace(urls.login)
    }
  }

  return (
    <button className={styles.btn} onClick={handleClick}>
      Подать заявку
    </button>
  )
}

export default SubscribeBtn
