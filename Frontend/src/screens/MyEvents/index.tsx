import { FC, useEffect } from 'react'
import styles from './MyEvents.module.css'
import EventBlock from './EventsBlockType/index.tsx'
import { useNavigate } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { useStores } from '../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'

const MyEvents: FC = () => {
  const navigate = useNavigate()
  const { eventStore } = useStores()
  const myEvents = eventStore.myEvents
  const myPastEvents = eventStore.myPastEvents
  const myActivity = eventStore.userActivity
  const myPastActivity = eventStore.userPastActivity

  useEffect(() => {
    eventStore.FetchMyEvents()
    eventStore.FetchMyPastEvents()
    eventStore.FetchUserActivity()
    eventStore.FetchUserPastActivity()
  }, [eventStore])

  const handleClick = () => {
    navigate(urls.createEvent)
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
        <EventBlock title='Предстоящие' events={myEvents} isPast={false} isActivity={false} />
        <EventBlock title='Прошедшие' events={myPastEvents} isPast={true} isActivity={false} />
      </div>

      <div className={styles.section}>
        <h1 className={styles.header_right}>Активность в мероприятиях</h1>
        <EventBlock title='Предстоящие' events={myActivity} isPast={false} isActivity={true} />
        <EventBlock title='Прошедшие' events={myPastActivity} isPast={true} isActivity={true} />
      </div>
    </div>
  )
}

export default observer(MyEvents)
