import { createRouter, createWebHashHistory } from 'vue-router'

import Login from '../views/LoginPage.vue'
import LeagueList from '../views/LeagueList.vue'
import MatchList from '../views/MatchList.vue'
import GameInfo from '../views/GameInfo.vue'
import GameInfoDetail from '../views/GameInfoDetail.vue'
import SideQuest from '../views/SideQuest.vue'
import SideQuestQuestion from '../views/SideQuestQuestion.vue'
import Map from '../views/Map.vue'
import AdminMatch from '../views/AdminMatch.vue'
import AdminLeagueStatus from '../views/AdminLeagueStatus.vue'
import AdminSelectLeague from '../views/AdminSelectLeague.vue'
import AdminGames from '../views/AdminGames.vue'
import AdminMatchListGame from '../views/AdminMatchListGame.vue'
import AdminGameInfoDetail from '../views/AdminGameInfoDetail.vue'
import AdminMap from '../views/AdminMap.vue'

const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login
  },
  {
    path: '/start/:code',
    name: 'LoginWithCode',
    component: Login,
    props: true
  },
  {
    path: '/admin/league-status',
    name: 'AdminLeagueStatus',
    component: AdminLeagueStatus,
    props: true
  },
  {
    path: '/admin/leagues',
    name: 'AdminSelectLeague',
    component: AdminSelectLeague,
    props: true
  },
  {
    path: '/admin/games',
    name: 'AdminGames',
    component: AdminGames,
    props: true
  },
  {
    path: '/admin/matches',
    name: 'AdminMatchListGame',
    component: AdminMatchListGame,
    props: true
  },
  {
    path: '/admin/matches/:matchId',
    name: 'AdminMatch',
    component: AdminMatch,
    props: true
  },
  {
    path: '/admin/games/:game',
    name: 'AdminGameInfoDetail',
    component: AdminGameInfoDetail,
    props: true
  },
  {
    path: '/admin/map',
    name: 'AdminMap',
    component: AdminMap
  },
  {
    path: '/sidequest',
    name: 'SideQuest',
    component: SideQuest
  },
  {
    path: '/sidequest/question/:id',
    name: 'SideQuestQuestion',
    component: SideQuestQuestion,
    props: true
  },
  {
    path: '/league-status',
    name: 'LeagueList',
    component: LeagueList
  },
  {
    path: '/matches',
    name: 'MatchList',
    component: MatchList
  },
  {
    path: '/games',
    name: 'GameInfo',
    component: GameInfo
  },
  {
    path: '/games/:game',
    name: 'GameInfoDetail',
    component: GameInfoDetail,
    props: true
  },
  {
    path: '/map',
    name: 'Map',
    component: Map
  }
]

export default function (store) {
  const router = createRouter({
    history: createWebHashHistory(),
    routes
  })

  router.beforeEach(async (to, from) => {
    if (!store.state.loginData && window.localStorage.getItem('testTeamCode')) {
      await store.dispatch('signIn')
      await store.dispatch('getGames')
    }

    if (window.localStorage.getItem('testTeamCode') && to.path === '/' && !from.matched.length) {
      if (store.state.loginData.access === 'admin') {
        await store.dispatch('getAdminLeagues')
        await store.dispatch('getAdminLeagueStatus')
        return { name: 'AdminSelectLeague' }
      } else {
        await store.dispatch('getLeagueStatus')
        await store.dispatch('getMatches')
        await store.dispatch('checkNewQuestions')
        return { name: 'LeagueList' }
      }
    }

    if (
      !window.localStorage.getItem('testTeamCode') &&
      to.path !== '/' &&
      !to.path.startsWith('/start')
    ) {
      return { name: 'Login' }
    }
  })
  return router
}
