import styles from './EventItem.module.css'
import UpcomingEventEditArrow from '../../../../assets/images/UpcomingEventEditArrow.svg?react'
import { useNavigate } from 'react-router-dom'
import { urls } from '../../../../navigate/app.urls.ts'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'

interface IEventItemProps {
  id: string
  title: string
  past: boolean
  isActivity: boolean
}

const EventItem: React.FC<IEventItemProps> = ({ title, past, isActivity, id }) => {
  const { eventStore } = useStores()
  const navigate = useNavigate()
  const handleClick = (e) => {
    console.log(id)
    eventStore.FetchEventInfoById(id)
  }
  return (
    <div className={styles.container} style={{ background: past ? 'rgba(255, 123.25, 0, 0.42)' : '#FF7B00' }}>
      <div className={styles.title}>{title}</div>
      <button className={styles.edit_div}>
        {isActivity ? (
          <div className={styles.edit} onClick={handleClick}>
            Подробнее
          </div>
        ) : (
          <div className={styles.edit} onClick={handleClick}>
            Редактировать
          </div>
        )}
        <UpcomingEventEditArrow />
      </button>
    </div>
  )
}

export default observer(EventItem)
