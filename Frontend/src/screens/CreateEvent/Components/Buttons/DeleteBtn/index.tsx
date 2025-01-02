import { urls } from '../../../../../navigate/app.urls.ts'
import { useNavigate, useParams } from 'react-router-dom'
import { useStores } from '../../../../../stores/rootStoreContext.ts'
import styles from './DeleteBtn.module.css'

const DeleteBtn = () => {
  const { id } = useParams<{ id: string }>()
  const { eventStore } = useStores()
  const navigate = useNavigate()
  const handleDelete = async () => {
    if (id) {
      await eventStore.DeleteEvent(id)
      navigate(urls.myEvents)
    }
  }
  return (
    <div>
      <button className={styles.update_btn} onClick={handleDelete}>
        Удалить
      </button>
    </div>
  )
}

export default DeleteBtn
