<template>
  <section class="rounded-md flex flex-col items-center py-3 px-3" :class="[
    passed ? 'opacity-70' : 'opacity-100',
    currentActiveMatch ? 'bg-ice-blue' : 'bg-vanilla',
    currentActiveMatch ? 'hover:bg-vanilla' : 'bg-ice-blue',
  ]" @click="() => {
    clickFunc();
  }
    ">
    <p class="label text-label-2" v-if="currentActiveMatch">{{ 'CURRENT' }}</p>
    <div class="flex w-full">
      <div class="flex w-full">
        <span class="text-xs flex space-x-5">
          <img class="w-10 h-10" :src="`icon/game-${gameType.replace(/_/g, '')}.svg`" />
          <p class="text">{{ $t(`games.${gameType}`) }}</p>

          <div v-if="$route.name === 'AdminMatchListGame'" class="flex space-x-3">
            <p class="text-xs font-bold w-7">Team:</p>
            <p class="text-xs w-full">{{ team1 }}</p>
          </div>
        </span>
      </div>

      <div class="flex space-x-3">
        <p class="text-xs font-bold w-full">Start</p>
        <p class="text-xs w-full">{{ start }}</p>
      </div>
    </div>
    <div v-if="twoteams || passed" class="text-label-1 flex flex-col w-full items-center">
      <hr class="w-full border-b-1 border-dark-blue mt-2 mb-4" />

      <div class="flex space-x-5">
        <span class="flex flex-col justify-center" :class="{
          'text-xs': true,
          winner: team1 === winner,
          loser: team2 === winner,
        }">
          <span v-if="twoteams">{{ team1 }}</span>
          <div v-if="passed && twoteams">
            <p :class="[winner === team1 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team1 ? 'Winner' : 'Loser' }}
            </p>
            <p v-if="passed">{{ team1result }} {{ $t('points') }}</p>
          </div>
        </span>
        <img v-if="twoteams" class="h-10 w-10" :src="`icon/match.png`" />
        <span class="flex flex-col justify-center text-xs" :class="{
          winner: team2 === winner,
          loser: team1 === winner,
        }">
          <span v-if="twoteams">{{ team2 }}</span>
          <div>
            <p v-if="passed && twoteams" :class="[winner === team2 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team2 ? 'Winner' : 'Loser' }}
            </p>
            <p v-if="passed">{{ team2result }} {{ $t('points') }}</p>
          </div>
        </span>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const props = defineProps<{
  gameType: string,
  gameAddOn: string,
  team1: string,
  team2: string,
  team1result: string,
  team2result: string,
  winner: string,
  start: string,
  clickFunc: () => void,
  currentActiveMatch: boolean,
}>();

const passed = props.team1result !== null;
const twoteams = props.team2 !== props.team1;
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
