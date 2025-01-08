import styles from './Event.module.css'
import Event_Detailed_Arrow from '../../assets/images/Event_Detailed_Arrow.svg?react'
import { Link } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'

const Event = ({ id, name, date, description, image }) => {
  return (
    <div className={styles.container}>
      <img className={styles.image} src={image} alt='Баннер события' />
      <div className={styles.container_info}>
        <div className={styles.title}>{name}</div>
        <div className={styles.date}>{date}</div>
        <div className={styles.description}>{description}</div>
        <Link to={urls.event.replace(':id', id)} className={styles.detailed}>
          Подробнее
          <Event_Detailed_Arrow />
        </Link>
      </div>
    </div>
  )
}

export default Event
