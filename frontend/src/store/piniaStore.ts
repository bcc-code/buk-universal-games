import type { Locale } from '@/libs/i18n';
import { defineStore } from 'pinia';

export const usePiniaStore = defineStore('piniaStore', {
  state: () => ({
    adminFilterGameSelected: null as number | null,
    adminLeagueSelected: null as number | null,
    testTeamCode: null as string | null,
    userLanguage: navigator.language.split('-')[0] || 'en' as string,
  }),
  actions: {
    setAdminFilterGameSelected(id: number) {
      this.adminFilterGameSelected = id;
    },
    setAdminLeagueSelected(id: number) {
      this.adminLeagueSelected = id;
    },
    setTestTeamCode(code: string) {
      this.testTeamCode = code;
    },
    setUserLanguage(locale: Locale) {
      this.userLanguage = locale;
    },
  },
  persist: true,
});
