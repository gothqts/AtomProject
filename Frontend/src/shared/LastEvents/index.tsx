import styles from './LastEvents.module.css'
import { useNavigate } from 'react-router-dom'
import Arrow from '../../assets/images/arrow-right.svg?react'
import { IUpcomingEvents } from '../../screens/Home/types/homeTypes.ts'

interface IPropsLastEvents {
  events: IUpcomingEvents[]
}

const LastEvents: React.FC<IPropsLastEvents> = ({ events }) => {
  const navigate = useNavigate()

  const truncateDescription = (description: string): string => {
    const endOfSentence = description.indexOf('.') + 1 // Находим первую точку и добавим 1 для включения
    return endOfSentence > 0 ? description.slice(0, endOfSentence) : description // Если точка найдена, обрезаем
  }

  const formatDate = (isoDate: string): string => {
    const date: Date = new Date(isoDate)

    const months: string[] = ['января', 'февраля', 'марта', 'апреля', 'мая', 'июня', 'июля', 'августа', 'сентября', 'октября', 'ноября', 'декабря']

    const day = date.getDate()
    const month = months[date.getMonth()]
    const hours = date.getHours().toString().padStart(2, '0')
    const minutes = date.getMinutes().toString().padStart(2, '0')

    return `${day} ${month} ${hours}:${minutes}`
  }

  return (
    <>
      {events.map((event, index) =>
        index % 2 !== 0 ? (
          <div key={event.id} className={styles.wrapper_odd}>
            <img src={event.bannerImage} alt='Изображение мероприятия' />
            <div className={styles.event_content}>
              <div className={styles.event_info}>
                <div className={styles.event_header}>{event.title}</div>
                <div className={styles.date}>{formatDate(event.dateStart)}</div>
                {/*<div className={styles.event_description}>{truncateDescription(event.description)}</div>*/}
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
                <div className={styles.date}>{formatDate(event.dateStart)}</div>
                {/*<div className={styles.event_description}>{truncateDescription(event.description)}</div>*/}
              </div>
              <button onClick={() => navigate(`/event/${event.id}`)} className={styles.detail_button}>
                Подробнее
                <Arrow style={{ marginLeft: '20px' }} />
              </button>
            </div>
            <img src={event.bannerImage} alt='Изображение мероприятия' />
          </div>
        )
      )}
    </>
  )
}

export default LastEvents
