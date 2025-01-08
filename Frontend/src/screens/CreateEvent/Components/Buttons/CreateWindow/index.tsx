import styles from './CreateWindow.module.css'
interface CreateWindowBtnProps {
  setModalActive: (active: boolean) => void
}

const CreateWindowBtn: React.FC<CreateWindowBtnProps> = ({ setModalActive }) => {
  const handleCreate = () => {
    setModalActive(true)
  }

  return (
    <button onClick={handleCreate} className={styles.btn}>
      Создать окно записи
    </button>
  )
}

export default CreateWindowBtn
