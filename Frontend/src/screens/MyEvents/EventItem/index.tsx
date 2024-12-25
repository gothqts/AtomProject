import styles from './EventItem.module.css'
import UpcomingEventEditArrow from '../../../assets/images/UpcomingEventEditArrow.svg?react'

interface IEventItemProps {
  title: string
  past: boolean
}

const EventItem: React.FC<IEventItemProps> = ({ title, past }) => {
  return (
    <div className={styles.container} style={{ background: past ? 'rgba(255, 123.25, 0, 0.42)' : '#FF7B00' }}>
      <div className={styles.title}>{title}</div>
      <button className={styles.edit_div}>
        <div className={styles.edit}>Редактировать</div>
        <UpcomingEventEditArrow />
      </button>
    </div>
  )
}

export default EventItem
