import SearchBar from '../../shared/searchBar'
import Event from '../../shared/Event'
import styles from './Events.module.css'
import { useEffect, useState } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import { formatDate } from '../../utils/formatingData/formatDate.ts'
import { IQueryParams } from '../../models/Events/request/eventRequests.ts'

const Events: React.FC = () => {
  const { eventStore } = useStores()
  const allEvents = eventStore.allEvents
  const [params, setParams] = useState<IQueryParams>({
    city: '',
    subject: '',
    date: '',
    take: 10,
    time: '',
    skip: 0,
    online: '',
    search: '',
  })

  useEffect(() => {
    eventStore.FetchFilteredEvents(params)
  }, [params])

  return (
    <div className={styles.container}>
      <SearchBar setParams={setParams} params={params} />
      <div className={styles.events_list}>
        {allEvents.map((event) => (
          <Event key={event.id} id={event.id} name={event.title} date={formatDate(event.dateStart)} description={event.description} image={event.bannerImage} />
        ))}
      </div>
    </div>
  )
}

export default observer(Events)
