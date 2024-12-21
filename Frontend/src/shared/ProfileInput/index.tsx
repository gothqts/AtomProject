import styles from './ProfileInput.module.css'
import { observer } from 'mobx-react-lite'

interface ProfileInputProps {
  title: string
  type: string
  value: string
  placeholder: string
  onChange: React.ChangeEventHandler<HTMLInputElement>
}

const ProfileInput: React.FC<ProfileInputProps> = observer(({ title, type, value, onChange, placeholder }) => {
  const shouldDisplayTitle: boolean = title !== 'E-mail' && title !== 'Телефон'

  return (
    <div className={styles.container}>
      {shouldDisplayTitle && <div className={styles.title}>{title}</div>}
      <div className={styles.input_container}>
        <input className={styles.input} type={type} value={value} onChange={onChange} placeholder={placeholder} />
      </div>
    </div>
  )
})

export default ProfileInput
