import styles from './SignWindows.module.css'
import { formatDate } from '../../../../utils/formatingData/formatDate.ts'
import { observer } from 'mobx-react-lite'
const SignWindow = ({ title, dateStart, maxVisitors, ticketsLeft, onClick }) => {
  return (
    <div className={styles.container} onClick={onClick}>
      <div className={styles.title}>{title}</div>
      <div className={styles.date}>{formatDate(dateStart)}</div>
      <div className={styles.visitors}>{`Всего мест: ${maxVisitors}`}</div>
      <div className={styles.ticketsLeft}>{`Доступно мест: ${ticketsLeft}`}</div>
    </div>
  )
}

export default observer(SignWindow)
