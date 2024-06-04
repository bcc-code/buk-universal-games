import { defineStore } from 'pinia';

export const usePiniaStore = defineStore('piniaStore', {
  state: () => ({
    adminFilterGameSelected: null as number | null,
    adminLeagueSelected: null as number | null,
    testTeamCode: null as string | null,
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
  },
  persist: true,
});
