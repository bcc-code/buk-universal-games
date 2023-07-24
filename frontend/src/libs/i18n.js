import { nextTick } from 'vue'
import { createI18n } from 'vue-i18n'

export const SUPPORT_LOCALES = ['cn', 'de', 'en', 'es', 'fi', 'fr', 'hu', 'it', 'nb', 'nl', 'pl', 'ro', 'ru', 'tr']

export function setupI18n(options = { locale: 'en', }) {
  if (!options.locale || !SUPPORT_LOCALES.includes(options.locale)) {
    console.warn(`The locale '${options.locale}' is not supported. Using 'en' as fallback.`)
    options.locale = 'en'
  }
  options.fallbackLocale = 'en';
  const i18n = createI18n(options)
  loadLocaleMessages(i18n.global, options.fallbackLocale) //load fallback messages
  setI18nLanguage(i18n.global, options.locale)
  return i18n
}

export function setI18nLanguage(i18n, locale) {
  loadLocaleMessages(i18n, locale)
  i18n.locale = locale
  document.querySelector('html').setAttribute('lang', locale)
}

export async function loadLocaleMessages(i18n, locale) {
  // load locale messages with dynamic import
  const messages = await import(`./../locales/${locale}.json`)

  // set locale and locale message
  i18n.setLocaleMessage(locale, messages.default)

  return nextTick()
}
