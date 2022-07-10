<template>
  <UserPageLayout>
    <nav>
      <button @click="$router.push({ name: 'GameInfo' })">&lt;</button>
      <button @click="$router.push({ name: 'GameInfo' })">⠇</button>
    </nav>

    <h3>Games</h3>
    <h2>
      <span class="icon" v-html="icons[gameParsed.name]"></span>
      <span>{{ gameParsed.name }}</span>
    </h2>

    <header class="banner" style="background-image: url('/images/illustrations/activity.svg')">
      <div>
        <p>W: {{ gameParsed.winnerPoints }}pt</p>
      </div>
      <div>
        <p>L: {{ gameParsed.looserPoints }}pt</p>
      </div>
    </header>

    <p class="description">
      {{ gameParsed.description }}
    </p>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
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
  components: { UserPageLayout },
  data() {
    return {
      loginError: "Game Info",
      gameParsed: {},
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
    console.log("game data", this.game);

    if (this.game) {
      this.gameParsed = JSON.parse(this.game);
    } else {
      this.$router.push({ name: "GameInfo" });
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

.description {
  color: #555;
  line-height: 1.5;
}
</style>
