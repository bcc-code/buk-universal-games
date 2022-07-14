import { createStore } from "vuex";
import { initData, getData } from "../libs/apiHelper"

const store = createStore({
  modules: {
  },
  state: {
    loginData: {},
    teamStatus: {},
    leagueStatus: {},
    scanning: {
      handlingURL: false,
      stickerCode: null
    }
  },
  mutations: {
    setLoginData(state, data) {
      state.loginData = data
    },
    setTeamStatus(state, data) {
      state.teamStatus = data
    },
    setLeagueStatus(state, data) {
      state.leagueStatus = data
    },
    setScanning(state, data) {
      state.scanning = data
    },
  },
  actions: {
    async getLoginData(ctx) {
      let loginData = await initData("Start/")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setLoginData", loginData)
      return loginData
    },
    async getTeamStatus(ctx) {
      let teamStatus = await getData("/Status/")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setTeamStatus", teamStatus)
      return teamStatus
    },
    async getLeagueStatus(ctx) {
      let leagueStatus = await getData("/Status/League")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setLeagueStatus", leagueStatus)
      return leagueStatus
    }
  },
});

export default store;
