<template>
  <ul v-if="loading" class="games games-loading">
    <li v-for="i in [0, 1, 2, 3, 4]" :key="i" class="game">
      <span class="game-icon"></span>
      <h3 class="game-title">....</h3>
    </li>
  </ul>
  <div v-else-if="games.length === 0">
    <h2>{{ $t("general_error") }}</h2>
    <p>{{ $t("please_refresh") }}</p>
  </div>
  <div v-else class="space-y-5 ">
    <div  v-for="game in games"
      :key="game.gameId" @click="$emit('clicked', game)">
      <
      <GamesListItem :game-id="game.id" :game-type="game.gameType" :game-start="game.start" />

    </div>
  </div>
</template>

<script setup lang="ts">
import GamesListItem from './GamesListItem.vue'
defineProps<{ games: Array<{ 
  gameType: string, 
  gameId: string | number
  id: string | number
  start: string }>, 
  loading: boolean }>()
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
  align-items: flex-start;
  justify-content: center;
}

.games .game .game-title {
  margin: .5em 0 0 0;
  font-weight: 400;
}

.games .game .game-icon {
  margin: 0;
  width: 3em;
  height: 3em;
  float: right;
}

.games-loading .game {
  background-color: var(--dark);
  background-repeat: no-repeat;
  background-size: 10em 100%;
  background-image: linear-gradient(to right, var(--dark) 0%, var(--dark-blue) 50%, var(--dark) 100%);
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
  color: var(--dark-blue);
  background-color: var(--dark-blue);
}
</style>
