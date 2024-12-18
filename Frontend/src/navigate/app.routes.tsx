import { urls } from './app.urls.ts'
import { lazy } from 'react'
import { IRoute } from './navigation.types.ts'
import PrivateRoute from './PrivateRoute.tsx'

const Register = lazy(() => import('../screens/Register'))
const Login = lazy(() => import('../screens/Login'))
const About = lazy(() => import('../screens/About'))
const Reviews = lazy(() => import('../screens/Reviews'))
const Home = lazy(() => import('../screens/Home'))
const Events = lazy(() => import('../screens/Events'))
const Profile = lazy(() => import('../screens/Profile'))
const EventDetails = lazy(() => import('../screens/EventDetails'))
const MyEvents = lazy(() => import('../screens/MyEvents'))
const CreateEvent = lazy(() => import('../screens/CreateEvent'))

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
    element: (
      <PrivateRoute>
        <Profile />
      </PrivateRoute>
    ),
  },
  {
    path: urls.event,
    element: <EventDetails />,
  },
  {
    path: urls.myEvents,
    element: (
      <PrivateRoute>
        <MyEvents />
      </PrivateRoute>
    ),
  },
  {
    path: urls.createEvent,
    element: (
      <PrivateRoute>
        <CreateEvent />
      </PrivateRoute>
    ),
  },
]
export default appRoutes
