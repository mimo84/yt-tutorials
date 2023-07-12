import { Link, NavLink } from 'react-router-dom'
import { classNames } from '../../../helpers/classNames'
import { useAuth } from '../../Auth/useAuth'

export default function DesktopNavigation() {
  const auth = useAuth()
  const isLoggedIn = auth.user?.token !== undefined
  return (
    <>
      <div className="flex flex-shrink-0 items-center">
        <Link to="/">
          <img
            className="block h-8 w-auto lg:hidden"
            src="https://tailwindui.com/img/logos/mark.svg?color=red&shade=600"
            alt="Your Company"
          />
          <img
            className="hidden h-8 w-auto lg:block"
            src="https://tailwindui.com/img/logos/mark.svg?color=red&shade=600"
            alt="Your Company"
          />
        </Link>
      </div>
      <div className="hidden sm:-my-px sm:ml-6 sm:flex sm:space-x-8">
        <NavLink
          to="about"
          className={({ isActive }) =>
            classNames(
              isActive
                ? 'text-skin-accent border-skin-fill'
                : 'hover:border-background-fill text-skin-accent border-transparent hover:text-skin-hover',
              'inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium'
            )
          }
        >
          About
        </NavLink>
        {!isLoggedIn && (
          <NavLink
            to="login"
            className={({ isActive }) =>
              classNames(
                isActive
                  ? 'text-skin-accent border-skin-fill'
                  : 'hover:border-background-fill text-skin-accent border-transparent hover:text-skin-hover',
                'inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium'
              )
            }
          >
            Login
          </NavLink>
        )}
        <NavLink
          to="/protected/diary"
          className={({ isActive }) =>
            classNames(
              isActive
                ? 'text-skin-accent border-skin-fill'
                : 'hover:border-background-fill text-skin-accent border-transparent hover:text-skin-hover',
              'inline-flex items-center border-b-2 px-1 pt-1 text-sm font-medium'
            )
          }
        >
          Diary
        </NavLink>
        <Link to="login" className="login-link" />
      </div>
    </>
  )
}
