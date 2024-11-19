import { createBrowserRouter, RouteObject } from 'react-router-dom';
import appRoutes from "./app.routes";
import Layout from "../shared/Layout";

const routes : RouteObject[] = appRoutes.map((r) => ({path: r.path, element: r.element}));

const appRouter = createBrowserRouter([
    {
        element: <Layout/>,
        children: routes,
    }
])
export default appRouter;