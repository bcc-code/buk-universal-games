<template>
  <UserPageLayout>
    <header class="banner">Banner</header>

    <h2>Games</h2>

    <ul v-if="loading" class="games games-loading">
      <li class="game">
        <span class="game-icon" v-html="icons.Metal"></span>
        <h3 class="game-title">Loading ...</h3>
      </li>
      <li class="game">
        <span class="game-icon" v-html="icons.Metal"></span>
        <h3 class="game-title">Loading ...</h3>
      </li>
      <li class="game">
        <span class="game-icon" v-html="icons.Metal"></span>
        <h3 class="game-title">Loading ...</h3>
      </li>
      <li class="game">
        <span class="game-icon" v-html="icons.Metal"></span>
        <h3 class="game-title">Loading ...</h3>
      </li>
      <li class="game">
        <span class="game-icon" v-html="icons.Metal"></span>
        <h3 class="game-title">Loading ...</h3>
      </li>
    </ul>
    <ul v-else class="games">
      <li class="game" v-for="game in games" :key="game.id">
        <span class="game-icon" v-html="icons[game.name]"></span>
        <h3 class="game-title">{{ game.name }}</h3>
      </li>
    </ul>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import { getData } from "@/libs/apiHelper";
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout },
  data() {
    return {
      loginError: "Game Info",
      games: [],
      loading: true,
      icons: {
        Earth: gameEarthIcon,
        Fire: gameFireIcon,
        Metal: gameMetalIcon,
        Wood: gameWoodIcon,
        Water: gameWaterIcon,
      },
    };
  },
  mounted() {
    getData("/Games")
      .then((r) => r.json())
      .then((r) => {
        this.games = r;
        this.loading = false;
      })
      .catch((e) => {
        this.loading = false;
        console.error(e);
      });
  },
  methods: {},
};
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  border-radius: 1em;
  margin: 2em auto;
  background-color: var(--dark);
  color: var(--green);
}

.games {
  display: grid;
  grid-template-columns: 1fr 1fr;
  list-style: none;
  padding: 0;
  margin: 0;
  gap: 1.5em;
}

.games .game {
  color: #fff;
  background-color: var(--dark);
  border-radius: 1em;
  padding: 1.5em;
  display: flex;
  flex-direction: column;
  align-items: start;
  justify-content: center;
}

.games-loading .game {
  background-color: var(--gray-2);
}

.games .game .game-title {
  margin: 1rem 0 0;
  font-weight: 400;
}

.games .game .game-icon {
  margin: 0;
  width: 2em;
  height: 2em;
  overflow: hidden;
}
</style>
