import {
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
  Route,
} from 'react-router-dom'
import Layout from './components/Layout/Layout'
import { AuthProvider } from './components/Auth/AuthProvider'
import RequireAuth from './components/Auth/RequireAuth'
import Home from './pages/home/Home'
import About from './pages/about/About'
import Login from './pages/login/Login'
import { Diary } from './pages/diary/Diary'
import NotFound from './pages/navigation/notFound/NotFound'

const router = createBrowserRouter(
  createRoutesFromElements(
    <Route path="/" element={<Layout />}>
      <Route index element={<Home />} />
      <Route path="about" element={<About />} />
      <Route path="login" element={<Login />} />
      <Route
        path="/protected/diary"
        element={
          <RequireAuth>
            <Diary />
          </RequireAuth>
        }
      />
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      <Route path="*" element={<NotFound />} />
    </Route>,
  ),
)

export function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  )
}
