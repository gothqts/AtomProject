import styles from './Modal.module.css'

const Modal: React.FC = ({ active, setActive, children }) => {
  return (
    <div className={`${styles.modal} ${active ? styles.active : ''}`} onClick={() => setActive(false)}>
      <div className={`${styles.modal_container} ${active ? styles.modal_container : ''}`} onClick={(e) => e.stopPropagation()}>
        {children}
      </div>
    </div>
  )
}

export default Modal
