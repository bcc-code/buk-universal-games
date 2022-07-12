import { createRouter, createWebHashHistory } from "vue-router";
import Login from "../pages/LoginPage.vue";
import LeagueList from "../pages/LeagueList.vue";
import MatchList from "../pages/MatchList.vue";
import GameInfo from "../pages/GameInfo.vue";
import GameInfoDetail from "../pages/GameInfoDetail.vue";
import Map from "../pages/Map.vue";
import store from '@/store'

const routes = [
  {
    path: "/",
    name: "Login",
    component: Login,
  },
  {
    path: "/start/:code",
    redirect: { name: 'LeagueList' }
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
  if (to.params.code) {
    window.localStorage.setItem("teamCode", to.params.code);
    const loginData = await store.dispatch("getLoginData")

    if (!loginData) {
      next({ name: 'Login' })
    }
  }

  next()
})


export default router;
