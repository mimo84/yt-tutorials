import axios, { AxiosRequestConfig } from 'axios'

interface fetchWrapProps<T> {
  method: 'get' | 'post' | 'put' | 'delete'
  url: string
  body?: T
  signal?: AbortSignal
}

const fetchWrap = async <T, B = void>({
  method,
  url,
  body,
  signal,
}: fetchWrapProps<B>) => {
  const jwtToken = localStorage.getItem('jwtToken')
  const config: AxiosRequestConfig = {
    baseURL: 'http://localhost:5066',
    headers: {
      Authorization: jwtToken ? `Bearer ${jwtToken}` : '',
    },
    signal: signal,
  }

  const { data } =
    (method === 'get' && (await axios.get<T>(url, config))) ||
    (method === 'post' && (await axios.post<T>(url, body, config))) ||
    (method === 'put' && (await axios.put<T>(url, body, config))) ||
    (method === 'delete' && (await axios.delete<T>(url, config))) ||
    {}
  return data
}

export const GET = <T>(url: string, signal?: AbortSignal) =>
  fetchWrap<T>({ method: 'get', url, signal })

export const POST = <T, B>(url: string, body: B) =>
  fetchWrap<T, B>({ method: 'post', url, body })

export const PUT = <T, B>(url: string, body: B) =>
  fetchWrap<T, B>({ method: 'put', url, body })

export const DELETE = <T>(url: string) =>
  fetchWrap<T>({ method: 'delete', url })
