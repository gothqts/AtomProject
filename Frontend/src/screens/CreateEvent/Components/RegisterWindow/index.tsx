import styles from './RegisterWindow.module.css'
import { FaPencil } from 'react-icons/fa6'
import { IoCloseSharp } from 'react-icons/io5'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import { formatDate } from '../../../../utils/formatingData/formatDate.ts'
import { ISignupWindowResponse } from '../../../../models/Events/response/EventsResponse.ts'

interface IRegisterWindowProps {
  data: ISignupWindowResponse
  windowId: string
  eventId: string
}

const RegisterWindow: React.FC<IRegisterWindowProps> = ({ data, windowId, eventId }) => {
  const { eventStore } = useStores()
  const handleDelete = async () => {
    if (windowId && eventId) {
      await eventStore.DeleteRegisterWindow(eventId, windowId)
      await eventStore.FetchSignUpWindows(eventId)
    }
  }
  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <div className={styles.title}>{data.title}</div>
        <IoCloseSharp className={styles.sharp} onClick={handleDelete} />
      </div>
      <div>{formatDate(data.dateTime)}</div>
      <div>{`Всего мест: ${data.maxVisitors}`}</div>
      <div>{`Доступно мест: ${data.ticketsLeft}`}</div>
    </div>
  )
}

export default observer(RegisterWindow)
