import { FC, useState } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'
import styles from './CreateEvent.module.css'
import EventInput from './EventInput'
import MuiPicker from '../../shared/muiDatePicker/MuiPicker.tsx'
import ProfileInput from '../../shared/ProfileInput'

const CreateEvent: FC = () => {
  const { eventStore } = useStores()

  const generateEventData = () => ({
    id: eventStore.creatingEvent?.id,
    isPublic: eventStore.creatingEvent?.isPublic,
    title: eventStore.creatingEvent?.title,
    bannerImage: eventStore.creatingEvent?.bannerImage,
    dateStart: '',
    dateEnd: '',
    isOnline: eventStore.creatingEvent?.isOnline,
    city: '',
    address: '',
    isSignupOpened: eventStore.creatingEvent?.isSignupOpened,
  })
  const [EventData, setEventData] = useState(generateEventData())

  const handleUpdate = (e) => {
    eventStore.UpdateEvent(EventData)
  }
  return (
    <div className={styles.container}>
      <div className={styles.header}>Новое мероприятие</div>
      <ProfileInput title='Название' value={EventData.title} />
      <div className={styles.dates}>
        <div className={styles.dates_header}>Дата начала мероприятия</div>
        <MuiPicker title='Выберите время начала мероприятия' />
        <div className={styles.dates_header}>Дата окончания мероприятия</div>
        <MuiPicker title='Выберите время окончания мероприятия' />
      </div>
      <ProfileInput title={'Баннер'} value={EventData.bannerImage} />
      <ProfileInput title={'Город'} value={EventData.city} />
      <ProfileInput title={'Адрес'} value={EventData.address} />
      <ProfileInput title={'Описание'} />
      <button className={styles.update_btn} onClick={handleUpdate}>
        Сохранить
      </button>
    </div>
  )
}

export default CreateEvent
