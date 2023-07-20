import { createStore } from "vuex";
import VuexPersistence from "vuex-persist";
import roundfinishedMonitor from "@/plugins/roundfinished-monitor";
import { initData, getData, postData } from "../libs/apiHelper"

const storageKeyPrefix = 'buk-universal-games-';

const vuexLocal = new VuexPersistence({
  storage: window.localStorage,
  key: `ubg-store`,
})

export default function (...plugins) {
  const store = createStore({
    plugins: [
      vuexLocal.plugin,
      roundfinishedMonitor(),
    ].concat(plugins),
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
        { id: 1, t: "guess", q: "guesshowmany1", a: ["115000","90000","50000","140000"]},
        { id: 2, t: "guess", q: "guesshowmany2", a: ["11200","7400","9900","13500"]},
        { id: 3, t: "remember", q: "remember", a: ["opta","optb","optc","optd"]},
        { id: 4, t: "insight", i:true, q: "insight", a: ["red","green","yellow","sametime"]},
        { id: 5, t: "recognize", i:true, q: "recognize1", a: ["Gershon Twilley","Harald Kronstad","Gunnar Gangsø","Bernt-Aksel Larsen"]},
        { id: 6, t: "recognize", i:true, q: "recognize2", a: ["peter","john","matthew","paul"]},
        { id: 7, t: "math", i:true, q: "math", a: ["30","81","200","67"]},
        { id: 8, t: "knowledge", i:true, q: "knowledge", a: ["singapore","serbia","japan","peru"]},
      ],
      qsOpened: [
        [],
        [],
        [],
        [],
        [],
      ],
      answers: [],
      submittedAnswers: [],
      scanning: {
        handlingURL: false,
        stickerCode: null
      },
      userLanguage: null,
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
        state.games = data
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
        if(state.qsOpened[round -1] && state.qsOpened[round -1].length > 0)
        {
          console.log("Already unlocked");
          return;
        }
        while(state.qsOpened[round -1].length < 2)
        {
          const newQuestion = state.qs[Math.floor(Math.random() * state.qs.length)]
          if(!state.qsOpened.flat().includes(newQuestion) && !state.qsOpened[round -1].some(q => q.t == newQuestion.t))
          {
            console.log(newQuestion);
            state.qsOpened[round -1].push(newQuestion);
          }
        }
      },
      setUserLanguage(state, language) {
        state.userLanguage = language;
      }
    },
    getters: {
      currentRound: (state) => {
        const now = new Date();
        const timeString = `${(now.getHours()).toString().padStart(2,'0')}:${(now.getMinutes()).toString().padStart(2,'0')}`;
        const currentMatchIndex = state.matches?.findLastIndex(match => match.start <= timeString);
        
        if(!currentMatchIndex || currentMatchIndex < 0) return 0;
        
        console.log(`Current round:${currentMatchIndex + 1}, based on time ${timeString}`);
        return currentMatchIndex + 1;
      },
      afterRoundPeriod: (state, getters) => {
        const round = getters.currentRound;
        if(round < 1) return { isAfterRound: false, round: round };

        const now = new Date();
        const currentRoundStart = new Date(`${now.toDateString()} ${state.matches?.[round - 1].start}`);
        const afterRoundStart = new Date(currentRoundStart.getTime() + (15 * 60_000));
        const afterRoundEnd = new Date(afterRoundStart.getTime() + (10 * 60_000));

        if (now >= afterRoundStart && now <= afterRoundEnd) {
          return { isAfterRound: true, round: round };
        }
        else {
          return { isAfterRound: false, round: round };
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

        if (!matches || matches.error) {
          matches = getFromCache('matches', [])
        }

        ctx.commit("setMatches", matches)

        return matches
      },
      checkNewQuestions(ctx, matches) {
        if(!matches || matches.length == 0)
          return;

        const { isAfterRound, round: currentRound } = ctx.getters.afterRoundPeriod;

        if(currentRound < 1) return;

        for(let i=1; i < 5 && (i<currentRound || (i == currentRound && isAfterRound)); i++)
        {
          const questions = ctx.state.qsOpened[i - 1] || [];
          console.log(currentRound, isAfterRound, i, questions, ctx.state.qsOpened[i - 1]);
          if (questions.length == 0) {
            ctx.commit("unlockNewQuestions", i)
          }
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
    }
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
