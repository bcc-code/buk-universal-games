import { createRouter, createWebHashHistory } from "vue-router";
import Login from "@/pages/LoginPage.vue";
import LeagueList from "@/pages/LeagueList.vue";
import MatchList from "@/pages/MatchList.vue";
import GameInfo from "@/pages/GameInfo.vue";
import GameInfoDetail from "@/pages/GameInfoDetail.vue";
import ScanResult from "@/pages/ScanResult.vue";
import ScanProcessing from "@/pages/ScanProcessing.vue";
import Map from "@/pages/Map.vue";
import AdminGame from "@/pages/AdminGame.vue";
import AdminLeagueStatus from "@/pages/AdminLeagueStatus.vue";
import AdminSelectLeague from "@/pages/AdminSelectLeague.vue";
import AdminGames from "@/pages/AdminGames.vue";
import AdminMatchListGame from "@/pages/AdminMatchListGame.vue";
import AdminMap from "@/pages/AdminMap.vue";
import store from '@/store'

const routes = [
  {
    path: "/",
    name: "Login",
    component: Login,
    props: true
  },
  {
    path: "/start/:code",
    redirect: { name: 'LeagueList' }
  },
  {
    path: "/admin/AdminLeagueStatus",
    name: "AdminLeagueStatus",
    component: AdminLeagueStatus,
    props: true
  },
  {
    path: "/admin/AdminSelectLeague",
    name: "AdminSelectLeague",
    component: AdminSelectLeague,
    props: true
  },
  {
    path: "/admin/AdminGames",
    name: "AdminGames",
    component: AdminGames,
    props: true
  },
  {
    path: "/admin/AdminMatchListGame",
    name: "AdminMatchListGame",
    component: AdminMatchListGame,
    props: true
  },
  {
    path: "/admin/AdminGame",
    name: "AdminGame",
    component: AdminGame,
    props: true
  },
  {
    path: "/admin/game-info-detail",
    name: "AdminGameInfoDetail",
    component: GameInfoDetail,
    props: true
  },
  {
    path: "/admin/map",
    name: "AdminMap",
    component: AdminMap,
  },
  {
    // TODO: Google Docs and QR code example are contradictive, which one is correct?
    path: "/scan/:stickerCode",
    name: "ScanProcessing1",
    component: ScanProcessing,
  },
  {
    path: "/sticker/:stickerCode",
    name: "ScanProcessing2",
    component: ScanProcessing,
  },
  {
    path: "/:code/sticker-scan-result",
    name: "ScanResult",
    component: ScanResult,
    props: true
  },
  {
    path: "/:code/league-list",
    name: "LeagueList",
    component: LeagueList,
  },
  {
    path: "/:code/match-list",
    name: "MatchList",
    component: MatchList,
  },
  {
    path: "/:code/game-info",
    name: "GameInfo",
    component: GameInfo,
  },
  {
    path: "/:code/game-info-detail",
    name: "GameInfoDetail",
    component: GameInfoDetail,
    props: true
  },
  {
    path: "/:code/map",
    name: "Map",
    component: Map,
  },
];

const router = createRouter({
  history: createWebHashHistory(),
  routes,
});

router.beforeEach(async (to, from, next) => {
  let nextOptions = null;

  if (to.params.stickerCode) {
    store.commit('setScanning', { handlingURL: true, stickerCode: to.params.stickerCode })
  }

  if (to.params.code) {
    window.localStorage.setItem("teamCode", to.params.code);
    const loginData = await store.dispatch("getLoginData")
    store.commit('setLoginMessage', '')


    if (!loginData || loginData.error) {
      if (loginData.error) {
        store.commit('setLoginMessage', loginData.error)
      } else {
        store.commit('setLoginMessage', 'Something went wrong, we could not log you in. Please try again')
      }
      nextOptions = { name: 'Login' }
    }

    if (loginData && loginData.access === 'Admin') {
      nextOptions = { name: 'AdminSelectLeague' }
    }
  }

  next(nextOptions)
})


export default router;
