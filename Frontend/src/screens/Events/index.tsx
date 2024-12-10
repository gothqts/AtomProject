import SearchBar from '../../shared/searchBar'
import Event from '../../shared/Event'
import styles from './Events.module.css'

const Events = () => {
  const events = [
    { id: 1, name: 'Мероприятие 1', date: '22 августа 18:00', description: 'Краткое описание мероприятия 1.' },
    { id: 2, name: 'Мероприятие 2', date: '23 августа 19:00', description: 'Краткое описание мероприятия 2.' },
    { id: 3, name: 'Мероприятие 3', date: '24 августа 20:00', description: 'Краткое описание мероприятия 3.' },
  ]

  return (
    <div className={styles.container}>
      <SearchBar />
      <div className={styles.events_list}>
        {events.map((event) => (
          <Event key={event.id} id={event.id} name={event.name} date={event.date} description={event.description} />
        ))}
      </div>
    </div>
  )
}

export default Events
