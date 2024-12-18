import { FC } from 'react'
import { useStores } from '../../stores/rootStoreContext.ts'

const CreateEvent: FC = () => {
  const { eventStore } = useStores()
  return (
    <div>
      <input />

      <input />
    </div>
  )
}

export default CreateEvent
