import {
  HomeIcon,
  PencilSquareIcon,
  UserIcon,
} from '@heroicons/react/24/outline'
import type { UserState } from '../Auth/useUserState'
import useUserState from '../Auth/useUserState'
import { useTranslation } from 'react-i18next'

type NavigationItemVisualize = UserState | 'any'

interface NavigationItem {
  name: string
  to: string
  icon: typeof HomeIcon
  userState: NavigationItemVisualize
}

const useNavigationItems = (): NavigationItem[] => {
  const userState = useUserState()
  const { t } = useTranslation()
  const navigationItems = [
    {
      name: t('navigation.about'),
      to: 'about',
      icon: HomeIcon,
      userState: 'any',
    },
    {
      name: t('navigation.diary'),
      to: 'protected/diary/show-diary',
      icon: PencilSquareIcon,
      userState: 'loggedIn',
    },
    {
      name: t('navigation.login'),
      to: 'login',
      icon: UserIcon,
      userState: 'loggedOut',
    },
  ] as const
  const navlinkToShow = navigationItems.filter(
    (n) => n.userState === 'any' || n.userState === userState,
  )
  return navlinkToShow
}

export default useNavigationItems
