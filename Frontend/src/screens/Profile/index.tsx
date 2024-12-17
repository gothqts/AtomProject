import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'
import ProfileInput from '../../shared/ProfileInput/index.tsx'
import { observer } from 'mobx-react-lite'
import { FC, useState, useEffect } from 'react'
import { UserRequest } from '../../models/User/request/userRequest.ts'

const Profile: FC = observer(() => {
  const { userStore } = useStores()
  const { user } = userStore

  const generateProfileData = () => ({
    PersonalData: [
      { title: 'ФИО', type: 'text', value: user?.fio },
      { title: 'Описание', type: 'text', value: user?.description },
      { title: 'Статус', type: 'text', value: user?.status },
      { title: 'Город', type: 'text', value: user?.city },
    ],
    Avatar: [{ title: 'Фото', type: 'text', value: user?.avatarImage }],
    Telephone: [{ title: 'Телефон', type: 'tel', value: user?.phone }],
    Email: [{ title: 'E-mail', type: 'email', value: user?.email }],
    Password: [
      { title: 'Текущий пароль', type: 'password', value: '' },
      { title: 'Новый пароль', type: 'password', value: '' },
    ],
  })

  const [profileData, setProfileData] = useState(generateProfileData())

  useEffect(() => {
    setProfileData(generateProfileData())
  }, [user])

  const handleClick = () => {
    const updatedData = {
      fio: profileData.PersonalData.find((field) => field.title === 'ФИО')?.value,
      description: profileData.PersonalData.find((field) => field.title === 'Описание')?.value,
      status: profileData.PersonalData.find((field) => field.title === 'Статус')?.value,
      city: profileData.PersonalData.find((field) => field.title === 'Город')?.value,
    }
    console.log(updatedData)
    userStore.UpdateData(updatedData)
  }

  const handleInputChange = (title: string) => (event: React.ChangeEvent<HTMLInputElement>) => {
    const value: string = event.target.value
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
        userStore.setUserFields('city', value)
        break
      case 'Статус':
        userStore.setUserFields('status', value)
        break
      case 'Описание':
        userStore.setUserFields('description', value)
        break
      case 'Текущий пароль':
        break
      case 'Новый пароль':
        break
      default:
        break
    }
  }

  return (
    <div className={styles.profile_container}>
      <div className={styles.section}>
        <div className={styles.section_title}>Личные данные</div>
        {profileData.PersonalData.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn} onClick={handleClick}>
          Сохранить
        </button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Аватар</div>
        {profileData.Avatar.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn}>Сохранить</button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Телефон</div>
        {profileData.Telephone.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn}>Сохранить</button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Email</div>
        {profileData.Email.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn}>Сохранить</button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Пароль</div>
        {profileData.Password.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn}>Сохранить</button>
      </div>
    </div>
  )
})

export default Profile
