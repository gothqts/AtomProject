import LogoIcon from '../../assets/images/Logo.svg?react'
import { Link, Outlet } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import styles from './Layout.module.css'
import { Suspense, useContext, useEffect } from 'react'
import { observer } from 'mobx-react-lite'
import { Context } from '../../index.tsx'

const Layout = observer(() => {
  const { store } = useContext(Context)
  return (
    <div className={styles.container}>
      <div className={styles.navbar}>
        <Link to={urls.home}>
          <LogoIcon />
        </Link>

        <li className={styles.link_list}>
          <Link className={styles.link} to={urls.events}>
            Мероприятия
          </Link>
          <Link className={styles.link} to={urls.about}>
            О сервисе
          </Link>
          <Link className={styles.link} to={urls.reviews}>
            Отзывы
          </Link>
        </li>

        <Link className={styles.link_auth} to={urls.register}>
          Вход / Регистрация
        </Link>
        <button type='submit' onClick={() => store.logout()}>
          Выйти из аккаунта
        </button>
        <button type='submit' onClick={() => store.RefreshTokens()}>
          Обновить токен
        </button>
      </div>
      <div className={styles.page_content}>
        <Suspense fallback={<p>Loading...</p>}>
          <Outlet />
        </Suspense>
      </div>
    </div>
  )
})

export default Layout
