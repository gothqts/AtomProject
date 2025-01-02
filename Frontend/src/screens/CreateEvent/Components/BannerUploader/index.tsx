import { useState } from 'react'
import styles from './BannerUploader.module.css'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import PhotoIcon from '../../../../assets/images/photoIcon.svg?react'

const BannerUploader: React.FC = () => {
  const { eventStore } = useStores()
  const [drag, setDrag] = useState<boolean>(false)

  function DragStartHandler(e) {
    e.preventDefault()
    setDrag(true)
  }

  function DragLeaveHandler(e) {
    e.preventDefault()
    setDrag(false)
  }

  function onDropHandler(e) {
    e.preventDefault()
    let file = e.dataTransfer.files
    const formData = new FormData()
    formData.append('file', file[0])
    eventStore.creatingEvent.bannerImage = formData
    setDrag(false)
    console.log(eventStore.creatingEvent.bannerImage)
  }

  return (
    <div className={styles.container}>
      {drag ? (
        <div
          onDragStart={(e) => DragStartHandler(e)}
          onDragLeave={(e) => DragLeaveHandler(e)}
          onDragOver={(e) => DragStartHandler(e)}
          onDrop={(e) => onDropHandler(e)}
          className={styles.drop_area}
        >
          <PhotoIcon />
          Загрузить фото
        </div>
      ) : (
        <div
          className={styles.drag_area}
          onDragStart={(e) => DragStartHandler(e)}
          onDragLeave={(e) => DragLeaveHandler(e)}
          onDragOver={(e) => DragStartHandler(e)}
        >
          <PhotoIcon />
          Добавить фото
        </div>
      )}
    </div>
  )
}

export default observer(BannerUploader)
