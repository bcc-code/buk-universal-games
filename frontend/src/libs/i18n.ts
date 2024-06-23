import { nextTick } from 'vue';
import { createI18n, type Composer, type VueI18n} from 'vue-i18n';

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

type Locale = typeof SUPPORT_LOCALES[number]

export async function setupI18n(locale:string = 'en') {
  let verifiedLocale:Locale;
  
  if( locale && SUPPORT_LOCALES.includes(locale as Locale)) verifiedLocale = locale as Locale;
  else {
    console.warn(
      `The locale '${locale}' is not supported. Using 'en' as fallback.`,
    );
  
    verifiedLocale = "en"
  } 

  const messages = await import(`./../locales/${verifiedLocale}.json`);
  const fallbackLocale= "en";
  const i18n = createI18n<false>({
    locale:verifiedLocale,
    fallbackLocale,
    allowComposition:true,
    legacy: false,
    messages,
    globalInjection:true
  });

  await nextTick();


  const html  = document.querySelector('html');
  if(!html) throw Error("Couldnt find html element")
  
    html.setAttribute('lang', verifiedLocale);

  return i18n;
}

