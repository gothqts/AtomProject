import styles from './RegisterWindow.module.css'
import { FaPencil } from 'react-icons/fa6'
import { IoCloseSharp } from 'react-icons/io5'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'

const RegisterWindow: React.FC = ({ title, windowId, eventId, setModalActive, handleUpdate }) => {
  const { eventStore } = useStores()
  const handleDelete = async () => {
    if (windowId && eventId) {
      await eventStore.DeleteRegisterWindow(eventId, windowId)
      await eventStore.FetchEventInfoById(eventId)
    }
  }
  const handleChange = () => {
    setModalActive(true)
  }

  return (
    <div className={styles.container}>
      <div className={styles.title}>{title}</div>
      <FaPencil className={styles.pencil} onClick={handleChange} />
      <IoCloseSharp className={styles.sharp} onClick={handleDelete} />
    </div>
  )
}

export default observer(RegisterWindow)
