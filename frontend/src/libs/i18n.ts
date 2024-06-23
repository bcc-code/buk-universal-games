import { usePiniaStore } from '@/store/piniaStore';
import { createI18n, type Composer } from 'vue-i18n';

export const SUPPORTED_LOCALES = {
  nb: 'Norsk',
  en: 'English',
  de: 'Deutsch',
  es: 'Español',
  nl: 'Nederlands',
  pl: 'Polski',
  cn: '中文',
  fi: 'Suomi',
  fr: 'Français',
  hu: 'Magyar',
  it: 'Italiano',
  ro: 'Română',
  ru: 'Русский',
  tr: 'Türkçe',
  ua: 'українська',
} as const;

export type Locale = keyof typeof SUPPORTED_LOCALES;

export async function setupI18n() {
  const i18n = createI18n({
    legacy: false,
    globalInjection: true,
    fallbackLocale: 'en',
  });

  const store = usePiniaStore()

  await setI18nLanguage(i18n.global, store.userLanguage);

  return i18n;
}

export async function setI18nLanguage(i18n: Composer, locale: string) {
  let verifiedLocale: Locale;

  if (locale && (locale in SUPPORTED_LOCALES)) {
    verifiedLocale = locale as Locale;
  } else {
    console.warn(`The locale '${locale}' is not supported. Using 'en' as fallback.`);
    verifiedLocale = 'en';
  }

  const messages = await import(`./../locales/${verifiedLocale}.json`);
  i18n.setLocaleMessage(verifiedLocale, messages.default);

  i18n.locale.value = verifiedLocale;

  const html = document.querySelector('html');
  if (!html) throw Error("Couldn't find html element");
  html.setAttribute('lang', verifiedLocale);

  usePiniaStore().setUserLanguage(verifiedLocale)
}
