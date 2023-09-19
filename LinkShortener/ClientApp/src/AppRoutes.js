import { Home } from "./components/Home";
import ShortenedView from "./components/ShortenedView";
import {Layout} from "./components/Layout";
import {redirect} from "react-router-dom";

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
        element: <ShortenedView />,
        loader: async ({params}) => {
          let { link } = params;
          const res = await fetch(`/links/${link}`);
          if (!res.ok){
            console.error(`Fetching short link [${link}] failed. Status code: ${res.statusCode}`);
            return redirect('/');
          }
          
          return await res.json();
        }
      }]
  },
];

export default AppRoutes;
