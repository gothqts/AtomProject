import styles from './SaveBtn.module.css'
import { urls } from '../../../../../navigate/app.urls.ts'
import { useStores } from '../../../../../stores/rootStoreContext.ts'
import { useNavigate } from 'react-router-dom'

const SaveBtn = () => {
  const { eventStore } = useStores()
  const CreatingEvent = eventStore.creatingEvent
  const navigate = useNavigate()
  const handleUpdate = async (e: React.MouseEvent<HTMLButtonElement>) => {
    if (CreatingEvent) {
      const { id, signupForm, ...UpdateEventData } = CreatingEvent
      if (CreatingEvent.bannerImageFilepath) {
        await eventStore.UpdateBanner(UpdateEventData.bannerImageFilepath, id)
      }
      await eventStore.UpdateEvent(UpdateEventData, id)
      await eventStore.UpdateSubscribeForm(id, signupForm)

      navigate(urls.myEvents)
    }
  }
  return (
    <div>
      <button className={styles.delete_btn} onClick={handleUpdate}>
        Сохранить
      </button>
    </div>
  )
}

export default SaveBtn
