import { useState } from 'react'
import styles from './AuthForm.module.css'
import BlueBtn from '../../shared/buttons/BlueBtn'
import { Link } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { observer } from 'mobx-react-lite'
import { useStores } from '../../stores/rootStoreContext.ts'

const AuthForm = observer(({ isLogin }) => {
  const [showSecondForm, setShowSecondForm] = useState<boolean>(false)
  const { authStore } = useStores()
  const generateInputValues = () => ({
    email: '',
    password: '',
    confirmPassword: '',
    fio: '',
    city: '',
    status: '',
  })

  const [values, setValues] = useState(generateInputValues())

  const handleChange = (value: string, name: string) => {
    setValues((prev) => ({ ...prev, [name]: value }))
  }

  const handleNext = (e) => {
    e.preventDefault()
    setShowSecondForm(true)
  }

  const handleSubmit = (e) => {
    e.preventDefault()
    if (isLogin) {
      authStore.login(values.email, values.password)
    } else {
      authStore.register(values.email, values.password, values.fio, values.city, values.status)
    }
  }

  return (
    <form className={styles.register_form} onSubmit={handleSubmit}>
      <div className={styles.form_title}>{isLogin ? 'Вход' : 'Регистрация'}</div>

      {!showSecondForm ? (
        <>
          {!isLogin && (
            <input
              className={styles.form_input}
              placeholder='ФИО'
              type='text'
              name='fio'
              onChange={(e) => handleChange(e.target.value, 'fio')}
              value={values.fio}
            />
          )}
          <input
            className={styles.form_input}
            placeholder='E-mail'
            type='email'
            name='email'
            onChange={(e) => handleChange(e.target.value, 'email')}
            value={values.email}
          />
          <input
            className={styles.form_input}
            placeholder='Пароль'
            type='password'
            name='password'
            onChange={(e) => handleChange(e.target.value, 'password')}
            value={values.password}
          />
          {!isLogin && (
            <input
              className={styles.form_input}
              placeholder='Повторите пароль'
              type='password'
              name='confirmPassword'
              onChange={(e) => handleChange(e.target.value, 'confirmPassword')}
              value={values.confirmPassword}
            />
          )}
          {!isLogin && (
            <Link to={urls.login} className={styles.form_paragraph}>
              Уже есть аккаунт?
            </Link>
          )}
          {/* Кнопка "Далее" должна вызывать handleNext только для регистрации */}
          <BlueBtn btn_placeholder={isLogin ? 'Войти' : 'Далее'} type={isLogin ? 'submit' : 'button'} onClick={isLogin ? handleSubmit : handleNext} />
        </>
      ) : (
        <>
          <input
            className={styles.form_input}
            placeholder='Город'
            type='text'
            name='city'
            onChange={(e) => handleChange(e.target.value, 'city')}
            value={values.city}
          />
          <input
            className={styles.form_input}
            placeholder='Статус'
            type='text'
            name='status'
            onChange={(e) => handleChange(e.target.value, 'status')}
            value={values.status}
          />
          <BlueBtn btn_placeholder={isLogin ? 'Войти' : 'Зарегистрироваться'} type='submit' />
        </>
      )}
    </form>
  )
})

export default AuthForm
