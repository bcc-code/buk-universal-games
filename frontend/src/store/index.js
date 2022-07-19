import { createStore } from "vuex";
import { initData, getData } from "../libs/apiHelper"

const store = createStore({
  modules: {
  },
  state: {
    loginData: {},
    teamStatus: {},
    leagueStatus: {},
    adminLeagues: [],
    adminLeagueStatus: {},
    adminLeagueSelected: 4,
    matches: [],
    games: [],
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
    setAdminLeagues(state, data) {
      state.adminLeagues = data
    },
    setAdminLeagueStatus(state, data) {
      state.adminLeagueStatus = data
    },
    setAdminLeagueSelected(state, data) {
      state.adminLeagueSelected = data
    },
    setScanning(state, data) {
      state.scanning = data
    },
    setMatches(state, data) {
      state.matches = data
    },
    setGames(state, data) {
      state.games = data
    }
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
    },
    async getAdminLeagueStatus(ctx) {
      if(!ctx.state.adminLeagueSelected)
        return

      let leagueStatus = await getData("/Admin/Leagues/" + ctx.state.adminLeagueSelected + "/Status")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setAdminLeagueStatus", leagueStatus)
      return leagueStatus
    },
    async getAdminLeagues(ctx) {
      let leagues = await getData("/Admin/Leagues/")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setAdminLeagues", leagues)
      return leagues
    },
    async setAdminLeagueSelected(ctx, id) {
      return ctx.commit("setAdminLeagueSelected", id)
    },
    async getMatches(ctx) {
      let matches = await getData("/Games/Matches")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setMatches", matches)
      return matches
    },
    async getGames(ctx) {
      let games = await getData("/Games")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setGames", games)
      return games
    }
  },
});

export default store;
