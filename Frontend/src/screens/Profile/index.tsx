import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'
import ProfileInput from '../../shared/ProfileInput/index.tsx'
import { observer } from 'mobx-react-lite'
import { FC, useState, useEffect } from 'react'
import { c } from 'vite/dist/node/types.d-aGj9QkWt'

interface IUpdatedData {
  fio: string
  description: string
  status: string
  city: string
}
interface ProfileField {
  title: string
  type: string
  value: string | null | undefined
}

interface ProfileData {
  PersonalData: ProfileField[]
  Avatar: ProfileField[]
  Telephone: ProfileField[]
  Email: ProfileField[]
  Password: ProfileField[]
}
const Profile: FC = observer(() => {
  const { userStore } = useStores()
  const { user } = userStore

  const generateProfileData = (): ProfileData => ({
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
      { title: 'Текущий пароль', type: 'password', value: user?.CurrentPassword },
      { title: 'Новый пароль', type: 'password', value: user?.NewPassword },
    ],
  })

  const [profileData, setProfileData] = useState(generateProfileData())

  useEffect(() => {
    setProfileData(generateProfileData())
  }, [user])

  const handleUpdatePersonalData = () => {
    const updatedData: IUpdatedData = {
      fio: profileData.PersonalData.find((field): boolean => field.title === 'ФИО')?.value || '',
      description: profileData.PersonalData.find((field): boolean => field.title === 'Описание')?.value || '',
      status: profileData.PersonalData.find((field): boolean => field.title === 'Статус')?.value || '',
      city: profileData.PersonalData.find((field): boolean => field.title === 'Город')?.value || '',
    }
    userStore.UpdateData(updatedData)
  }

  const handleUpdateTel = () => {
    const telephone: string | null = user.phone
    console.log(telephone)
    userStore.UpdateTel('+79222222222')
  }
  const handleUpdateEmail = () => {
    const email: string | null = user.email
    console.log(email)
    userStore.UpdateEmail(email)
  }
  const handleUpdatePassword = () => {
    const currentPassword = user.CurrentPassword
    const newPassword = user.NewPassword

    // Проверка на наличие паролей перед обновлением
    if (currentPassword && newPassword) {
      console.log(currentPassword, newPassword)
      userStore.UpdatePsw(currentPassword, newPassword)
    } else {
      console.error('Пароли не могут быть пустыми')
    }
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
        userStore.setUserFields('CurrentPassword', value)
        break
      case 'Новый пароль':
        userStore.setUserFields('NewPassword', value)
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
        <button className={styles.update_btn} onClick={handleUpdatePersonalData}>
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
        <button className={styles.update_btn} onClick={handleUpdateTel}>
          Сохранить
        </button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Email</div>
        {profileData.Email.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn} onClick={handleUpdateEmail}>
          Сохранить
        </button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Пароль</div>
        {profileData.Password.map((field) => (
          <ProfileInput key={field.title} title={field.title} type={field.type} value={field.value} onChange={handleInputChange(field.title)} />
        ))}
        <button className={styles.update_btn} onClick={handleUpdatePassword}>
          Сохранить
        </button>
      </div>
    </div>
  )
})

export default Profile
