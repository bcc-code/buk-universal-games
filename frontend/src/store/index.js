import { createStore } from "vuex";
import { initData, getData, postData } from "../libs/apiHelper"

const storageKeyPrefix = 'buk-universal-games-';

const store = createStore({
  modules: {
  },
  state: {
    loginData: {},
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
    async getTeamStatus(ctx, override) {
      const savedDataAge = getSavedDataAge('teamStatus')
      let teamStatus

      if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
        teamStatus = await getData("/Status/")
          .then(r => {
            if (r.status == 200) {
              return r.json()
            }
          })
          .then(r => {
            if (r.error) {
              throw r.error;
            }

            saveData('teamStatus', r)

            return r;
          }).catch(e => {
            console.error(e)
          })
      }

      if (!teamStatus) {
        teamStatus = getSavedData('teamStatus', {})
      }

      ctx.commit("setTeamStatus", teamStatus)

      return teamStatus

    },
    async getLeagueStatus(ctx, override) {
      const savedDataAge = getSavedDataAge('leagueStatus')
      let leagueStatus

      if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
        leagueStatus = await getData("/Status/League")
          .then(r => {
            if (r.status == 200) {
              return r.json()
            }
          })
          .then(r => {
            if (r.error) {
              throw r.error;
            }

            saveData('leagueStatus', r)

            return r;
          }).catch(e => {
            console.error(e)
          })
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
    async getAdminMatches(ctx) {
      let matches = await getData("/Admin/Leagues/" + ctx.state.adminLeagueSelected + "/Matches")
        .then(r => {
          if (r.status == 200) {
            return r.json()
          }
        })
        .then(r => {
          return r;
        })

      ctx.commit("setAdminMatches", matches)
      return matches
    },
    async getMatches(ctx, override) {
      const savedDataAge = getSavedDataAge('matches')
      let matches

      if (savedDataAge === null || savedDataAge > 30 || (savedDataAge > 5 && override)) {
        matches = await getData("/Games/Matches")
          .then(r => {
            if (r.status == 200) {
              return r.json()
            }
          })
          .then(r => {
            if (r.error) {
              throw r.error;
            }

            saveData('matches', r)

            return r;
          }).catch(e => {
            console.error(e)
          })
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
        games = await getData("/Games")
          .then(r => {
            if (r.status == 200) {
              return r.json()
            }
          })
          .then(r => {
            if (r.error) {
              throw r.error;
            }

            saveData('games', r)

            return r;
          }).catch(e => {
            console.error(e)
          })
      }

      if (!games) {
        games = getSavedData('games', [])
      }

      ctx.commit("setGames", games)
      ctx.commit('setGamesLoading', false)

      return games
    },
    async setWinner(ctx, payload) {
      let winner = await postData("/Admin/Games/" + payload.matchId + "/Winner/" + payload.teamId)
      return winner
    }
  },
});

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

  return age;
}

export default store;
