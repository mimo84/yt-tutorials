import { ReactNode, useState } from 'react'
import AuthContext from './AuthContext'
import { loginUser } from '../../api/auth'

export interface AuthProps {
  email: string
  password: string
}

export interface UserInfo {
  displayName: string
  token: string
  bio: string
  firstName: string
  familyName: string
  address: string
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserInfo | null>(null)

  const signIn = async (user: AuthProps, callback: VoidFunction) => {
    try {
      const { data } = await loginUser(user)

      if (!data) {
        return
      }
      localStorage.setItem('jwtToken', data.token)

      setUser(data)
    } catch (e) {
      console.log('something went wrong!')
    }
    callback()
  }

  const signOut = (callback: VoidFunction) => {
    setUser(null)
    callback()
  }

  const value = { user, signIn, signOut }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
