import { initReactI18next } from 'react-i18next'
import i18n from 'i18next'
import LanguageDetector from 'i18next-browser-languagedetector'
import Backend from 'i18next-http-backend'

import en from '../locales/en/translation.json'
import it from '../locales/it/translation.json'
import br from '../locales/br/translation.json'

export const resources = {
  en: { translation: en },
  it: { translation: it },
  br: { translation: br },
}

export const defaultNS = 'translation'

// don't want to use this?
// have a look at the Quick start guide
// for passing in lng and translations on init

void i18n
  // load translation using http -> see /assets/locales (i.e. https://github.com/i18next/react-i18next/tree/master/example/react/assets/locales)
  // learn more: https://github.com/i18next/i18next-http-backend
  // want your translations to be loaded from a professional CDN? => https://github.com/locize/react-tutorial#step-2---use-the-locize-cdn
  .use(Backend)
  // detect user language
  // learn more: https://github.com/i18next/i18next-browser-languageDetector
  .use(LanguageDetector)
  // pass the i18n instance to react-i18next.
  .use(initReactI18next)
  // init i18next
  // for all options read: https://www.i18next.com/overview/configuration-options
  .init({
    returnNull: false,
    fallbackLng: 'en',
    debug: true,
    supportedLngs: ['en', 'it', 'br'],
    ns: ['translation'],
    defaultNS,
    load: 'languageOnly',
    nonExplicitSupportedLngs: true,
    interpolation: {
      escapeValue: false, // not needed for react as it escapes by default
    },
    resources,
  })

export default i18n