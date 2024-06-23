import { nextTick } from 'vue';
import { createI18n, type Composer } from 'vue-i18n';

export const SUPPORT_LOCALES = [
  'cn',
  'de',
  'en',
  'es',
  'fi',
  'fr',
  'hu',
  'it',
  'nb',
  'nl',
  'pl',
  'ro',
  'ru',
  'tr',
  'ua',
] as const;

type Locale = typeof SUPPORT_LOCALES[number];

export async function setupI18n(locale: string = 'en') {
  const fallbackLocale = 'en';
  const i18n = createI18n({
    legacy: false,
    globalInjection: true,
    locale,
    fallbackLocale,
  });

  await setI18nLanguage(i18n.global, locale);

  return i18n;
}

export async function setI18nLanguage(i18n: Composer, locale: string) {
  let verifiedLocale: Locale;

  if (locale && SUPPORT_LOCALES.includes(locale as Locale)) {
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
}
