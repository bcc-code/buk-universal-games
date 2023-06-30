import { createApp, } from 'vue'
import App from './App.vue'
import initRouter from '../src/router'
import initStore from '../src/store'
import { setupI18n } from './libs/i18n'
import NotificationService from './services/notification.service'
import createMatchNotifierPlugin from './plugins/match-notifier'

const notificationService = new NotificationService();
const matchNotifierPlugin = createMatchNotifierPlugin(notificationService);
const store = initStore(matchNotifierPlugin);

createApp(App)
    .use(store)
    .use(initRouter(store))
    .use(setupI18n({ locale: 'no' }))
    .provide('notificationService', notificationService)
    .mount('#app')
