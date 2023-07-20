import { NavLink } from 'react-router-dom'
import { classNames } from '../../../helpers/classNames'
import useNavigationItems from '../useNavigationItems'

const DesktopNavigation = () => {
  const navigationItems = useNavigationItems()
  const fakeActive = false
  return (
    <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-cyan-50 px-6">
      <div className="flex h-16 shrink-0 items-center">
        <img
          className="h-8 w-auto"
          src="https://tailwindui.com/img/logos/mark.svg?color=slate-300"
          alt="Your Company"
        />
      </div>
      <nav className="flex flex-1 flex-col">
        <ul role="list" className="flex flex-1 flex-col gap-y-7">
          <li>
            <ul role="list" className="-mx-2 space-y-1">
              {navigationItems.map((item) => (
                <li key={item.name}>
                  <NavLink
                    to={item.to}
                    className={({ isActive }) =>
                      classNames(
                        isActive
                          ? 'is-active bg-slate-600 text-white'
                          : 'text-slate-600 hover:bg-slate-400 hover:text-white',
                        'group flex gap-x-3 rounded-md p-2 text-sm font-semibold leading-6',
                      )
                    }
                  >
                    <item.icon
                      className={
                        'h-6 w-6 shrink-0 text-slate-600 group-hover:text-zinc-100 group-[.is-active]:text-zinc-100'
                      }
                      aria-hidden="true"
                    />
                    {item.name}
                  </NavLink>
                </li>
              ))}
            </ul>
          </li>

          <li className="-mx-6 mt-auto">
            <a
              href="#"
              className="flex items-center gap-x-4 px-6 py-3 text-sm font-semibold leading-6 text-slate-600 hover:bg-slate-700 hover:text-white"
            >
              <img
                className="h-8 w-8 rounded-full bg-slate-700"
                src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                alt=""
              />
              <span className="sr-only">Your profile</span>
              <span aria-hidden="true">Tom Cook</span>
            </a>
          </li>
        </ul>
      </nav>
    </div>
  )
}

export default DesktopNavigation
