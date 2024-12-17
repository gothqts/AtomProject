import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'
import ProfileInput from '../../shared/ProfileInput/index.tsx'
import { observer } from 'mobx-react-lite'
import { FC } from 'react'

const Profile: FC = observer(() => {
  const { userStore } = useStores()
  const { user } = userStore

  const personal_data_fields = [
    { title: 'ФИО', type: 'text', value: user?.fio },
    { title: 'Телефон', type: 'tel', value: user?.phone },
    { title: 'E-mail', type: 'email', value: user?.email },
    { title: 'Фото', type: 'text', value: user?.avatarImage },
  ]

  const safety_fields = [
    { title: 'Пароль', type: 'password', value: '' }, // Пароль не сохраняем в состоянии
  ]

  const about_fields = [
    { title: 'Город', type: 'text', value: user?.status }, // Используем статус как пример
    { title: 'Статус', type: 'text', value: user?.status },
  ]

  // Обработчик изменения значений инпутов
  const handleInputChange = (title: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value
    switch (title) {
      case 'ФИО':
        userStore.setUserFields('fio', value)
        break
      case 'Телефон':
        userStore.setUserFields('phone', value)
        break
      case 'E-mail':
        userStore.setUserFields('email', value)
        break
      case 'Фото':
        userStore.setUserFields('avatarImage', value)
        break
      case 'Город':
        userStore.setUserFields('status', value) // Предполагается, что мы используем статус здесь
        break
      case 'Статус':
        userStore.setUserFields('status', value)
        break
      default:
        break
    }
  }

  return (
    <div className={styles.profile_container}>
      <div className={styles.section}>
        <div className={styles.section_title}>Личные данные</div>
        {personal_data_fields.map((field) => (
          <ProfileInput
            key={field.title}
            title={field.title}
            type={field.type}
            value={field.value || ''} // Передаем значение из user
            onChange={handleInputChange(field.title)} // Передаем обработчик изменения
          />
        ))}
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Безопасность</div>
        {safety_fields.map((field, index) => (
          <ProfileInput
            key={index}
            title={field.title}
            type={field.type}
            value={field.value} // Для пароля можно оставить пустым или не выставлять значение
            onChange={() => {}} // Убедитесь, что обработчик корректный или оставьте пустым
          />
        ))}
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Информация о себе</div>
        {about_fields.map((field) => (
          <ProfileInput
            key={field.title}
            title={field.title}
            type={field.type}
            value={field.value}
            onChange={handleInputChange(field.title)} // Передаем обработчик изменения
          />
        ))}
        <ProfileInput
          title='О себе'
          type='text'
          value={user?.description} // Добавляем поле "О себе" для редактирования
          onChange={handleInputChange('О себе')}
        />
      </div>
    </div>
  )
})

export default Profile
