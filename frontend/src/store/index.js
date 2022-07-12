import { createStore } from "vuex";
import { initData, getData } from "../libs/apiHelper"

const store = createStore({
  modules: {
  },
  state: {
    loginData: {},
    teamStatus: {},
    leagueStatus: {},
    matches: [],
    games: []
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
          if(r.status == 200) {
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
          if(r.status == 200) {
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
          if(r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
            return r; 
        })

        ctx.commit("setLeagueStatus", leagueStatus)
        return leagueStatus
    },
    async getMatches(ctx) {
        let matches = await getData("/Games/Matches")
        .then(r => {
          if(r.status == 200) {
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
          if(r.status == 200) {
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
