import axios from 'axios'

axios.defaults.baseURL = import.meta.env.VITE_API_URL

axios.interceptors.request.use((config) => {
  const token = localStorage.getItem('jwtToken')
  if (token && config.headers) config.headers.Authorization = `Bearer ${token}`
  return config
})

const configuredAxios = axios
export default configuredAxios
