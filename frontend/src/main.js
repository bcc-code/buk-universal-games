import { createApp, } from 'vue'
import App from './App.vue'
import router from '../src/router'
import store from '../src/store'
import { setupI18n } from './libs/i18n'
import NotificationService from '@/services/notification.service'

createApp(App)
    .use(store)
    .use(initRouter(store))
    .mount('#app')
