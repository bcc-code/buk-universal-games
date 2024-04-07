import { createApp, } from 'vue'
import App from './App.vue'
import initRouter from '../src/router'
import initStore from '../src/store'
import { setupI18n } from './libs/i18n'
import NotificationService from './services/notification.service'
import createMatchNotifierPlugin from './plugins/match-notifier'
import './index.css'

const notificationService = new NotificationService();
const matchNotifierPlugin = createMatchNotifierPlugin(notificationService);

const savedLanguage = localStorage.getItem('userLanguage');
const language = (savedLanguage || navigator.language || navigator.userLanguage || 'en').split('-')[0];
const i18n = setupI18n({ locale: language });

const store = initStore(matchNotifierPlugin);

createApp(App)
    .use(store)
    .use(initRouter(store))
    .use(i18n)
    .provide('notificationService', notificationService)
    .mount('#app')
