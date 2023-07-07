import { createStore } from "vuex";
import createPersistedState from "vuex-persistedstate";
import { initData, getData, postData } from "../libs/apiHelper"

const storageKeyPrefix = 'buk-universal-games-';

export default function (...plugins) {
  const store = createStore({
    plugins: [
      createPersistedState({
        key: storageKeyPrefix + 'store'
      })
    ].concat(plugins),
    modules: {
    },
    state: {
      loginData: {},
      loginMessage: '',
      teamStatus: {},
      leagueStatus: {},
      adminLeagues: [],
      adminLeagueStatus: {},
      adminLeagueSelected: 4,
      adminFilterGameSelected: null,
      matches: [],
      adminMatches: [],
      games: [],
      gamesLoading: true,
      coins: ["dgsdfg","as45zzz","gztzz4ez","d5435","g45g4","j767jf"],
      qs: [
        { id: 1, t: "guess", q: "guesshowmany1", a: ["100","1000","10000","20000"]},
        { id: 2, t: "guess", q: "guesshowmany2", a: ["3000","4000","5000","6000"]},
        { id: 3, t: "remember", q: "remember", a: ["opta","optb","optc","optd"]},
        { id: 4, t: "insight", q: "insight", a: ["red","green","yellow","sametime"]},
        { id: 5, t: "guess", q: "guesshowoften", a: ["1","2","4","8"]},
        { id: 6, t: "recognize", q: "recognize1", a: ["horse","camel","antilope","giraffe"]},
        { id: 7, t: "recognize", q: "recognize2", a: ["elephant","tiger","owl","lion"]},
        { id: 8, t: "recognize", q: "recognize3", a: ["peter","john","matthew","paul"]},
        { id: 9, t: "recognize", q: "recognize4", a: ["bkg","gkb","kgb","bgk"]},
        { id: 10, t: "math", q: "math", a: ["30","81","200","67"]},
        { id: 11, t: "knowledge", q: "knowledge", a: ["singapore","serbia","japan","peru"]},
      ],
      qsOpened: {
        1: [],
        2: [],
        3: [],
        4: [],
        5: [],
      },
      answers: [],
      submittedAnswers: [],
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
      setQuestions(state, data) {
        state.questions = data
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
        state.games = data.map(game => {
          return {
            ...game,
            gameType: game.gameType.toLowerCase()
          }
        })
      },
      setGamesLoading(state, data) {
        state.gamesLoading = data
      },
      addAnswer(state, data) {
        data.time = new Date();
        state.answers.push(data)
      },
      setAnswersSubmitted(state, data) {
        state.submittedAnswers.push(...data);
        state.answers = state.answers.filter(item => !data.includes(item))
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
      unlockNewQuestions(state, round) {
        console.log(round);
        for(let i = 0; i < 2; i++)
        {
          const newQuestion = state.qs[Math.floor(Math.random() * state.qs.length)]
          console.log(newQuestion);
          state.qsOpened[round].push(newQuestion)
        }
      }
    },
    actions: {
      async signIn(ctx) {
        ctx.commit("setLoginMessage", 'Logging you in, please wait ...')
        const loginData = await initData(store, "start/")
        ctx.commit("setLoginData", loginData)
        return loginData
      },
      async getTeamStatus(ctx, override) {
        const cacheAge = getCachedDataAge('teamStatus')
        let teamStatus

        if (cacheAge === null || cacheAge > 30 || (cacheAge > 5 && override)) {
          teamStatus = await getData(store, "status/")
          if (!teamStatus.error) {
            cacheData('teamStatus', teamStatus)
          }
        }

        if (!teamStatus) {
          teamStatus = getFromCache('teamStatus', {})
        }

        ctx.commit("setTeamStatus", teamStatus)

        return teamStatus
      },
      async submitAnswers(ctx) {
        const answers = ctx.state.answers.slice();
        const result = await postData(`sidequest/guesses`, answers);
        if (!result.error) {
          ctx.commit("setAnswersSubmitted", answers);
        }
      },
      async confirmTeamResult(ctx, payload) {
        const result = await postData(`matches/${payload.matchId}/results`, payload)
        if (!result.error) {
          ctx.commit("replaceMatch", result)
        }
        return result
      },
      async getLeagueStatus(ctx, override) {
        const cacheAge = getCachedDataAge('leagueStatus')
        let leagueStatus

        if (cacheAge === null || cacheAge > 30 || (cacheAge > 5 && override)) {
          leagueStatus = await getData(store, "status/league")
          if (!leagueStatus.error) {
            cacheData('leagueStatus', leagueStatus)
          }
        }

        if (!leagueStatus) {
          leagueStatus = getFromCache('leagueStatus', {})
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
        const cacheAge = getCachedDataAge('matches')
        let matches

        if (cacheAge === null || cacheAge > 30 || (cacheAge > 5 && override)) {
          matches = await getData(store, "matches")
          if (!matches.error) {
            cacheData('matches', matches)
          }
        }

        if (!matches) {
          matches = getFromCache('matches', [])
        }

        ctx.commit("setMatches", matches)

        return matches
      },
      checkNewQuestions(ctx) {
        const now = new Date();
        const timeString = `${(now.getHours() + 14).toString().padStart(2,'0')}:${(now.getMinutes() + 10).toString().padStart(2,'0')}`;
        const round = ctx.state.matches.findIndex(match => match.start > timeString)
        console.log(round);
        if(round < 1) return;
        const questions = ctx.state.qsOpened[round];
        if (questions.length == 0) {
          ctx.commit("unlockNewQuestions", round)
        }
      },
      async getGames(ctx) {
        const savedDataAge = getCachedDataAge('games')
        let games
        ctx.commit('setGamesLoading', true)

        if (savedDataAge === null || savedDataAge > 30) {
          games = await getData(store, "games")
          if (!games.error) {
            cacheData('games', games)
          }
        }

        if (!games) {
          games = getFromCache('games', [])
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

function getFromCache(storageKey, fallbackValue) {
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

function cacheData(storageKey, data) {
  window.localStorage.setItem(storageKeyPrefix + storageKey, JSON.stringify({
    timestamp: new Date().getTime(),
    data
  }));
}

// Gets age in seconds
function getCachedDataAge(storageKey) {
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
