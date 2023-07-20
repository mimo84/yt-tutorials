import { useEffect, useState } from 'react'
import { useAuth } from './useAuth'

export type UserState = 'loggedIn' | 'loggedOut'

const useUserState = () => {
  const auth = useAuth()
  const [userState, setUserState] = useState<UserState>('loggedOut')
  useEffect(() => {
    if (auth.user?.token !== undefined) {
      setUserState('loggedIn')
    } else {
      setUserState('loggedOut')
    }
  }, [auth])

  return userState
}

export default useUserState
