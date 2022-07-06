import { createRouter, createWebHistory } from "vue-router";
import Login from "../pages/LoginPage.vue";
import LeagueList from "../pages/LeagueList.vue";

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
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});


export default router;
