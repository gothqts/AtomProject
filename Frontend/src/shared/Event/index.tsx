import styles from './Event.module.css'
import Arrow from '../../assets/images/arrow-right.svg?react'
import { useNavigate } from 'react-router-dom'
import { truncateString } from '../../utils/FormatingString/formatingString.ts'

const Event = ({ id, name, date, description, image, data }) => {
  const navigate = useNavigate()
  const navigateToDetails = (event) => {
    const { id, title, dateStart, description, bannerImage, address, city } = event
    navigate(`/event/${id}`, {
      state: { id, title, dateStart, description, bannerImage, address, city },
    })
  }
  return (
    <div className={styles.container}>
      <img className={styles.image} src={image} alt='Баннер события' />
      <div className={styles.container_info}>
        <div className={styles.title}>{name}</div>
        <div className={styles.date}>{date}</div>
        <div className={styles.description}>{truncateString(description, 50)}</div>
        <button onClick={() => navigateToDetails(data)} className={styles.detail_button}>
          Подробнее
          <Arrow style={{ marginLeft: '20px' }} />
        </button>
      </div>
    </div>
  )
}

export default Event
