// import './assets/main.css'
import './index.css'
import { createApp } from 'vue'
import App from './App.vue'
import { initRouter } from '../src/router'
import { setupI18n } from './libs/i18n'

// ------------------ pinia section
// import { createPinia } from 'pinia'
// const store = createPinia()
// ------------------ vuex section
import initStore from './old/store'
import createMatchNotifierPlugin from './old/plugins/match-notifier'
import NotificationService from './old/services/notification.service'

const notificationService = new NotificationService()
const matchNotifierPlugin = createMatchNotifierPlugin(notificationService)
const store = initStore(matchNotifierPlugin)
// ------------------

const savedLanguage = localStorage.getItem('userLanguage')

const language = (savedLanguage || navigator.language || navigator.languages[0] || 'en').split(
  '-'
)[0]
const i18n = setupI18n({ locale: language })

createApp(App)
  .use(store)
  .use(initRouter(store))
  .use(i18n)
  .provide('notificationService', notificationService)
  .mount('#app')
