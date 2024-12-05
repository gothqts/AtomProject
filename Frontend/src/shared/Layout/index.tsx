import { Suspense, useState } from 'react'
import { Link, Outlet } from 'react-router-dom'
import { urls } from '../../navigate/app.urls.ts'
import styles from './Layout.module.css'
import LogoIcon from '../../assets/images/Logo.svg?react'
import { observer } from 'mobx-react-lite'
import { useStores } from '../../stores/rootStoreContext.ts'
import DroppedMenu from './DroppedMenu/index.tsx'

const Layout = observer(() => {
  const { authStore } = useStores()
  const avatar = authStore.AuthState.user?.avatarImage
  const name = authStore.AuthState.user?.fio
  const [expanded, setExpanded] = useState<boolean>(false) // Состояние для управления DropDown меню

  const handleNameClick = () => {
    setExpanded((prev) => !prev)
  }

  return (
    <div className={styles.container}>
      <div className={styles.navbar}>
        <Link to={urls.home}>
          <LogoIcon />
        </Link>

        <ul className={styles.link_list}>
          <Link className={styles.link} to={urls.events}>
            Мероприятия
          </Link>
          <Link className={styles.link} to={urls.about}>
            О сервисе
          </Link>
          <Link className={styles.link} to={urls.reviews}>
            Отзывы
          </Link>
        </ul>

        {!authStore.AuthState.isAuth ? (
          <Link className={styles.link_auth} to={urls.register}>
            Вход / Регистрация
          </Link>
        ) : (
          <div className={styles.user_info}>
            <div className={styles.avatar_circle}>
              <img src={avatar} alt='Avatar' className={styles.avatar_image} />
            </div>
            <div className={styles.user_name} onClick={handleNameClick}>
              {name}
            </div>
            {expanded && <DroppedMenu setExpanded={setExpanded} />}
          </div>
        )}
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
