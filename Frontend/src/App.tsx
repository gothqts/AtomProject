import { useEffect } from 'react'
import { urls } from './navigate/app.urls.ts'
import { RouterProvider } from 'react-router-dom'
import appRouter from './navigate/app.router.tsx'
import { useStores } from './stores/rootStoreContext.ts'

const App = () => {
  const { authStore } = useStores()
  useEffect(() => {
    if (localStorage.getItem('token')) {
      authStore.checkAuth()
    }
    if (location.pathname === '/') {
      location.replace(urls.home)
    }
  }, [])

  return <RouterProvider router={appRouter} />
}

export default App