import styles from './Modal.module.css'
import { observer } from 'mobx-react-lite'

const Modal: React.FC = ({ active, setModalActive, children }) => {
  const handleClose = () => {
    setModalActive(false)
  }

  return (
    <div className={`${styles.modal} ${active ? styles.active : ''}`} onClick={handleClose}>
      <div className={`${styles.modal_container} ${active ? styles.modal_container : ''}`} onClick={(e) => e.stopPropagation()}>
        {children}
      </div>
    </div>
  )
}

export default observer(Modal)
