import { classNames } from '../../../helpers/classNames'
import { NavLink } from 'react-router-dom'
import useNavigationItems from '../useNavigationItems'

const fakeActive = true

const MobileNavigation = () => {
  const navigationItems = useNavigationItems()

  return (
    <div className="flex grow flex-col gap-y-5 overflow-y-auto bg-cyan-50 px-6 pb-2">
      <div className="flex h-16 shrink-0 items-center">
        <img
          className="h-8 w-auto"
          src="https://tailwindui.com/img/logos/mark.svg?color=slate-400"
          alt="Your Company"
        />
      </div>
      <nav className="flex flex-1 flex-col">
        <ul role="list" className="flex flex-1 flex-col gap-y-7">
          <li>
            <ul role="list" className="-mx-2 space-y-1">
              {navigationItems.map((ni) => {
                return (
                  <li key={ni.to}>
                    <NavLink
                      to={ni.to}
                      className={({ isActive }) =>
                        classNames(
                          isActive
                            ? 'bg-indigo-700 text-white'
                            : 'text-indigo-200 hover:bg-indigo-700 hover:text-white',
                          'group flex gap-x-3 rounded-md p-2 text-sm font-semibold leading-6',
                        )
                      }
                    >
                      <ni.icon
                        className={classNames(
                          fakeActive
                            ? 'text-white'
                            : 'text-indigo-200 group-hover:text-white',
                          'h-6 w-6 shrink-0',
                        )}
                        aria-hidden="true"
                      />
                      {ni.name}
                    </NavLink>
                  </li>
                )
              })}
            </ul>
          </li>
        </ul>
      </nav>
    </div>
  )
}

export default MobileNavigation
