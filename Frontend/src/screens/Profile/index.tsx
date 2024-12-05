import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'
import ProfileInput from '../../shared/ProfileInput/index.tsx'

const Profile = () => {
  const { userStore } = useStores()
  const { user } = userStore

  const personal_data_fields = [
    { title: 'ФИО', type: 'text' },
    { title: 'Телефон', type: 'tel' },
    { title: 'E-mail', type: 'email' },
    { title: 'Фото', type: 'text' },
  ]

  const safety_fields = [{ title: 'Пароль', type: 'password' }]

  const about_fields = [
    { title: 'Город', type: 'text' },
    { title: 'Статус', type: 'text' },
  ]

  return (
    <div className={styles.profile_container}>
      <div className={styles.section}>
        <div className={styles.section_title}>Личные данные</div>
        {personal_data_fields.map((field) => (
          <ProfileInput title={field.title} type={field.type} />
        ))}
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Безопасность</div>
        {safety_fields.map((field) => (
          <ProfileInput title={field.title} type={field.type} />
        ))}
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Информация о себе</div>
        {about_fields.map((field) => (
          <ProfileInput title={field.title} type={field.type} />
        ))}
        <ProfileInput title='О себе' type='text' />
      </div>
    </div>
  )
}

export default Profile
