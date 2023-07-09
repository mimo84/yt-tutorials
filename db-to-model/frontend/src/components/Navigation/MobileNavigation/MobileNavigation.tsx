import { Disclosure } from '@headlessui/react'
import { classNames } from '../../../helpers/classNames'
import { useAuth } from '../../Auth/useAuth'
import { BellIcon } from '@heroicons/react/20/solid'
import { useLocation, useNavigate } from 'react-router-dom'

// to be kept as temporary placeholder
const userNavigation = [
  { name: 'Your Profile', href: '#' },
  { name: 'Settings', href: '#' },
  { name: 'Sign out', href: '#' },
]
export function MobileNavigation() {
  const auth = useAuth()
  const location = useLocation()
  const navigate = useNavigate()

  return (
    <Disclosure.Panel className="sm:hidden">
      <div className="space-y-1 pb-3 pt-2">
        {/* TODO: Doesn't work. I need to find a way to use an hook */}
        <Disclosure.Button
          onClick={() => navigate('/about')}
          as="a"
          className={classNames(
            location.pathname === '/about'
              ? 'border-indigo-500 bg-indigo-50 text-indigo-700'
              : 'border-transparent text-gray-600 hover:border-gray-300 hover:bg-gray-50 hover:text-gray-800',
            'block border-l-4 py-2 pl-3 pr-4 text-base font-medium'
          )}
          aria-current={location.pathname === '/about' ? 'page' : undefined}
        >
          About
        </Disclosure.Button>
        <Disclosure.Button
          onClick={() => navigate('/login')}
          as="a"
          className={classNames(
            location.pathname === '/login'
              ? 'border-indigo-500 bg-indigo-50 text-indigo-700'
              : 'border-transparent text-gray-600 hover:border-gray-300 hover:bg-gray-50 hover:text-gray-800',
            'block border-l-4 py-2 pl-3 pr-4 text-base font-medium'
          )}
          aria-current={location.pathname === '/login' ? 'page' : undefined}
        >
          Login
        </Disclosure.Button>
        <Disclosure.Button
          onClick={() => navigate('/protected/diary')}
          as="a"
          className={classNames(
            location.pathname === '/protected/diary'
              ? 'border-indigo-500 bg-indigo-50 text-indigo-700'
              : 'border-transparent text-gray-600 hover:border-gray-300 hover:bg-gray-50 hover:text-gray-800',
            'block border-l-4 py-2 pl-3 pr-4 text-base font-medium'
          )}
          aria-current={
            location.pathname === '/protected/diary' ? 'page' : undefined
          }
        >
          Diary
        </Disclosure.Button>
      </div>
      <div className="border-t border-gray-200 pb-3 pt-4">
        <div className="flex items-center px-4">
          <div className="flex-shrink-0">
            <img
              className="h-10 w-10 rounded-full"
              src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
              alt=""
            />
          </div>
          <div className="ml-3">
            <div className="text-base font-medium text-gray-800">
              {auth.user?.displayName}
            </div>
            <div className="text-sm font-medium text-gray-500">
              {auth.user?.firstName}
            </div>
          </div>
          <button
            type="button"
            className="ml-auto flex-shrink-0 rounded-full bg-white p-1 text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
          >
            <span className="sr-only">View notifications</span>
            <BellIcon className="h-6 w-6" aria-hidden="true" />
          </button>
        </div>
        <div className="mt-3 space-y-1">
          {userNavigation.map((item) => (
            <Disclosure.Button
              key={item.name}
              as="a"
              href={item.href}
              className="block px-4 py-2 text-base font-medium text-gray-500 hover:bg-gray-100 hover:text-gray-800"
            >
              {item.name}
            </Disclosure.Button>
          ))}
        </div>
      </div>
    </Disclosure.Panel>
  )
}
