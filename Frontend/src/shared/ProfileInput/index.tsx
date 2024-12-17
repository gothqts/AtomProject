import styles from './ProfileInput.module.css'

const ProfileInput = ({ title, type, value, onChange }) => {
  return (
    <div className={styles.container}>
      <div className={styles.title}>{title}</div>
      <div className={styles.input_container}>
        <input
          className={styles.input}
          type={type}
          value={value} // Устанавливаем значение из props
          onChange={onChange} // Устанавливаем обработчик изменения из props
          placeholder={`Введите ${title}`} // Добавляем placeholder
        />
        <button className={styles.edit}>Изменить</button>
      </div>
    </div>
  )
}

export default ProfileInput
