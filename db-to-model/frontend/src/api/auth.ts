import { AuthProps, UserInfo } from '../components/Auth/AuthProvider'
import { POST } from './config'

export const loginUser = (user: AuthProps) =>
  POST<UserInfo, AuthProps>('/user/login', user)
