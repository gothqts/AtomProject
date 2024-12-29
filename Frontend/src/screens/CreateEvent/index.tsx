import { ChangeEvent, FC, useEffect } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'
import styles from './CreateEvent.module.css'
import MuiPicker from '../../shared/muiDatePicker/MuiPicker.tsx'
import ProfileInput from '../../shared/ProfileInput'
import { IBasicEventInfo } from '../../models/Events/response/EventsResponse.ts'
import { observer } from 'mobx-react-lite'
import BannerUploader from './Components/BannerUploader'
import dayjs, { Dayjs } from 'dayjs'

const CreateEvent: React.FC = () => {
  const { eventStore } = useStores()
  const CreatingEvent: IBasicEventInfo | null = eventStore.creatingEvent
  const handleInputChange = (field: keyof IBasicEventInfo) => (event: ChangeEvent<HTMLInputElement>) => {
    const value: string = event.target.value
    eventStore.setCreatingEventData(field, value)
  }

  const handleDateChange = (field: 'dateStart' | 'dateEnd') => (newValue) => {
    if (newValue) {
      eventStore.setCreatingEventData(field, newValue.toISOString())
    } else {
      eventStore.setCreatingEventData(field, '')
    }
  }

  const handleUpdate = (e: React.MouseEvent<HTMLButtonElement>) => {
    if (CreatingEvent) {
      const { id, ...UpdateEventData } = CreatingEvent
      console.log(CreatingEvent)
      eventStore.UpdateEvent(UpdateEventData, id)
    }
  }

  return (
    <div className={styles.container}>
      <div className={styles.header}>Новое мероприятие</div>
      <ProfileInput title='Название' type='text' value={CreatingEvent?.title || ''} placeholder='Введите название' onChange={handleInputChange('title')} />
      <div className={styles.dates_header}>Дата начала мероприятия</div>
      <MuiPicker title='Выберите время начала мероприятия' value={dayjs(CreatingEvent?.dateStart)} onChange={handleDateChange('dateStart')} />
      <div className={styles.dates_header}>Дата окончания мероприятия</div>
      <MuiPicker title='Выберите время окончания мероприятия' value={dayjs(CreatingEvent?.dateEnd)} onChange={handleDateChange('dateEnd')} />
      <BannerUploader />
      <ProfileInput
        type='text'
        title='Город'
        value={CreatingEvent?.city || ''}
        placeholder='Введите город проведения мероприятия'
        onChange={handleInputChange('city')}
      />
      <ProfileInput type='text' title='Адрес' value={CreatingEvent?.address || ''} placeholder='Введите адрес' onChange={handleInputChange('address')} />
      <ProfileInput
        type='text'
        title='Описание'
        value={CreatingEvent?.description || ''}
        placeholder='Расскажите о мероприятии'
        onChange={handleInputChange('description')}
      />
      <button className={styles.update_btn} onClick={handleUpdate}>
        Сохранить
      </button>
    </div>
  )
}

export default observer(CreateEvent)
