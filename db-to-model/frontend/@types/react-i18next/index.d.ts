import 'react-i18next'

import type { resources } from '../../src/utlis/i18n'

declare module 'react-i18next' {
  interface CustomTypeOptions {
    defaultNS: 'translation'
    resources: (typeof resources)['en']
  }
}
