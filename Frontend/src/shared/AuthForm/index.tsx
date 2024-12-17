import { useState } from 'react'
import styles from './AuthForm.module.css'
import BlueBtn from '../../shared/buttons/BlueBtn'
import { Link, useNavigate } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import { observer } from 'mobx-react-lite'
import { useStores } from '../../stores/rootStoreContext.ts'
import { Form } from 'react-router-dom'
import { useForm } from 'react-hook-form'

const AuthForm = observer(({ isLogin }) => {
  const [showSecondForm, setShowSecondForm] = useState<boolean>(false)
  const { authStore } = useStores()
  const navigate = useNavigate()

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm()

  const onSubmit = (data) => {
    if (isLogin) {
      authStore.login(data.email, data.password)
    } else {
      authStore.register(data.email, data.password, data.fio, data.city, data.status)
    }
  }

  const handleNext = () => {
    const result = handleSubmit((data) => {
      setShowSecondForm(true)
    })

    // Отключаем возможность переключения формы, пока есть ошибки валидации
    result()
  }

  return (
    <Form className={styles.register_form} onSubmit={handleSubmit(onSubmit)} action={urls.events}>
      <div className={styles.form_title}>{isLogin ? 'Вход' : 'Регистрация'}</div>

      {!showSecondForm ? (
        <>
          {!isLogin && (
            <>
              <input className={styles.form_input} placeholder='ФИО' type='text' {...register('fio', { required: !isLogin && 'ФИО не может быть пустым' })} />
              {errors.fio && <span className={styles.error_message}>{errors.fio.message}</span>}
            </>
          )}
          <input
            className={styles.form_input}
            placeholder='E-mail'
            type='email'
            {...register('email', {
              required: 'Email не может быть пустым',
              pattern: { value: /^\S+@\S+$/, message: 'Некорректный Email' },
            })}
          />
          {errors.email && <span className={styles.error_message}>{errors.email.message}</span>}
          <input className={styles.form_input} placeholder='Пароль' type='password' {...register('password', { required: 'Пароль не может быть пустым' })} />
          {errors.password && <span className={styles.error_message}>{errors.password.message}</span>}

          {!isLogin && (
            <>
              <input
                className={styles.form_input}
                placeholder='Повторите пароль'
                type='password'
                {...register('confirmPassword', {
                  required: 'Подтверждение пароля обязательно',
                  validate: (value) => value === watch('password') || 'Пароли не совпадают',
                })}
              />
              {errors.confirmPassword && <span className={styles.error_message}>{errors.confirmPassword.message}</span>}
            </>
          )}
          {!isLogin && (
            <Link to={urls.login} className={styles.form_paragraph}>
              Уже есть аккаунт?
            </Link>
          )}
          <BlueBtn
            btn_placeholder={isLogin ? 'Войти' : 'Далее'}
            type={isLogin ? 'submit' : 'button'}
            onClick={isLogin ? undefined : handleNext} // Меняем обработчик клика
          />
        </>
      ) : (
        <>
          <input className={styles.form_input} placeholder='Город' type='text' {...register('city', { required: 'Город не может быть пустым' })} />
          {errors.city && <span className={styles.error_message}>{errors.city.message}</span>}
          <input className={styles.form_input} placeholder='Статус' type='text' {...register('status', { required: 'Статус не может быть пустым' })} />
          {errors.status && <span className={styles.error_message}>{errors.status.message}</span>}
          <BlueBtn btn_placeholder={isLogin ? 'Войти' : 'Зарегистрироваться'} type='submit' />
        </>
      )}
    </Form>
  )
})

export default AuthForm
