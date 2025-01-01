import { ChangeEvent, FC, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { useStores } from '../../stores/rootStoreContext.ts'
import styles from './CreateEvent.module.css'
import MuiPicker from '../../shared/muiDatePicker/MuiPicker.tsx'
import ProfileInput from '../../shared/ProfileInput'
import { IBasicEventInfo } from '../../models/Events/response/EventsResponse.ts'
import { observer } from 'mobx-react-lite'
import BannerUploader from './Components/BannerUploader'
import dayjs from 'dayjs'
import BooleanToggle from './Components/BooleanToogle/index.tsx'
import DeleteBtn from './Components/Buttons/DeleteBtn'
import SaveBtn from './Components/Buttons/SaveBtn'

interface IRouteParams {
  id: string
}
const CreateEvent: FC = () => {
  const { id } = useParams<IRouteParams>()

  const { eventStore, authStore } = useStores()
  const CreatingEvent: IBasicEventInfo = eventStore.creatingEvent

  useEffect(() => {
    if (id) {
      eventStore.FetchEventInfoById(id)
    }
  }, [])

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

  return (
    <div className={styles.container}>
      <div className={styles.header}>Новое мероприятие</div>
      <ProfileInput title='Название' type='text' value={CreatingEvent.title} placeholder='Введите название' onChange={handleInputChange('title')} />
      <div className={styles.dates_header}>Дата начала мероприятия</div>
      <MuiPicker title='Выберите время начала мероприятия' value={dayjs(CreatingEvent?.dateStart)} onChange={handleDateChange('dateStart')} />
      <div className={styles.dates_header}>Дата окончания мероприятия</div>
      <MuiPicker title='Выберите время окончания мероприятия' value={dayjs(CreatingEvent?.dateEnd)} onChange={handleDateChange('dateEnd')} />
      <BannerUploader />
      <ProfileInput
        type='text'
        title='Город'
        value={CreatingEvent.city}
        placeholder='Введите город проведения мероприятия'
        onChange={handleInputChange('city')}
      />
      <ProfileInput type='text' title='Адрес' value={CreatingEvent.address} placeholder='Введите адрес' onChange={handleInputChange('address')} />
      <ProfileInput
        type='text'
        title='Описание'
        value={CreatingEvent.description}
        placeholder='Расскажите о мероприятии'
        onChange={handleInputChange('description')}
      />
      <BooleanToggle label='Онлайн' value={CreatingEvent.isOnline} onChange={(newValue) => eventStore.setCreatingEventData('isOnline', newValue)} />

      <BooleanToggle
        label='Меропритие публичное'
        value={CreatingEvent.isPublic}
        onChange={(newValue) => eventStore.setCreatingEventData('isPublic', newValue)}
      />

      <BooleanToggle
        label='Открыта регистрация'
        value={CreatingEvent.isSignupOpened}
        onChange={(newValue) => eventStore.setCreatingEventData('isSignupOpened', newValue)}
      />
      <div className={styles.btn_container}>
        <SaveBtn />
        <DeleteBtn />
      </div>
    </div>
  )
}

export default observer(CreateEvent)
