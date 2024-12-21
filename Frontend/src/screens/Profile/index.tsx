import styles from './Profile.module.css'
import { useStores } from '../../stores/rootStoreContext.ts'
import ProfileInput from '../../shared/ProfileInput/index.tsx'
import { observer } from 'mobx-react-lite'
import { FC, ChangeEvent, useEffect, useState } from 'react'
import { IUser } from '../../models/User/response/User.ts'
import AvatarUploader from './Components/AvatarUploader'
import { IPasswords } from '../../models/User/request/userRequest.ts'

const Profile: FC = observer(() => {
  const { userStore } = useStores()
  const user: IUser | null = userStore.user

  const handleInputChange = (field: keyof IUser) => (event: ChangeEvent<HTMLInputElement>) => {
    const value: string = event.target.value
    userStore.setUserFields(field, value)
  }
  const handlePasswordChange = (field: keyof IPasswords) => (event: ChangeEvent<HTMLInputElement>) => {
    const value = event.target.value
    setPasswords((prevPasswords) => ({
      ...prevPasswords,
      [field]: value,
    }))
  }
  const handleUpdateData = () => {
    const UserData = {
      fio: user.fio,
      status: user.status,
      city: user.city,
      description: user.description,
    }
    console.log(UserData)
    userStore.UpdateData(UserData)
  }
  const handleUpdateTel = () => {
    const telephone = user.phone
    console.log(telephone)
    userStore.UpdateTel(telephone)
  }
  const handleUpdateEmail = () => {
    const email = user.email
    console.log(email)
    userStore.UpdateEmail(email)
  }
  const generatePasswords = (): IPasswords => ({
    currentPassword: '',
    newPassword: '',
  })
  const [passwords, setPasswords] = useState(generatePasswords())
  const handleUpdatePassword = () => {
    const currentPassword = passwords.currentPassword
    const newPassword = passwords.newPassword

    if (currentPassword && newPassword) {
      userStore.UpdatePsw(currentPassword, newPassword)
    } else {
      console.log('Пароли не могут быть пустыми')
    }
  }

  return (
    <div className={styles.profile_container}>
      <div className={styles.section}>
        <div className={styles.section_title}>Личные данные</div>
        <ProfileInput title='ФИО' type='text' value={user.fio} onChange={handleInputChange('fio')} placeholder='Введите ФИО' />
        <ProfileInput
          title='О себе'
          type='text'
          value={userStore.user.description}
          onChange={handleInputChange('description')}
          placeholder='Введите описание'
        />
        <ProfileInput title='Статус' type='text' value={user.status} onChange={handleInputChange('status')} placeholder='Введите статаус' />
        <ProfileInput title='Город' type='text' value={user.city} onChange={handleInputChange('city')} placeholder='Введите город' />
        <button className={styles.update_btn} onClick={handleUpdateData}>
          Сохранить
        </button>
      </div>
      <AvatarUploader />
      <div className={styles.section}>
        <div className={styles.section_title}>Телефон</div>
        <ProfileInput title='Телефон' type='text' value={user.phone} onChange={handleInputChange('phone')} placeholder='Введите телефон' />
        <button className={styles.update_btn} onClick={handleUpdateTel}>
          Сохранить
        </button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Email</div>

        <ProfileInput title='Email' type='email' value={user.email} onChange={handleInputChange('email')} placeholder='Введите email' />

        <button className={styles.update_btn} onClick={handleUpdateEmail}>
          Сохранить
        </button>
      </div>

      <div className={styles.section}>
        <div className={styles.section_title}>Пароль</div>
        <ProfileInput
          title='Текущий пароль'
          type='password'
          value={passwords.currentPassword}
          onChange={handlePasswordChange('currentPassword')}
          placeholder='Текущий пароль'
        />
        <ProfileInput
          title='Новый пароль'
          type='password'
          value={passwords.newPassword}
          onChange={handlePasswordChange('newPassword')}
          placeholder='Новый пароль'
        />
        <button className={styles.update_btn} onClick={handleUpdatePassword}>
          Сохранить
        </button>
      </div>
    </div>
  )
})

export default Profile
