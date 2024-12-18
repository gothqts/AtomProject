import { Navigate, useLocation } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { urls } from './app.urls.ts'

const PrivateRoute = ({ children }) => {
  let location = useLocation()
  if (!localStorage.getItem('token')) {
    return <Navigate to={urls.login} state={{ from: location }} replace />
  }
  return children
  // const { authStore } = useStores()
  // if (authStore.AuthState.isLoading) {
  //   return <div>Checking auth...</div>
  // }
  // if (authStore.AuthState.isAuth) {
  //   return <Outlet />
  // } else {
  //   console.log(authStore.AuthState.isAuth)
  //   return <Navigate to={urls.login} />
  // }
}

export default observer(PrivateRoute)
