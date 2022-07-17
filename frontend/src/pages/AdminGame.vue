<template>
  <AdminPageLayout>
    <nav>
      <button
        @click="
          $router.push({
            name: 'AdminGames',
          })
        "
      >
        &lt;
      </button>
      <button
        @click="
          $router.push({
            name: 'GameInfoDetail',
            params: {
              game: JSON.stringify(gameParsed),
              code: $store.state.loginData.code,
            },
          })
        "
      >
        Game-info
      </button>
    </nav>

    <h3>Games</h3>
    <h2>
      <span class="icon" v-html="icons[gameParsed.name]"></span>
      <span>{{ gameParsed.name }}</span>
    </h2>

    <ul>
      <li
        v-for="team in gameParsed.teams"
        :key="team.name"
        :class="{ selected: team.name === selectedTeam }"
        v-on:click="selectedTeam = team.name"
      >
        {{ team.name }}
      </li>
    </ul>

    <div class="buttons">
      <button class="btn btn-success">Winner</button>
      <button class="btn btn-blank">Loser</button>
    </div>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
// import { getData } from "@/libs/apiHelper";
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "LoginPage",
  props: {
    game: String,
  },
  components: { AdminPageLayout },
  data() {
    return {
      loginError: "Game Info",
      gameParsed: {},
      loading: true,
      selectedTeam: null,
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
    const tmpGame = {
      name: "Water",
      teams: [{ name: "Sandefjord" }, { name: "Ottowa" }],
    };

    // if (this.game) {
    if (tmpGame) {
      // this.gameParsed = JSON.parse(this.game);
      this.gameParsed = tmpGame;
    } else {
      this.$router.push({ name: "Login" });
    }
  },
  methods: {},
};
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  height: 20em;
  border-radius: 1em;
  margin: 2em auto;
  background-color: var(--dark);
  color: #fff;
  display: flex;
  justify-content: space-between;
  align-items: end;
  background-size: 80%;
  background-position: center;
  background-repeat: no-repeat;
}

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

ul {
  display: grid;
  gap: 1em;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 10em;
  list-style: none;
  margin: 0 0 2em 0;
  padding: 0;
}

ul li {
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: var(--gray-2);
  border-radius: 1em;
}

ul li.selected {
  color: #fff;
  background-color: var(--dark);
}

.buttons {
  display: flex;
  flex-direction: column;
  gap: 1em;
}

.buttons button {
  width: 100%;
}
</style>
