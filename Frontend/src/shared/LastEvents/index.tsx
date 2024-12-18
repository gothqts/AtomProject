import styles from './LastEvents.module.css'
import { useNavigate } from 'react-router-dom'
import Arrow from '../../assets/images/arrow-right.svg?react'

const Index = ({ events }) => {
  const navigate = useNavigate()

  const truncateDescription = (description) => {
    const endOfSentence = description.indexOf('.') + 1 // Находим первую точку и добавим 1 для включения
    return endOfSentence > 0 ? description.slice(0, endOfSentence) : description // Если точка найдена, обрезаем
  }

  return (
    <>
      {events.map((event) =>
        event.id % 2 !== 0 ? (
          <div key={event.id} className={styles.wrapper_odd}>
            <img src={event.image} alt='Изображение мероприятия' />
            <div className={styles.event_content}>
              <div className={styles.event_info}>
                <div className={styles.event_header}>{event.title}</div>
                <div className={styles.date}>
                  {event.date} {event.time}
                </div>
                <div className={styles.event_description}>{truncateDescription(event.description)}</div>
              </div>
              <button onClick={() => navigate(`/events/${event.id}`)} className={styles.detail_button}>
                Подробнее
                <Arrow style={{ marginLeft: '20px' }} />
              </button>
            </div>
          </div>
        ) : (
          <div key={event.id} className={styles.wrapper_even}>
            <div className={styles.event_content}>
              <div className={styles.event_info}>
                <div className={styles.event_header}>{event.title}</div>
                <div className={styles.date}>
                  {event.date} {event.time}
                </div>
                <div className={styles.event_description}>{truncateDescription(event.description)}</div>
              </div>
              <button onClick={() => navigate(`/event/${event.id}`)} className={styles.detail_button}>
                Подробнее
                <Arrow style={{ marginLeft: '20px' }} />
              </button>
            </div>
            <img src={event.image} alt='Изображение мероприятия' />
          </div>
        )
      )}
    </>
  )
}

export default Index
