import { createContext } from 'react'
import { AuthProps, LoggedInUser } from './AuthProvider'

interface AuthContextType {
  user: LoggedInUser | null
  signIn: (user: AuthProps, callback: VoidFunction) => void
  signOut: (callback: VoidFunction) => void
}

const AuthContext = createContext<AuthContextType>(null!)
export default AuthContext
