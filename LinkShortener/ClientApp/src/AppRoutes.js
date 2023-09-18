import { Home } from "./components/Home";
import ShortenedView from "./components/ShortenedView";

const AppRoutes = [
  {
    index: true,
    element: <Home />
  },
  {
    path: '/url/:link',
    element: <ShortenedView />
  }
];

export default AppRoutes;
