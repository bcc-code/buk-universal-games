<template>
  <section class="card card-light" @click="() => {if(clickFunc) clickFunc()}">
    <div class="card-light-column">
      <span class="card-light-text game-title">
        <span class="icon" v-html="icons[game]"></span>
        <span class="text">{{ game + ' ' + (gameAddOn || '')}}</span>
      </span>
    </div>
    <div class="card-light-column">
      <span :class="{ 'card-light-text': true, 'card-teams': true, winner: team1 === winner, loser: team2 === winner }">
        <span>{{ team1 }}</span>
      </span>
      <span :class="{ 'card-light-text': true, 'card-teams': true, winner: team2 === winner, loser: team1 === winner }">
        <span>{{ team2 }}</span>
      </span>
    </div>
    <div class="card-light-column">
      <h2 class="card-light-text">{{ start }}</h2>
    </div>
  </section>
</template>

<script>
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "MatchListItem",
  props: {
    game: String,
    gameAddOn: String,
    team1: String,
    team2: String,
    winner: String,
    start: String,
    clickFunc: Function,
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
button {
  background-color: inherit;
}

.card {
  padding: 0.25em 0.5em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 35% 50% 15%;
  align-items: center;
}

.card-light {
  background-color: white;
  color: var(--dark);
}

.card-light-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.card-dark {
  background-color: var(--dark);
  color: white;
}

.card-light-text {
  font-size: 0.85em;
  color: inherit;
  margin: 0.35em;
}

.card-btn {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 0;
  padding: 0.15em 0.15em 0.15em 0.65em;
}

.card-teams {
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
  position: relative;
  padding-left: 1.5em;
}

.card-teams::after {
  content: " ";
  position: absolute;
  top: 50%;
  left: 0;
  transform: translateY(-50%);
  background-color: black;
  width: 1em;
  height: 1em;
  border-radius: 100%;
}
.card-teams.winner::after {
  background-color: var(--green);
}

.card-teams.loser::after {
  background-color: var(--red);
}

.game-title {
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
}
</style>
