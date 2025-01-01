import { FC, useState } from 'react'
import EventItem from './EventItem/index.tsx'
import styles from './EventsBlockType.module.css'
import { observer } from 'mobx-react-lite'

interface EventBlockProps {
  title: string
  events: Array<{ id: string; title: string }>
  isPast: boolean
  isActivity: boolean
}

const EventBlock: FC<EventBlockProps> = ({ title, events, isPast, isActivity }) => {
  const [displayCount, setDisplayCount] = useState<number>(2)
  const [showAll, setShowAll] = useState<boolean>(false)

  const handleShowMore = () => {
    setDisplayCount((prevCount) => prevCount + 10)
    setShowAll(true)
  }

  const handleHide = () => {
    setShowAll(false)
    setDisplayCount(2)
  }

  if (events.length == 0 && isActivity) {
    return (
      <div className={styles.eventBlock}>
        <h2>{title}</h2>
        <div className={styles.error_container}>Вы не зарегестрированы на мероприятия</div>
      </div>
    )
  } else if (events.length == 0 && !isActivity) {
    return (
      <div className={styles.eventBlock}>
        <h2>{title}</h2>
        <div className={styles.error_container}>Создайте мероприятие</div>
      </div>
    )
  } else {
    return (
      <div className={styles.eventBlock}>
        <h2>{title}</h2>
        {events.slice(0, showAll ? events.length : displayCount).map((event) => (
          <EventItem key={event.id} title={event.title} past={isPast} isActivity={isActivity} id={event.id} />
        ))}
        {!showAll && events.length > displayCount && (
          <button className={styles.showMore} onClick={handleShowMore}>
            Показать ещё
          </button>
        )}
        {showAll && (
          <button className={styles.showMore} onClick={handleHide}>
            Скрыть
          </button>
        )}
      </div>
    )
  }
}

export default observer(EventBlock)
