import { Outlet } from 'react-router-dom'
import { Fragment, useState } from 'react'
import { Dialog, Transition } from '@headlessui/react'
import { Bars3Icon, XMarkIcon } from '@heroicons/react/24/outline'

import Footer from '../Footer/Footer'
import MobileNavigation from '../Navigation/MobileNavigation/MobileNavigation'
import DesktopNavigation from '../Navigation/DesktopNavigation/DesktopNavigation'
import { useTranslation } from 'react-i18next'
import usFlag from '../../images/icons/flag-us.svg'
import itFlag from '../../images/icons/flag-it.svg'
import brFlag from '../../images/icons/flag-br.svg'

export default function Layout() {
  const [sidebarOpen, setSidebarOpen] = useState<boolean>(false)
  const { i18n } = useTranslation()
  const changeLanguage = (lng: string): void => {
    void i18n.changeLanguage(lng)
  }
  return (
    <>
      <div>
        <Transition.Root show={sidebarOpen} as={Fragment}>
          <Dialog
            as="div"
            className="relative z-50 lg:hidden"
            onClose={setSidebarOpen}
          >
            <Transition.Child
              as={Fragment}
              enter="transition-opacity ease-linear duration-300"
              enterFrom="opacity-0"
              enterTo="opacity-100"
              leave="transition-opacity ease-linear duration-300"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              {/* grey bg when opening the sidebar on mobile */}
              <div className="fixed inset-0 bg-gray-900/80" />
            </Transition.Child>

            <div className="fixed inset-0 flex">
              <Transition.Child
                as={Fragment}
                enter="transition ease-in-out duration-300 transform"
                enterFrom="-translate-x-full"
                enterTo="translate-x-0"
                leave="transition ease-in-out duration-300 transform"
                leaveFrom="translate-x-0"
                leaveTo="-translate-x-full"
              >
                <Dialog.Panel className="relative mr-16 flex w-full max-w-xs flex-1">
                  <Transition.Child
                    as={Fragment}
                    enter="ease-in-out duration-300"
                    enterFrom="opacity-0"
                    enterTo="opacity-100"
                    leave="ease-in-out duration-300"
                    leaveFrom="opacity-100"
                    leaveTo="opacity-0"
                  >
                    <div className="absolute left-full top-0 flex w-16 justify-center pt-5">
                      <button
                        type="button"
                        className="-m-2.5 p-2.5"
                        onClick={() => setSidebarOpen(false)}
                      >
                        <span className="sr-only">Close sidebar</span>
                        <XMarkIcon
                          className="h-6 w-6 text-white"
                          aria-hidden="true"
                        />
                      </button>
                    </div>
                  </Transition.Child>
                  <MobileNavigation />
                </Dialog.Panel>
              </Transition.Child>
            </div>
          </Dialog>
        </Transition.Root>

        {/* Static sidebar for desktop */}
        <div className="hidden lg:fixed lg:inset-y-0 lg:z-50 lg:flex lg:w-72 lg:flex-col">
          <DesktopNavigation />
        </div>

        <div className="sticky top-0 z-40 flex items-center gap-x-6 bg-cyan-50 px-4 py-4 shadow-sm sm:px-6 lg:hidden">
          <button
            type="button"
            className="-m-2.5 p-2.5 text-slate-600 lg:hidden"
            onClick={() => setSidebarOpen(true)}
          >
            <span className="sr-only">Open sidebar</span>
            <Bars3Icon className="h-6 w-6" aria-hidden="true" />
          </button>
          <div className="flex flex-1 justify-end gap-2 text-sm font-semibold leading-6 text-slate-900">
            <button
              type="button"
              className="h-7 w-7"
              onClick={() => changeLanguage('br')}
            >
              <img src={brFlag} alt="Lingua Brasileira" />
            </button>
            <button
              type="button"
              className="h-7 w-7"
              onClick={() => changeLanguage('it')}
            >
              <img src={itFlag} alt="Lingua Italiana" />
            </button>
            <button
              className="h-7 w-7"
              type="button"
              onClick={() => changeLanguage('en')}
            >
              <img src={usFlag} alt="English Language" />
            </button>
          </div>
        </div>

        <main className="py-10 lg:pl-72">
          <div className="px-4 sm:px-6 lg:px-8">
            <Outlet />
            <Footer />
          </div>
        </main>
      </div>
    </>
  )
}
