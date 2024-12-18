import { FC } from 'react'
import styles from './MyEvents.module.css'
import EventsMeddleLine from '../../assets/images/myEventsGraph.svg?react'
import UpcomingEvent from '../../shared/MyEvent/UpcomingEvent/index.tsx'
import PastEvent from '../../shared/MyEvent/PastEvent/index.tsx'

const MyEvents: FC = () => {
  return (
    <div className={styles.container}>

      <div className={styles.section_div}>
        <div className={styles.section_title}>Мои мероприятия</div>

        <div className={styles.upcoming}>
          <div className={styles.upcoming_title}>Предстоящие</div>
          <div className={styles.events_list}>
            <UpcomingEvent title='Новый год'/>
            <UpcomingEvent title='Пасха'/>
            <PastEvent title='Прошедшее мероприятие'/>
          </div>
          <button className={styles.show_more}>Показать ещё</button>
        </div>

        <div>

        </div>

      </div>

      <EventsMeddleLine />

      <div className={styles.section_div}>

        <div className={styles.left_section_titles}>
          <div className={styles.section_title}>Активность в мероприятиях</div>
          <div className={styles.upcoming_title}>Предстоящие</div>
        </div>

      </div>
    </div>
  )
}

export default MyEvents
