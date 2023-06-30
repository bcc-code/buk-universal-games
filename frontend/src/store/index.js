import { createStore } from "vuex";
import { initData, getData, postData } from "../libs/apiHelper"

const storageKeyPrefix = 'buk-universal-games-';

export default function (...plugins) {
  const store = createStore({
    plugins,
    modules: {
    },
    state: {
      loginData: {},
      loginMessage: '',
      teamStatus: getSavedData("teamStatus", {}),
      leagueStatus: getSavedData("leagueStatus", {}),
      adminLeagues: [],
      adminLeagueStatus: {},
      adminLeagueSelected: 4,
      adminFilterGameSelected: null,
      matches: getSavedData("matches", []),
      adminMatches: [],
      games: getSavedData("games", []),
      gamesLoading: true,
      qs: [
        { q: "colorshirt", a: ["red","blue","yellow","green"]},
      ],
      answers: [],
      scanning: {
        handlingURL: false,
        stickerCode: null
      },
      currentAmountOfRequestRetries: []
    },
    mutations: {
      setLoginData(state, data) {
        state.loginData = data
      },
      setLoginMessage(state, error) {
        state.loginMessage = error
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
      replaceMatch(state, data) {
        const index = state.adminMatches.findIndex(m => m.matchId === data.matchId)
        if (index !== -1) {
          data.team1 = state.adminMatches[index].team1
          data.team2 = state.adminMatches[index].team2
          data.winner = state.adminMatches[index].winnerId === data.team1Id ? data.team1 : data.team2
          state.adminMatches[index] = data
        }
      },
      setAdminMatches(state, data) {
        state.adminMatches = data
      },
      setGames(state, data) {
        state.games = data
      },
      setGamesLoading(state, data) {
        state.gamesLoading = data
      },
      setAdminFilterGameSelected(state, data) {
        state.adminFilterGameSelected = data
      },
      resetAdminFilterGameSelected(state) {
        state.adminFilterGameSelected = null
      },
      addRequestRetry(state, requestUrl) {
        const exist = state.currentAmountOfRequestRetries[requestUrl]

        if (exist) {
          state.currentAmountOfRequestRetries[requestUrl]++
        } else {
          state.currentAmountOfRequestRetries[requestUrl] = 1
        }
      },
      resetRequestRetry(state, requestUrl) {
        const exist = state.currentAmountOfRequestRetries[requestUrl]

        if (exist) {
          state.currentAmountOfRequestRetries[requestUrl] = null
        }
      },
    },
    actions: {
      async getLoginData(ctx) {
        ctx.commit("setLoginMessage", 'Logging you in, please wait ...')
        const loginData = await initData(store, "start/")
        ctx.commit("setLoginData", loginData)
        return loginData
      },
      async getTeamStatus(ctx, override) {
        const savedDataAge = getSavedDataAge('teamStatus')
        let teamStatus

        if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
          teamStatus = await getData(store, "status/")
          if (!teamStatus.error) {
            saveData('teamStatus', teamStatus)
          }
        }

        if (!teamStatus) {
          teamStatus = getSavedData('teamStatus', {})
        }

        ctx.commit("setTeamStatus", teamStatus)

        return teamStatus

      },
      async confirmTeamResult(ctx, payload) {
        const result = await postData(`matches/${payload.matchId}/results`, payload)
        if (!result.error) {
          ctx.commit("replaceMatch", result)
        }
        return result
      },
      async getLeagueStatus(ctx, override) {
        const savedDataAge = getSavedDataAge('leagueStatus')
        let leagueStatus

        if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
          leagueStatus = await getData(store, "status/league")
          if (!leagueStatus.error) {
            saveData('leagueStatus', leagueStatus)
          }
        }

        if (!leagueStatus) {
          leagueStatus = getSavedData('leagueStatus', {})
        }

        ctx.commit("setLeagueStatus", leagueStatus)

        return leagueStatus
      },
      async getAdminLeagueStatus(ctx) {
        if (!ctx.state.adminLeagueSelected)
          return

        let leagueStatus = await getData(store, "admin/leagues/" + ctx.state.adminLeagueSelected + "/status")


        ctx.commit("setAdminLeagueStatus", leagueStatus)
        return leagueStatus
      },
      async getAdminLeagues(ctx) {
        const leagues = await getData(store, "admin/leagues/")
        ctx.commit("setAdminLeagues", leagues)
        return leagues
      },
      async setAdminLeagueSelected(ctx, id) {
        return ctx.commit("setAdminLeagueSelected", id)
      },
      async getAdminMatches(ctx) {
        const matches = await getData(store, "admin/leagues/" + ctx.state.adminLeagueSelected + "/matches")
        ctx.commit("setAdminMatches", matches)
        return matches
      },
      async getMatches(ctx, override) {
        const savedDataAge = getSavedDataAge('matches')
        let matches

        if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
          matches = await getData(store, "matches")
          if (!matches.error) {
            saveData('matches', matches)
          }
        }

        if (!matches) {
          matches = getSavedData('matches', [])
        }

        ctx.commit("setMatches", matches)

        return matches
      },
      async getGames(ctx) {
        const savedDataAge = getSavedDataAge('games')
        let games
        ctx.commit('setGamesLoading', true)

        if (savedDataAge === null || savedDataAge > 30) {
          games = await getData(store, "games")
          if (!games.error) {
            saveData('games', games)
          }
        }

        if (!games) {
          games = getSavedData('games', [])
        }

        ctx.commit("setGames", games)
        ctx.commit('setGamesLoading', false)

        return games
      },
      async setWinner(ctx, payload) {
        let winner = await postData("admin/games/" + payload.matchId + "/winner/" + payload.teamId)
        return winner
      }
    },
  });
  return store;
}

function getSavedData(storageKey, fallbackValue) {
  const savedData = window.localStorage.getItem(storageKeyPrefix + storageKey);
  let data = fallbackValue

  if (savedData) {
    try {
      data = JSON.parse(savedData).data
    } catch (error) {
      console.error(error)
    }
  }

  return data
}

function saveData(storageKey, data) {
  window.localStorage.setItem(storageKeyPrefix + storageKey, JSON.stringify({
    timestamp: new Date().getTime(),
    data
  }));
}

// Gets age in seconds
function getSavedDataAge(storageKey) {
  let age = null;
  const savedData = window.localStorage.getItem(storageKeyPrefix + storageKey);

  if (savedData) {
    try {
      const savedDataParsed = JSON.parse(savedData)
      const secondsSinceSaved = (new Date().getTime() - savedDataParsed.timestamp) / 1000;
      age = Math.floor(secondsSinceSaved);
    } catch (error) {
      console.error(error)
    }
  }

  // return age * 100;
  return age;
}
