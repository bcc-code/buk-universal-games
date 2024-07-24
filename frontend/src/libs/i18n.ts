import { usePiniaStore } from '@/store/piniaStore';
import { createI18n, type Composer } from 'vue-i18n';

export const SUPPORTED_LOCALES = {
  nb: 'Norwegian',
  en: 'English',
  de: 'German',
  es: 'Spanish',
  nl: 'Dutch',
  pl: 'Polish',
  fr: 'French',
  ua: 'Ukrainian',
} as const;

export type Locale = keyof typeof SUPPORTED_LOCALES;

export async function setupI18n() {
  const i18n = createI18n({
    legacy: false,
    globalInjection: true,
    fallbackLocale: 'en',
    warnHtmlMessage: false,
  });

  const store = usePiniaStore();

  await setI18nLanguage(i18n.global, store.userLanguage);

  return i18n;
}

export async function setI18nLanguage(i18n: Composer, locale: string) {
  let verifiedLocale: Locale;

  if (locale && locale in SUPPORTED_LOCALES) {
    verifiedLocale = locale as Locale;
  } else {
    console.warn(
      `The locale '${locale}' is not supported. Using 'en' as fallback.`,
    );
    verifiedLocale = 'en';
  }

  const messages = await import(`./../locales/${verifiedLocale}.json`);
  i18n.setLocaleMessage(verifiedLocale, messages.default);

  i18n.locale.value = verifiedLocale;

  const html = document.querySelector('html');
  if (!html) throw Error("Couldn't find html element");
  html.setAttribute('lang', verifiedLocale);

  usePiniaStore().setUserLanguage(verifiedLocale);
}
