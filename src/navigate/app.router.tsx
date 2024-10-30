import { createBrowserRouter, RouteObject } from 'react-router-dom';
import appRoutes from "./app.routes.tsx";

const routes = appRoutes.map((r) => ({path: r.path, element: r.element}));

const appRouter = createBrowserRouter([
    {
        children: routes,
    }
])
export default appRouter;