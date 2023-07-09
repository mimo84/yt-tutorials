import { AuthProps, LoggedInUser } from '../components/Auth/AuthProvider'
import { POST } from './config'

export const loginUser = (user: AuthProps) =>
  POST<LoggedInUser, AuthProps>('/user/login', user)
