<template>
  <section
    class="rounded-md flex flex-col items-center py-3 px-3"
    :class="[
      passed ? 'opacity-70' : 'opacity-100',
      currentActiveMatch ? 'bg-ice-blue' : 'bg-vanilla',
      currentActiveMatch ? 'hover:bg-vanilla' : 'bg-ice-blue',
    ]"
    @click="
      () => {
        if (clickFunc) clickFunc();
      }
    "
  >
    <p class="label text-label-2" v-if="currentActiveMatch">{{ 'CURRENT' }}</p>
    <div class="flex w-full">
      <div class="flex w-full">
        <span class="text-xs flex space-x-5">
          <img
            class="w-10 h-10"
            :src="`icon/game-${gameType?.replaceAll('_', '')}.svg`"
          />
          <p class="text">{{ $t(`games.${gameType}`) }}</p>
        </span>
      </div>

      <div class="flex space-x-3">
        <p class="text-xs font-bold w-full">Start</p>
        <p class="text-xs w-full">{{ start }}</p>
      </div>
    </div>
    <div
      v-if="twoteams"
      class="text-label-1 border-t-2 border-dark-blue flex flex-col w-full items-center"
    >
      <div class="flex space-x-5">
        <span
          class="flex flex-col justify-center"
          :class="{
            'text-xs': true,
            winner: team1 === winner,
            loser: team2 === winner,
          }"
        >
          <span>{{ team1 }}</span>
          <div v-if="passed">
            <p :class="[winner === team1 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team1 ? 'Winner' : 'Loser' }}
            </p>
            <p>{{ team1result }}</p>
          </div>
        </span>
        <img class="h-10 w-10" :src="`icon/match.png`" />
        <span
          class="flex flex-col justify-center"
          :class="{
            'text-xs': true,
            winner: team2 === winner,
            loser: team1 === winner,
          }"
        >
          <span>{{ team2 }}</span>
          <div v-if="passed">
            <p :class="[winner === team2 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team2 ? 'Winner' : 'Loser' }}
            </p>
            <p>{{ team2result }}</p>
          </div>
        </span>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const props = defineProps({
  gameType: String,
  gameAddOn: String,
  team1: String,
  team2: String,
  team1result: String,
  team2result: String,
  winner: String,
  start: String,
  clickFunc: Function,
  currentActiveMatch: String,
});

console.log(props.currentActiveMatch, 'current active match');

const passed = props.winner !== '';
const twoteams = props.team2 !== 'noteam';
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
  content: ' ';
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
