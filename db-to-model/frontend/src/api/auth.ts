import { AxiosResponse } from 'axios'
import { AuthProps, UserInfo } from '../components/Auth/AuthProvider'
import configuredAxios from './config'

export const loginUser = (user: AuthProps) =>
  configuredAxios.post<AuthProps, AxiosResponse<UserInfo>>('/user/login', user)
