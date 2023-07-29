import { useNavigate, useLocation } from 'react-router-dom'
import { AuthProps } from '../../components/Auth/AuthProvider'
import { useAuth } from '../../components/Auth/useAuth'
import imgUrl from '../../images/healthy-food-bg-white.jpg'
import { useTranslation } from 'react-i18next'
interface LocationState {
  from?: {
    pathname?: string
  }
}
export default function Login() {
  const navigate = useNavigate()
  const location = useLocation()
  const auth = useAuth()
  const { t } = useTranslation()
  const state = location.state as LocationState
  const from = state?.from?.pathname || '/'

  function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault()

    const formData = new FormData(event.currentTarget)
    const email = formData.get('email') as string
    const password = formData.get('password') as string
    const siginProps: AuthProps = { email, password }
    auth.signIn(siginProps, () => {
      navigate(from, { replace: true })
    })
  }

  return (
    <>
      <div className="flex min-h-full flex-1">
        <div className="flex flex-1 flex-col justify-center px-4 py-12 sm:px-6 lg:flex-none lg:px-20 xl:px-24">
          <div className="mx-auto w-full max-w-sm lg:w-96">
            <div>
              <img
                className="h-10 w-auto"
                src="https://tailwindui.com/img/logos/mark.svg?color=indigo&shade=600"
                alt="Your Company"
              />
              <h2 className="mt-8 text-2xl font-bold leading-9 tracking-tight text-gray-900">
                {t('login.signin')}
              </h2>
            </div>

            <div className="mt-10">
              <div>
                <form className="space-y-6" onSubmit={handleSubmit}>
                  <div>
                    <label
                      htmlFor="email"
                      className="block text-sm font-medium leading-6 text-gray-900"
                    >
                      {t('login.email-label')}
                    </label>
                    <div className="mt-2">
                      <input
                        id="email"
                        name="email"
                        type="email"
                        autoComplete="email"
                        required
                        className="block w-full rounded-md border-0 py-1.5 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                      />
                    </div>
                  </div>

                  <div>
                    <label
                      htmlFor="password"
                      className="block text-sm font-medium leading-6 text-gray-900"
                    >
                      {t('login.password-label')}
                    </label>
                    <div className="mt-2">
                      <input
                        id="password"
                        name="password"
                        type="password"
                        defaultValue="MyT3stPa$$w0rd"
                        autoComplete="current-password"
                        required
                        className="block w-full rounded-md border-0 py-1.5 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                      />
                    </div>
                  </div>

                  <div>
                    <button
                      type="submit"
                      className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
                    >
                      {t('login.signin-button')}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
        <div className="relative hidden w-0 flex-1 lg:block">
          <img
            className="absolute inset-0 h-full w-full object-cover"
            src={imgUrl}
            alt=""
          />
          <a href="https://www.freepik.com/free-photo/buddha-bowl-dish-with-vegetables-legumes-top-view_13807905.htm#query=healthy%20food&position=1&from_view=search&track=ais">
            Image by jcomp
          </a>{' '}
          on Freepik
        </div>
      </div>
    </>
  )
}
