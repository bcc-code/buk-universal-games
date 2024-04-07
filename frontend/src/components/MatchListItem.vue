<template>
  <section class="rounded-md py-2 px-2 text-label-1 bg-ice-blue flex w-full" @click="() => { if (clickFunc) clickFunc() }">
    <div class="flex flex-col w-full">
      <span class="text-xs game-title">
        <img class="w-10 h-10" :src="`icon/game-${gameType?.replaceAll('_','')}.svg`" />
        <span class="text">{{ $t(`games.${gameType}`) }} - {{ gameAddOn || '' }}</span>
      </span>
    </div>
    <div class="text-label-1 flex flex-col w-full">
      <span :class="{ 'text-xs': true, 'card-teams': true, winner: team1 === winner, loser: team2 === winner }">
        <span>{{ team1 }}</span>
      </span>
      <span :class="{ 'text-xs': true, 'card-teams': true, winner: team2 === winner, loser: team1 === winner }">
        <span>{{ team2 }}</span>
      </span>
    </div>
    <div class="flex flex-col w-full">
      <span class="text-xs">{{ start }}</span>
    </div>
  </section>
</template>

<script>

export default {
  name: "MatchListItem",
  props: {
    gameType: String,
    gameAddOn: String,
    team1: String,
    team2: String,
    winner: String,
    start: String,
    clickFunc: Function,
  },
  data() {
    return {};
  },
};
</script>

<style scoped>



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
