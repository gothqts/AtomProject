import styles from './home.module.css'
import LastEvents from '../../shared/LastEvents'
import Arrow from '../../assets/images/arrow_down.svg?react'
import BlueBtn from '../../shared/buttons/BlueBtn'
import { urls } from '../../navigate/app.urls.ts'
import { Link } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { useStores } from '../../stores/rootStoreContext.ts'
import { useEffect } from 'react'

const Home = observer(() => {
  const { authStore, eventStore } = useStores()

  useEffect(() => {
    eventStore.FetchUpcomingEvents()
  }, [])

  return (
    <div className={styles.home_container}>
      <div className={styles.main_wrapper}>
        <div className={styles.title}>
          Интересные
          <br />
          мероприятия
          <br />
          для вас
        </div>
        {!authStore.AuthState.isAuth ? (
          <div className={styles.btn}>
            <Link className={styles.link} to={urls.register}>
              <BlueBtn btn_placeholder={'Зарегестрироваться'} type='submit' />
            </Link>
          </div>
        ) : (
          <div></div>
        )}
      </div>

      <div className={styles.selection_of_events}>
        <div className={styles.selection_of_events_title}>Подборка мероприятий</div>
        <LastEvents events={eventStore.upcomingEvents} />
      </div>
      <div className={styles.footer_link}>
        Интересные факты о нас
        <Arrow />
      </div>
    </div>
  )
})

export default Home
