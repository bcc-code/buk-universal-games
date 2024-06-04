import { createRouter, createWebHashHistory } from 'vue-router';

import Login from '../pages/LoginPage.vue';
import LeagueList from '../pages/LeagueList.vue';
import MatchList from '../pages/MatchList.vue';
import GameInfo from '../pages/GameInfo.vue';
import GameInfoDetail from '../pages/GameInfoDetail.vue';
import Map from '../pages/MapPage.vue';
import AdminMatch from '../pages/AdminMatch.vue';
import AdminLeagueStatus from '../pages/AdminLeagueStatus.vue';
import AdminSelectLeague from '../pages/AdminSelectLeague.vue';
import AdminSelectGame from '../pages/AdminSelectGame.vue';
import AdminMatchListGame from '../pages/AdminMatchListGame.vue';
import AdminMap from '../pages/AdminMap.vue';

const routes = [
  {
    path: '/',
    name: 'Login',
    component: Login,
  },
  {
    path: '/start/:code',
    name: 'LoginWithCode',
    component: Login,
    props: true,
  },
  {
    path: '/admin/league-status',
    name: 'AdminLeagueStatus',
    component: AdminLeagueStatus,
    props: true,
  },
  {
    path: '/admin/leagues',
    name: 'AdminSelectLeague',
    component: AdminSelectLeague,
    props: true,
  },
  {
    path: '/admin/games',
    name: 'AdminSelectGame',
    component: AdminSelectGame,
    props: true,
  },
  {
    path: '/admin/matches',
    name: 'AdminMatchListGame',
    component: AdminMatchListGame,
    props: true,
  },
  {
    path: '/admin/matches/:matchId',
    name: 'AdminMatch',
    component: AdminMatch,
    props: true,
  },
  {
    path: '/admin/map',
    name: 'AdminMap',
    component: AdminMap,
  },
  {
    path: '/league-status',
    name: 'LeagueList',
    component: LeagueList,
  },
  {
    path: '/matches',
    name: 'MatchList',
    component: MatchList,
  },
  {
    path: '/games',
    name: 'GameInfo',
    component: GameInfo,
  },
  {
    path: '/games/:game',
    name: 'GameInfoDetail',
    component: GameInfoDetail,
    props: true,
  },
  {
    path: '/map',
    name: 'Map',
    component: Map,
  },
];

export function initRouter(store: any) {
  const router = createRouter({
    history: createWebHashHistory(),
    routes,
  });

  router.beforeEach(async (to, from) => {
    if (!store.state.loginData && window.localStorage.getItem('testTeamCode')) {
      await store.dispatch('signIn');
      await store.dispatch('getGames');
    }

    if (
      window.localStorage.getItem('testTeamCode') &&
      to.path === '/' &&
      !from.matched.length
    ) {
      if (store.state.loginData.access === 'admin') {
        await store.dispatch('getAdminLeagues');
        await store.dispatch('getAdminLeagueStatus');
        return { name: 'AdminSelectLeague' };
      } else {
        await store.dispatch('getLeagueStatus');
        await store.dispatch('getMatches');
        await store.dispatch('checkNewQuestions');
        return { name: 'LeagueList' };
      }
    }

    if (
      !window.localStorage.getItem('testTeamCode') &&
      to.path !== '/' &&
      !to.path.startsWith('/start')
    ) {
      return { name: 'Login' };
    }
  });
  return router;
}
