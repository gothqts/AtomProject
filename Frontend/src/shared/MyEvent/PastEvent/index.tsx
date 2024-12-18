import styles from './PastEvent.module.css'
import UpcomingEventEditArrow from '../../../assets/images/UpcomingEventEditArrow.svg?react'

const PastEvent = ({ title }) => {
  return (
    <div className={styles.container}>
		<div className={styles.title}>{title}</div>
		<button className={styles.edit_div}>
			<div className={styles.edit}>Редактировать</div>
			<UpcomingEventEditArrow />
		</button>
	</div>
  )
}

export default PastEvent