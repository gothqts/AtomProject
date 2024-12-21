import { useState } from 'react'
import styles from './AvatarUploader.module.css'
import { useStores } from '../../../../stores/rootStoreContext.ts'
import { observer } from 'mobx-react-lite'
import PhotoIcon from '../../../../assets/images/photoIcon.svg?react'

const AvatarUploader: React.FC = () => {
  const { userStore } = useStores()
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
    userStore.UpdateAvatar(formData)
    setDrag(false)
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
          Отпустите фото
        </div>
      ) : (
        <div
          className={styles.drag_area}
          onDragStart={(e) => DragStartHandler(e)}
          onDragLeave={(e) => DragLeaveHandler(e)}
          onDragOver={(e) => DragStartHandler(e)}
        >
          <PhotoIcon />
          Перетащите фото
        </div>
      )}
    </div>
  )
}

export default observer(AvatarUploader)
