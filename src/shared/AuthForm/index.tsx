import { useState } from 'react'
import styles from './AuthForm.module.css'
import BlueBtn from '../../shared/buttons/BlueBtn'
import { Link } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import Store from '../../store/store.ts'
import { observer } from 'mobx-react-lite'

const AuthForm = observer(({ isLogin }) => {
  const [showSecondForm, setShowSecondForm] = useState<boolean>(false)
  const { register, login } = Store

  const generateInputValues = () => ({
    email: '',
    password: '',
    confirmPassword: '',
    fio: '',
    city: '',
    status: '',
  })

  const [values, setValues] = useState(generateInputValues())

  // Изменение обработчика для обновления значений по имени
  const handleChange = (value: string, name: string) => {
    setValues((prev) => ({ ...prev, [name]: value }))
    console.log({ ...values, [name]: value }) // Логируем обновленные значения
  }

  const handleNext = (e) => {
    e.preventDefault()
    setShowSecondForm(true)
  }

  return (
    <form className={styles.register_form}>
      <div className={styles.form_title}>{isLogin ? 'Вход' : 'Регистрация'}</div>

      {!showSecondForm ? (
        <>
          {!isLogin && (
            <input
              className={styles.form_input}
              placeholder='ФИО'
              type='text'
              name='fio' // Добавлен атрибут name
              onChange={(e) => handleChange(e.target.value, 'fio')} // Передаем имя поля
              value={values.fio} // Привязка значения из состояния
            />
          )}
          <input
            className={styles.form_input}
            placeholder='E-mail'
            type='email'
            name='email' // Добавлен атрибут name
            onChange={(e) => handleChange(e.target.value, 'email')} // Передаем имя поля
            value={values.email} // Привязка значения из состояния
          />
          <input
            className={styles.form_input}
            placeholder='Пароль'
            type='password'
            name='password' // Добавлен атрибут name
            onChange={(e) => handleChange(e.target.value, 'password')} // Передаем имя поля
            value={values.password} // Привязка значения из состояния
          />
          {!isLogin && (
            <input
              className={styles.form_input}
              placeholder='Повторите пароль'
              type='password'
              name='confirmPassword' // Добавлен атрибут name
              onChange={(e) => handleChange(e.target.value, 'confirmPassword')} // Передаем имя поля
              // Здесь можно добавить проверку на совпадение паролей, если необходимо
            />
          )}
          {!isLogin && (
            <Link to={urls.login} className={styles.form_paragraph}>
              Уже есть аккаунт?
            </Link>
          )}
          <BlueBtn btn_placeholder={isLogin ? 'Войти' : 'Далее'} onClick={handleNext} />
        </>
      ) : (
        <>
          <input
            className={styles.form_input}
            placeholder='Город'
            type='text'
            name='city' // Добавлен атрибут name
            onChange={(e) => handleChange(e.target.value, 'city')} // Передаем имя поля
            value={values.city} // Привязка значения из состояния
          />
          <input
            className={styles.form_input}
            placeholder='Статус'
            type='text'
            name='status' // Добавлен атрибут name
            onChange={(e) => handleChange(e.target.value, 'status')} // Передаем имя поля
            value={values.status} // Привязка значения из состояния
          />
          <BlueBtn
            onClick={
              isLogin ? () => login(values.email, values.password) : () => register(values.email, values.password, values.fio, values.city, values.status)
            }
            btn_placeholder={isLogin ? 'Войти' : 'Зарегистрироваться'}
          />
        </>
      )}
    </form>
  )
})

export default AuthForm
