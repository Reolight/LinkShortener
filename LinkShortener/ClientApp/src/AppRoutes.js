import { Home } from "./components/Home";
import ShortenedView from "./components/ShortenedView";
import {Layout} from "./components/Layout";

const AppRoutes = [
  {
    path: '/',
    element: <Layout />,
    children: [
      {
        index: true,
        element: <Home/>,
        loader: async ({params}) => {
          const res = await fetch('/links');
          if (!res.ok) {
            console.error('fetching short links failed');
            return;
          }

          return await res.json();
        }
      },
      {
        path: '/url/:link',
        element: <ShortenedView />
      }]
  },
];

export default AppRoutes;
