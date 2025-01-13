import { ChangeEvent, FC, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { useStores } from '../../stores/rootStoreContext.ts'
import styles from './CreateEvent.module.css'
import ProfileInput from '../../shared/ProfileInput'
import { IBasicEventInfo, MyCreatingEvent } from '../../models/Events/response/EventsResponse.ts'
import { observer } from 'mobx-react-lite'
import BannerUploader from './Components/BannerUploader'
import BooleanToggle from './Components/BooleanToogle/index.tsx'
import SaveBtn from './Components/Buttons/SaveBtn'
import DeleteBtn from './Components/Buttons/DeleteBtn'
import Modal from '../../shared/Modal'
import WindowForm from './Components/WindowForm'
import RegisterWindow from './Components/RegisterWindow'
import CreateWindowBtn from './Components/Buttons/CreateWindow'
import DateTimeSelect from './Components/DateTimeSelect'

interface IRouteParams {
  id: string
}

const CreateEvent: FC = () => {
  const { id } = useParams<IRouteParams>()
  const { eventStore } = useStores()
  const CreatingEvent: MyCreatingEvent = eventStore.creatingEvent
  const [modalActive, setModalActive] = useState<boolean>(false)
  const Windows = eventStore.currentWindows
  const SubscribeFormParams = eventStore.creatingEvent.signupForm
  useEffect(() => {
    if (id) {
      eventStore.FetchEventInfoById(id)
      eventStore.FetchSignUpWindows(id)
    }
  }, [])

  const handleInputChange = (field: keyof IBasicEventInfo) => (event: ChangeEvent<HTMLInputElement>) => {
    const value: string = event.target.value
    eventStore.setCreatingEventData(field, value)
  }
  const handleDateChange = (field: 'dateStart' | 'dateEnd') => (newValue: string) => {
    if (newValue) {
      eventStore.setCreatingEventData(field, newValue.toString())
    } else {
      eventStore.setCreatingEventData(field, '')
    }
  }

  return (
    <div className={styles.container}>
      <div className={styles.header}>Создание мероприятия</div>
      <ProfileInput title='Название' type='text' value={CreatingEvent.title} placeholder='Введите название' onChange={handleInputChange('title')} />
      <div className={styles.dates_header}>Дата начала мероприятия</div>
      <DateTimeSelect value={CreatingEvent.dateStart} onChange={handleDateChange('dateStart')} />
      <div className={styles.dates_header}>Дата окончания мероприятия</div>
      <DateTimeSelect value={CreatingEvent.dateEnd} onChange={handleDateChange('dateEnd')} />
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
      <div className={styles.form_container}>
        <div className={styles.form_container_header}>Настройка формата</div>
        <BooleanToggle label='Онлайн?' value={CreatingEvent.isOnline} onChange={(newValue) => eventStore.setCreatingEventData('isOnline', newValue)} />
        <BooleanToggle
          label='Меропритие публичное?'
          value={CreatingEvent.isPublic}
          onChange={(newValue) => eventStore.setCreatingEventData('isPublic', newValue)}
        />
        <BooleanToggle
          label='Открыта регистрация?'
          value={CreatingEvent.isSignupOpened}
          onChange={(newValue) => eventStore.setCreatingEventData('isSignupOpened', newValue)}
        />
      </div>
      <div className={styles.windows_container}>
        <div className={styles.windows_header}>Окна записи</div>
        <div className={styles.windows_list}>
          {Windows.map((window) => (
            <RegisterWindow key={window.id} data={window} windowId={window.id} eventId={id} setModalActive={setModalActive} />
          ))}
          <CreateWindowBtn className={styles.create_window_btn} setModalActive={setModalActive} />
        </div>

        <Modal active={modalActive} setModalActive={setModalActive}>
          <WindowForm eventId={id} setModalActive={setModalActive} />
        </Modal>
      </div>
      <div className={styles.form_container}>
        <div className={styles.form_container_header}>Настройка формы записи</div>
        <BooleanToggle
          label={'Необходимо указать телефон при записи?'}
          value={SubscribeFormParams.isPhoneRequired}
          onChange={(newValue) => eventStore.setCreatingEventSubForm('isPhoneRequired', newValue)}
        />
        <BooleanToggle
          label={`Необходимо указать ФИО при записи?`}
          value={SubscribeFormParams.isFioRequired}
          onChange={(newValue) => eventStore.setCreatingEventSubForm('isFioRequired', newValue)}
        />
        <BooleanToggle
          label={`Необходимо указать email при записи?`}
          value={SubscribeFormParams.isEmailRequired}
          onChange={(newValue) => eventStore.setCreatingEventSubForm('isEmailRequired', newValue)}
        />
      </div>
      <div className={styles.btn_container}>
        <DeleteBtn />
        <SaveBtn />
      </div>
    </div>
  )
}

export default observer(CreateEvent)
