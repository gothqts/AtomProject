import styles from './droppedMenu.module.css'
import { useNavigate } from 'react-router-dom'
import { urls } from '../../../navigate/app.urls.ts'
import { useStores } from '../../../stores/rootStoreContext.ts'
import { PropsWithChildren, useEffect, useRef, useCallback } from 'react'
import LogoutImg from '../../../assets/images/logout-icon.svg?react'
import ProfileImg from '../../../assets/images/user-icon.svg?react'
import MyEventsIcon from '../../../assets/images/myEventsIcon.svg?react'

const DroppedMenu: React.FC<
  PropsWithChildren<{
    setExpanded: (boolean) => void
  }>
> = ({ setExpanded }) => {
  const { authStore } = useStores()
  const menuRef = useRef(null)
  const navigate = useNavigate()
  const userID = authStore.AuthState.user?.id

  const handleClickOutside = useCallback(
    (event) => {
      if (menuRef.current && !menuRef.current.contains(event.target)) {
        setExpanded(false)
      }
    },
    [setExpanded]
  )

  useEffect(() => {
    document.addEventListener('mousedown', handleClickOutside)

    // Удаляем обработчик при размонтировании
    return () => {
      document.removeEventListener('mousedown', handleClickOutside)
    }
  }, [handleClickOutside])

  return (
    <div ref={menuRef} className={styles.container}>
      <button className={styles.profile_btn} onClick={() => navigate(`/profile/ ${userID}`)}>
        <ProfileImg />
        <span style={{ marginLeft: '21px' }}>Профиль</span>
      </button>
      <button className={styles.logout_btn} type='submit' onClick={() => authStore.logout()}>
        <LogoutImg style={{ marginLeft: '4px' }} />
        <span>Выход</span>
      </button>
      <button className={styles.logout_btn} type='submit' onClick={() => navigate(urls.myEvents)}>
        <MyEventsIcon style={{ marginLeft: '4px' }} />
        <span>Мои мероприятия</span>
      </button>
    </div>
  )
}

export default DroppedMenu
