import { createRouter, createWebHistory } from "vue-router";
import Login from "../pages/LoginPage.vue";
import LeagueList from "../pages/LeagueList.vue";
import MatchList from "../pages/MatchList.vue";
import GameInfo from "../pages/GameInfo.vue";
import GameInfoDetail from "../pages/GameInfoDetail.vue";
import Map from "../pages/Map.vue";

const routes = [
  {
    path: "/",
    name: "Login",
    component: Login,
  },
  {
    path: "/league-list",
    name: "LeagueList",
    component: LeagueList,
  },
  {
    path: "/match-list",
    name: "MatchList",
    component: MatchList,
  },
  {
    path: "/game-info",
    name: "GameInfo",
    component: GameInfo,
  },
  {
    path: "/game-info-detail",
    name: "GameInfoDetail",
    component: GameInfoDetail,
    props: true
  },
  {
    path: "/map",
    name: "Map",
    component: Map,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});


export default router;
