<template>
  <section v-if="selectedMatch.team1" class="card card-dark">
    <section>
      <div class="card-dark-column">
        <h1 class="card-dark-text" @click="showGameInfo">
          <img class="icon" :src="require(`@/assets/icons/game-${game.gameType}.svg`)"/>
          <span>{{ game.name }}</span><span class="title-arrow">></span>
          
        </h1>
      </div>
    </section>
    <section class="card-row">
      <div class="card-dark-column">
        <h2 class="card-dark-text">{{ selectedMatch?.team1 }}</h2>
        <div v-if="selectedMatch.winner">
          <h2 :class="{ 'card-dark-text' : true, 'winner' : selectedMatch.winnerId == selectedMatch.team1Id }">{{ selectedMatch.team1Result ? $t("score." + game.gameType, { score:selectedMatch.team1Result}) : '-' }}</h2>
        </div>
      </div>
      <div class="vl"></div>
      <div class="card-dark-column">
        <h2 class="card-dark-text">{{ selectedMatch?.team2 }}</h2>
        <div v-if="selectedMatch.winner">
          <h2 :class="{ 'card-dark-text' : true, 'winner' : selectedMatch.winnerId == selectedMatch.team2Id }">{{ selectedMatch.team2Result ? $t("score." + game.gameType, { score:selectedMatch.team2Result}) : "-" }}</h2>
        </div>
      </div>
    </section>
  </section>
</template>

<script>
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";


export default {
  name: "MatchCard",
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
  props: {
    selectedMatch: Object,
    game: Object,
  },
  methods: {
    showGameInfo() {
      this.$router.push({
            name: 'GameInfoDetail',
            params: {
              game: this.game.id,
            },
          })
    },
  },
};
</script>

<style scoped>
h1 {
  font-weight: 500;
  display: flex;
  align-items: center;
  gap: 0.5em;
}

h2 {
  font-weight: normal;
  margin: 0;
}

button {
  background-color: inherit;
}

.icon {
  width: 3em;
  height: 3em;
  margin-right: 0.5em;
}
.title-arrow {
  font-weight: normal;
  font-family: sans-serif;
}
.card {
  padding: 1em;
  border-radius: 0.75em;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.card-row {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.card-dark {
  background-color: var(--dark);
  color: white;
}

.card-dark-column {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.card-dark-text {
  font-size: 1.25em;
  color: inherit;
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}

.vl {
  border-left: 2px solid var(--green);
  height: 80px;
  margin: 0.5em 0;
}

.winner {
  color: var(--green);
}
</style>
