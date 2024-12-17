import styles from './ProfileInput.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'

const ProfileInput = ({ title, type, value, onChange }) => {
  const { userStore } = useStores()
  const { UpdateData } = userStore
  return (
    <div className={styles.container}>
      <div className={styles.title}>{title}</div>
      <div className={styles.input_container}>
        <input className={styles.input} type={type} value={value} onChange={onChange} placeholder={`Введите ${title}`} />
      </div>
    </div>
  )
}

export default ProfileInput
