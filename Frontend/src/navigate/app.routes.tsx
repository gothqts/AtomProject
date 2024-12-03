import { urls } from './app.urls.ts'
import { lazy } from 'react'
import { IRoute } from './navigation.types.ts'
import EventDetails from '../screens/EventDetails'

const Register = lazy(() => import('../screens/Register'))
const Login = lazy(() => import('../screens/Login'))
const About = lazy(() => import('../screens/About'))
const Reviews = lazy(() => import('../screens/Reviews'))
const Home = lazy(() => import('../screens/Home'))
const Events = lazy(() => import('../screens/Events'))
const Profile = lazy(() => import('../screens/Profile'))

const appRoutes: IRoute[] = [
  {
    path: urls.register,
    element: <Register />,
  },
  {
    path: urls.login,
    element: <Login />,
  },
  {
    path: urls.home,
    element: <Home />,
  },
  {
    path: urls.about,
    element: <About />,
  },
  {
    path: urls.reviews,
    element: <Reviews />,
  },
  {
    path: urls.events,
    element: <Events />,
  },
  {
    path: urls.profile,
    element: <Profile />,
  },
  {
    path: urls.event,
    element: <EventDetails />,
  },
]
export default appRoutes
