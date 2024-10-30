import {urls} from './app.urls.ts';
import {lazy} from "react";
import {IRoute} from "./navigation.types.ts";
import PrivateRoute from "./PrivateRoutes.tsx";

const Register = lazy(() => import('../screens/Register/index.tsx'));
const Login = lazy(()=> import('../screens/Login/index.tsx'));
const About = lazy(() => import('../screens/About/index.tsx'));
const Reviews = lazy(() => import('../screens/Reviews/index.tsx'));
const Home = lazy(() => import('../screens/Home/index.tsx'));
const Events = lazy(() => import('../screens/Events/index.tsx'));


const appRoutes: IRoute[] = [
    {
        path: urls.register,
        element: <Register/>,
    },
    {
        path: urls.login,
        element: <Login/>
    },
    {
        path: urls.home,
        element: <Home/>
    },
    {
        path: urls.about,
        element: <About/>,
    },
    {
        path: urls.reviews,
        element: <Reviews/>
    },
    {
        path: urls.events,
        element: <Events/>
    }
]
export default appRoutes;