import { useLocation } from 'react-router-dom'
import styles from './EventDetails.module.css'
import { formatDate } from '../../utils/formatingData/formatDate.ts'
import SubscribeBtn from '../../shared/buttons/SubscribeBtn'
import { useStores } from '../../stores/rootStoreContext.ts'
import { useEffect, useState } from 'react'
import Modal from '../../shared/Modal/index.tsx'
import SignWindow from './Components/SignWindow'
import SubscribeForm from './Components/SubscribeForm'
import { ISignUpResponse } from '../../models/Events/response/EventsResponse.ts'

const EventDetail: React.FC = () => {
  useEffect(() => {
    window.scrollTo(0, 0)
  }, [])
  const { eventStore } = useStores()
  const location = useLocation()
  const event = location.state
  const SignWindows = eventStore.currentWindows

  const [modalActive, setModalActive] = useState<boolean>(false)
  const [showSubscribeForm, setShowSubscribeForm] = useState<boolean>(false)
  const [selectedSignWindow, setSelectedSignWindow] = useState<ISignUpResponse>(null)

  const handleSignWindowClick = (signWindow) => {
    setSelectedSignWindow(signWindow)
    setShowSubscribeForm(true)
  }

  const handleSubscribeBtnClick = () => {
    setModalActive(true)
  }

  const handleModalClose = () => {
    setModalActive(false)
    setShowSubscribeForm(false)
    setSelectedSignWindow(null)
  }

  return (
    <div className={styles.container}>
      <div className={styles.image_container}>
        <img className={styles.img} src={event.bannerImage} alt='Изображение мероприятия' />
      </div>
      <div className={styles.info_container}>
        <div className={styles.title}>{event.title}</div>
        <div className={styles.description}>{event.description}</div>
        <div className={styles.date}>{formatDate(event.dateStart)}</div>
        <div className={styles.address}>
          {event.city}, {event.address}
        </div>
        <SubscribeBtn eventId={event.id} setModalActive={handleSubscribeBtnClick} />

        <Modal active={modalActive} setModalActive={handleModalClose}>
          {showSubscribeForm && selectedSignWindow ? (
            <SubscribeForm selectedSignWindow={selectedSignWindow} eventId={event.id} />
          ) : (
            <div className={styles.windows_container}>
              {SignWindows.map((signWindow) => (
                <SignWindow
                  key={signWindow.id}
                  title={signWindow.title}
                  dateStart={signWindow.dateTime}
                  ticketsLeft={signWindow.ticketsLeft}
                  maxVisitors={signWindow.maxVisitors}
                  onClick={() => handleSignWindowClick(signWindow)}
                />
              ))}
            </div>
          )}
        </Modal>
      </div>
    </div>
  )
}

export default EventDetail
