import './index.css';
import { createApp } from 'vue';
import App from './App.vue';
import { initRouter } from './router';
import { setupI18n } from './libs/i18n';

// ------------------ pinia section
import { createPinia } from 'pinia';
import piniaPluginPersistedstate from 'pinia-plugin-persistedstate';

const piniaStore = createPinia();
piniaStore.use(piniaPluginPersistedstate);

import { VueQueryPlugin } from '@tanstack/vue-query';
import NotificationService from './services/notification.service';

// ------------------ vuex section
import initStore from './store';
import createMatchNotifierPlugin from './plugins/match-notifier';
import { usePiniaStore } from './store/piniaStore';

const notificationService = new NotificationService();
const matchNotifierPlugin = createMatchNotifierPlugin(notificationService);
const vuexStore = initStore(matchNotifierPlugin);
// ----------------

const app = createApp(App)
  .use(piniaStore)
  .use(vuexStore)
  .use(initRouter(vuexStore))
  .use(VueQueryPlugin)
  .provide('notificationService', notificationService);
  

  const i18n = await setupI18n();
  
  app.use(i18n)
  
  app.mount('#app');
