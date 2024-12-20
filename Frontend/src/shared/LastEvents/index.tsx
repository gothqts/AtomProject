import { useNavigate } from 'react-router-dom'
import Arrow from '../../assets/images/arrow-right.svg?react'
import { IUpcomingEvents } from '../../screens/Home/types/homeTypes.ts'
import styles from './LastEvents.module.css'
import { formatDate } from '../../utils/formatingData/formatDate.ts'

interface IPropsLastEvents {
  events: IUpcomingEvents[]
}

const LastEvents: React.FC<IPropsLastEvents> = ({ events }) => {
  const navigate = useNavigate()

  const navigateToDetails = (event) => {
    const { id, title, dateStart, description, bannerImage, address, city } = event
    navigate(`/event/${id}`, {
      state: { id, title, dateStart, description, bannerImage, address, city },
    })
  }

  return (
    <>
      {events.map((event, index) => (
        <div key={event.id} className={index % 2 !== 0 ? styles.wrapper_odd : styles.wrapper_even}>
          {index % 2 !== 0 && <img src={event.bannerImage} alt='Изображение мероприятия' />}
          <div className={styles.event_content}>
            <div className={styles.event_info}>
              <div className={styles.event_header}>{event.title}</div>
              <div className={styles.event_address}>
                {event.city}, {event.address}
              </div>
              <div className={styles.date}>{formatDate(event.dateStart)}</div>
              <div className={styles.event_description}>{event.description}</div>
            </div>
            <button onClick={() => navigateToDetails(event)} className={styles.detail_button}>
              Подробнее
              <Arrow style={{ marginLeft: '20px' }} />
            </button>
          </div>
          {index % 2 === 0 && <img src={event.bannerImage} alt='Изображение мероприятия' />}
        </div>
      ))}
    </>
  )
}

export default LastEvents
