import { FC } from 'react'
import styles from './EventInput.module.css'

const EventInput: FC = ({ type, value, onChange, title }) => {
  return (
    <div>
      <div className={styles.input_container}>
        <input className={styles.input} type={type} value={value} onChange={onChange} placeholder={`Введите ${title}`} />
      </div>
    </div>
  )
}

export default EventInput
