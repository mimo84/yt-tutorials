import {
  HomeIcon,
  PencilSquareIcon,
  UserIcon,
} from '@heroicons/react/24/outline'
import type { UserState } from '../Auth/useUserState'
import useUserState from '../Auth/useUserState'

type NavigationItemVisualize = UserState | 'any'

interface NavigationItem {
  name: string
  to: string
  icon: typeof HomeIcon
  userState: NavigationItemVisualize
}

const navigationItems = [
  {
    name: 'About',
    to: 'about',
    icon: HomeIcon,
    userState: 'any',
  },
  {
    name: 'Diary',
    to: 'protected/diary/show-diary',
    icon: PencilSquareIcon,
    userState: 'loggedIn',
  },
  { name: 'Login', to: 'login', icon: UserIcon, userState: 'loggedOut' },
] as const

const useNavigationItems = (): NavigationItem[] => {
  const userState = useUserState()
  const navlinkToShow = navigationItems.filter(
    (n) => n.userState === 'any' || n.userState === userState,
  )
  return navlinkToShow
}

export default useNavigationItems
