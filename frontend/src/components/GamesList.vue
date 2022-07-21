<template>
  <ul v-if="loading" class="games games-loading">
    <li v-for="i in [0, 1, 2, 3, 4]" :key="i" class="game">
      <span class="game-icon"></span>
      <h3 class="game-title">Loading</h3>
    </li>
  </ul>
  <div v-else-if="games.length === 0">
    <p>Something went wrong, please try again later ...</p>
  </div>
  <ul v-else class="games">
    <li class="game" v-for="game in games" :key="game.id" @click="$emit('clicked', game)">
      <span class="game-icon" v-html="icons[game.name]"></span>
      <h3 class="game-title">{{ game.name }}</h3>
    </li>
  </ul>
</template>

<script>
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "LeageListItem",
  props: {
    games: {},
    loading: Boolean,
  },
  data() {
    return {
      icons: {
        Earth: gameEarthIcon,
        Fire: gameFireIcon,
        Metal: gameMetalIcon,
        Wood: gameWoodIcon,
        Water: gameWaterIcon,
      },
    };
  },
};
</script>

<style scoped>
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

.games-loading .game {
  background-color: var(--dark);
  background-repeat: no-repeat;
  background-size: 10em 100%;
  background-image: linear-gradient(to right, var(--dark) 0%, hsl(323, 50%, 33%) 50%, var(--dark) 100%);
  animation-duration: 750ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

@keyframes shimmer {
  0% {
    background-position: -12em 0;
  }

  100% {
    background-position: 12em 0;
  }
}

.games-loading .game .game-title,
.games-loading .game .game-icon {
  color: hsl(323, 50%, 33%);
  background-color: hsl(323, 50%, 33%);
}
</style>
