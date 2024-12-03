import SearchBar from '../../shared/searchBar'
import Event from '../../shared/Event'
import styles from './events.module.css'

const Events = () => {
  return (
    <div className={styles.container}>
      <SearchBar />
      <div className={styles.events_list}>
        <Event />
        <Event />
        <Event />
      </div>
    </div>
  )
}

export default Events
