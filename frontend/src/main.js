import { createApp, } from 'vue'
import App from './App.vue'
import router from '../src/router'
import store from '../src/store'
import { setupI18n } from './libs/i18n'
const store = initStore();

createApp(App)
    .use(store)
    .use(initRouter(store))
    .mount('#app')
