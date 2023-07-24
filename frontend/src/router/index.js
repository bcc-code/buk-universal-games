import { createRouter, createWebHashHistory } from "vue-router";
import Login from "@/pages/LoginPage.vue";
import LeagueList from "@/pages/LeagueList.vue";
import MatchList from "@/pages/MatchList.vue";
import GameInfo from "@/pages/GameInfo.vue";
import GameInfoDetail from "@/pages/GameInfoDetail.vue";
import SideQuest from "@/pages/SideQuest.vue";
import SideQuestQuestion from "@/pages/SideQuestQuestion.vue";
import Map from "@/pages/Map.vue";
import AdminMatch from "@/pages/AdminMatch.vue";
import AdminLeagueStatus from "@/pages/AdminLeagueStatus.vue";
import AdminSelectLeague from "@/pages/AdminSelectLeague.vue";
import AdminGames from "@/pages/AdminGames.vue";
import AdminMatchListGame from "@/pages/AdminMatchListGame.vue";
import AdminMap from "@/pages/AdminMap.vue";

const routes = [
  {
    path: "/",
    name: "Login",
    component: Login,
  },
  {
    path: "/start/:code",
    name: "LoginWithCode",
    component: Login,
    props: true
  },
  {
    path: "/admin/league-status",
    name: "AdminLeagueStatus",
    component: AdminLeagueStatus,
    props: true
  },
  {
    path: "/admin/leagues",
    name: "AdminSelectLeague",
    component: AdminSelectLeague,
    props: true
  },
  {
    path: "/admin/games",
    name: "AdminGames",
    component: AdminGames,
    props: true
  },
  {
    path: "/admin/matches",
    name: "AdminMatchListGame",
    component: AdminMatchListGame,
    props: true
  },
  {
    path: "/admin/matches/:matchId",
    name: "AdminMatch",
    component: AdminMatch,
    props: true
  },
  {
    path: "/admin/games/:game",
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
    path: "/sidequest",
    name: "SideQuest",
    component: SideQuest
  },
  {
    path: "/sidequest/question/:id",
    name: "SideQuestQuestion",
    component: SideQuestQuestion,
    props: true
  },
  {
    path: "/league-status",
    name: "LeagueList",
    component: LeagueList,
  },
  {
    path: "/matches",
    name: "MatchList",
    component: MatchList,
  },
  {
    path: "/games",
    name: "GameInfo",
    component: GameInfo,
  },
  {
    path: "/games/:game",
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

export default function (store) {
  const router = createRouter({
    history: createWebHashHistory(),
    routes,
  });

  router.beforeEach(async (to, from) => {
    if (!store.state.loginData && window.localStorage.getItem('teamCode')) {
      await store.dispatch('signIn')
      await store.dispatch('getGames')
    }

    if (window.localStorage.getItem('teamCode') && to.path === '/' && !from.matched.length) {
      if(store.state.loginData.access === 'admin') {
        this.$store.dispatch('getAdminLeagues')
        this.$store.dispatch('getAdminLeagueStatus')
        return { name: 'AdminSelectLeague' };
      } else {

        await store.dispatch('getLeagueStatus')
        await store.dispatch('getMatches')
        await store.dispatch('checkNewQuestions')
        return { name: 'LeagueList' };
      }
    }
  })
  return router;
}
