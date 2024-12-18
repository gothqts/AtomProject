import { FC } from 'react'
import styles from './MyEvents.module.css'
import { useLocation, useNavigate } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { useStores } from '../../stores/rootStoreContext.ts'

const MyEvents: FC = () => {
  const navigate = useNavigate()
  const { eventStore } = useStores()
  const handleClick = () => {
    navigate(urls.createEvent)
    eventStore.CreateEvent()
  }
  return (
    <div className={styles.container}>
      <div>
        <h1>Мои мероприятия</h1>
      </div>
      <div>
        <h1>Активность в мероприятиях</h1>
      </div>
      <button className={styles.create_btn} onClick={handleClick}>
        Создать мероприятие
      </button>
    </div>
  )
}

export default MyEvents
