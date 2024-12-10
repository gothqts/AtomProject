import { Navigate, Outlet, Route } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import { useStores } from '../stores/rootStoreContext.ts'
import { urls } from './app.urls.ts'

const PrivateRoute = (props) => {
  const { authStore } = useStores()
  if (authStore.AuthState.isLoading) {
    return <div>Checking auth...</div>
  }
  if (authStore.AuthState.isAuth) {
    return <Outlet />
  } else {
    return <Navigate to={urls.login} />
  }
}

export default observer(PrivateRoute)
