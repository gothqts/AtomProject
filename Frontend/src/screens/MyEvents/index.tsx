import { FC, useEffect, useState } from 'react'
import styles from './MyEvents.module.css'
import EventItem from './EventItem/index.tsx'
import { useNavigate } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { useStores } from '../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'

const MyEvents: FC = () => {
  const navigate = useNavigate()
  const { eventStore } = useStores()
  const myEvents = eventStore.myEvents
  const myPastEvents = eventStore.myPastEvents
  const [upcomingDisplayCount, setUpcomingDisplayCount] = useState<number>(2)
  const [pastEventDisplayCount, setPastEventDisplayCount] = useState<number>(2)
  const [showAllUpcomingEvents, setShowAllUpcomingEvents] = useState<boolean>(false)
  const [showAllPastEvents, setShowAllPastEvents] = useState<boolean>(false)

  useEffect(() => {
    eventStore.FetchMyEvents()
    eventStore.FetchMyPastEvents()
  }, [eventStore])

  const handleClick = () => {
    navigate(urls.createEvent)
    eventStore.CreateEvent()
  }

  const handleShowMoreUpcoming = () => {
    setUpcomingDisplayCount((prevCount) => prevCount + 10)
    setShowAllUpcomingEvents(true)
  }

  const handleHideUpcomingEvents = () => {
    setShowAllUpcomingEvents(false)
    setUpcomingDisplayCount(2)
  }

  const handleShowMorePastEvents = () => {
    setPastEventDisplayCount((prevCount) => prevCount + 10)
    setShowAllPastEvents(true)
  }

  const handleHidePastEvents = () => {
    setShowAllPastEvents(false)
    setPastEventDisplayCount(2)
  }

  return (
    <div className={styles.container}>
      <div className={styles.section}>
        <div className={styles.header_container}>
          <h1>Мои мероприятия</h1>
          <button className={styles.create_btn} onClick={handleClick}>
            Создать
          </button>
        </div>

        <h2>Предстоящие</h2>
        {myEvents.slice(0, showAllUpcomingEvents ? myEvents.length : upcomingDisplayCount).map((event) => (
          <EventItem key={event.id} title={event.title} past={false} />
        ))}
        {!showAllUpcomingEvents ? (
          <button className={styles.show_more} onClick={handleShowMoreUpcoming}>
            Показать ещё
          </button>
        ) : (
          <button className={styles.show_more} onClick={handleHideUpcomingEvents}>
            Скрыть события
          </button>
        )}

        <h2>Прошедшие</h2>
        {myPastEvents.slice(0, showAllPastEvents ? myPastEvents.length : pastEventDisplayCount).map((event) => (
          <EventItem key={event.id} title={event.title} past={true} />
        ))}
        {pastEventDisplayCount < myPastEvents.length && !showAllPastEvents && (
          <button className={styles.show_more} onClick={handleShowMorePastEvents}>
            Показать ещё
          </button>
        )}
        {showAllPastEvents && (
          <button className={styles.show_more} onClick={handleHidePastEvents}>
            Скрыть события
          </button>
        )}
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

export default observer(MyEvents)
