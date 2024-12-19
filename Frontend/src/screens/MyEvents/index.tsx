import { FC } from 'react'
import styles from './MyEvents.module.css'
import EventsMeddleLine from '../../assets/images/myEventsGraph.svg?react'
import UpcomingEvent from '../../shared/MyEvent/UpcomingEvent/index.tsx'
import PastEvent from '../../shared/MyEvent/PastEvent/index.tsx'
import { useNavigate } from 'react-router-dom'
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
      <div className={styles.section_div}>
        <div className={styles.section_title}>Мои мероприятия</div>

        <div className={styles.upcoming}>
          <div className={styles.upcoming_title}>Предстоящие</div>
          <div className={styles.events_list}>
            <UpcomingEvent title='Новый год' />
            <UpcomingEvent title='Пасха' />
            <PastEvent title='Прошедшее мероприятие' />
          </div>
          <button className={styles.show_more}>Показать ещё</button>
        </div>

        <div></div>
      </div>

      <EventsMeddleLine />

      <div className={styles.section_div}>
        <div className={styles.left_section_titles}>
          <div className={styles.section_title}>Активность в мероприятиях</div>
          <div className={styles.upcoming_title}>Предстоящие</div>
        </div>
      </div>
      <button className={styles.create_btn} onClick={handleClick}>
        Создать мероприятие
      </button>
    </div>
  )
}

export default MyEvents
