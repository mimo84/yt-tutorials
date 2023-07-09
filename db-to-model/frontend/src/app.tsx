import About from './pages/About'
import { Diary } from './pages/Diary'
import {
  RouterProvider,
  createBrowserRouter,
  createRoutesFromElements,
  Route,
} from 'react-router-dom'
import Home from './pages/Home'
import Layout from './components/Layout/Layout'
import NotFound from './pages/NotFound'
import Login from './pages/Login'
import { AuthProvider } from './components/Auth/AuthProvider'
import RequireAuth from './components/Auth/RequireAuth'

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
    </Route>
  )
)

export function App() {
  return (
    <AuthProvider>
      <RouterProvider router={router} />
    </AuthProvider>
  )
}
