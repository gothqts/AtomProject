import { useParams, useLocation } from 'react-router-dom'
import styles from './EventDetails.module.css'
import { formatDate } from '../../utils/formatingData/formatDate.ts'

const EventDetail: React.FC = () => {
  const { eventId } = useParams<{ eventId: string }>()
  const location = useLocation()
  const event = location.state || {}

  return (
    <div className={styles.container}>
      <div className={styles.image_container}>
        <img className={styles.img} src={event.bannerImage} alt='Изображение мероприятия' />
      </div>
      <div className={styles.info_container}>
        <div className={styles.title}>{event.title}</div>
        <div className={styles.description}>{event.description}</div>
        <div className={styles.date}>{formatDate(event.dateStart)}</div>
        <div className={styles.address}>
          {event.city}, {event.address}
        </div>
      </div>
    </div>
  )
}

export default EventDetail
