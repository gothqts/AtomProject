import styles from './ProfileInput.module.css'

const ProfileInput = ({ title, type, value, onChange }) => {
  const shouldDisplayTitle: boolean = title !== 'E-mail' && title !== 'Телефон'

  return (
    <div className={styles.container}>
      {shouldDisplayTitle && <div className={styles.title}>{title}</div>}
      <div className={styles.input_container}>
        <input className={styles.input} type={type} value={value} onChange={onChange} placeholder={`Введите ${title}`} />
      </div>
    </div>
  )
}

export default ProfileInput
