<template>
  <section v-if="selectedMatch?.team1" class="card bg-vanilla p-1">
    <section>
      <div class="card-dark-column">
        <h1 class="gameinfo card-dark-text" @click="showGameInfo">
          <img class="icon" :src="`icon/game-${game?.gameType.replaceAll('_','')}.svg`"/>
          <span>{{ $t(`games.${game?.gameType}`) }}</span><span class="title-arrow">></span>
        </h1>
      </div>
    </section>
    <section class="flex justify-center align-center w-full">
      <div class="flex flex-col justify-center align-center w-full">
        <h2 class="card-dark-text">{{ selectedMatch?.team1 }}</h2>
        <div v-if="selectedMatch.winner">
          <h2 :class="{ 'card-dark-text' : true, 'winner' : selectedMatch.winnerId == selectedMatch.team1Id }">{{ selectedMatch.team1Result ? $t("score." + game?.gameType, { score:selectedMatch.team1Result}) : '-' }}</h2>
        </div>
      </div>
      <div class="vl"></div>
      <div class="flex flex-col justify-center align-center w-full">
        <h2 class="card-dark-text">{{ selectedMatch?.team2 }}</h2>
        <div v-if="selectedMatch.winner">
          <h2 :class="{ 'card-dark-text' : true, 'winner' : selectedMatch.winnerId == selectedMatch.team2Id }">{{ selectedMatch.team2Result ? $t("score." + game?.gameType, { score:selectedMatch.team2Result}) : "-" }}</h2>
        </div>
      </div>
    </section>
  </section>
</template>

<script>
export default {
  name: "MatchCard",
  props: {
    selectedMatch: Object,
    game: Object,
  },
  methods: {
    showGameInfo() {
      this.$router.push({
            name: 'GameInfoDetail',
            params: {
              game: this.game?.id,
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

.gameinfo {
  cursor:pointer;
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
  background-color: var(--label-1);
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
