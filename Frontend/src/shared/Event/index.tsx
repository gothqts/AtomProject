import styles from './Event.module.css'
import Event_Detailed_Arrow from '../../assets/images/Event_Detailed_Arrow.svg?react'
import { Link } from 'react-router-dom'

const Event = () => {
  return (
    <div className={styles.container}>
      <div className={styles.image}></div>

      <div className={styles.container_info}>
        <div className={styles.title}>Мероприятие 1</div>

        <div className={styles.date}>22 августа 18:00</div>

        <div className={styles.description}>Краткое описание...</div>

        <Link className={styles.detailed}>
          Подробнее
          <Event_Detailed_Arrow />
        </Link>
      </div>
    </div>
  )
}

export default Event
