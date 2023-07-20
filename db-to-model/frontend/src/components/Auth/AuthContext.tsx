import { createContext } from 'react'
import { AuthProps, UserInfo } from './AuthProvider'

interface AuthContextType {
  user: UserInfo | null
  signIn: (user: AuthProps, callback: VoidFunction) => void
  signOut: (callback: VoidFunction) => void
}

const AuthContext = createContext<AuthContextType>(null!)
export default AuthContext
