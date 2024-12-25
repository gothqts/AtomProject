import { FC, useEffect } from 'react'
import styles from './MyEvents.module.css'
import EventItem from './EventItem/index.tsx'
import { useNavigate } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { useStores } from '../../stores/rootStoreContext.ts'

const MyEvents: FC = () => {
  const navigate = useNavigate()
  const { eventStore } = useStores()
  const myEvents = eventStore.myEvents
  const myPastEvents = eventStore.myPastEvents

  // Загрузка мероприятий
  useEffect(() => {
    eventStore.FetchMyEvents()
  }, [eventStore])

  const handleClick = () => {
    navigate(urls.createEvent)
    eventStore.CreateEvent()
  }

  return (
    <div className={styles.container}>
      <div className={styles.section}>
        <h1>Мои мероприятия</h1>
        <h2>Предстоящие</h2>
        {myEvents.map((event) => (
          <EventItem key={event.id} title={event.title} past={false} />
        ))}
        <button className={styles.show_more}>Показать ещё</button>
        <h2>Прошедшие</h2>
        {myPastEvents.map((event) => (
          <EventItem key={event.id} title={event.title} past={true} />
        ))}
        <button className={styles.show_more}>Показать ещё</button>
        <button className={styles.create_btn} onClick={handleClick}>
          создать
        </button>
      </div>
      <div className={styles.section}>
        <h1 className={styles.header_right}>Активность в мероприятиях</h1>
        <h2 className={styles.header_right}>Предстоящие</h2>
        <EventItem title='Мероприятие' past={false} />
        <EventItem title='Мероприятие' past={false} />
        <button className={styles.show_more}>Показать ещё</button>
        <h2>Прошедшие</h2>
        <EventItem title='Мероприятие' past={true} />
        <EventItem title='Мероприятие' past={true} />
        <button className={styles.show_more}>Показать ещё</button>
      </div>
    </div>
  )
}

export default MyEvents
