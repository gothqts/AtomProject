import { useParams } from 'react-router-dom'

const EventDetail = () => {
  const { eventId } = useParams()

  return (
    <div>
      <h1>Детали мероприятия {eventId}</h1>
      <p>Здесь будет информация о мероприятии.</p>
    </div>
  )
}

export default EventDetail
