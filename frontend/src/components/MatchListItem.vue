<template>
  <section class="rounded-md flex flex-col items-center py-3 px-3" :class="[
    passed ? 'opacity-70' : 'opacity-100',
    currentActiveMatch
      ? 'bg-yellow-50 border-yellow-100 border-1 shadow-md'
      : 'bg-white border-ice-200 border-1 shadow-md',
  ]" @click="() => {
    clickFunc?.();
  }
    ">
    <p class="label text-label-2" v-if="currentActiveMatch">{{ 'CURRENT' }}</p>
    <div class="flex w-full">
      <div class="flex w-full">
        <span class="text-xs flex space-x-5">
          <img class="w-10 h-10" :src="`icon/game-${gameType.replace(/_/g, '')}.svg`" />
          <p class="text">{{ $t(`games.${gameType}`) }}</p>

          <div v-if="$route.name === 'AdminMatchListGame' && !twoteams" class="flex space-x-3">
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
            <p v-if="winner" :class="[winner === team1 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team1 ? 'Winner' : 'Loser' }}
            </p>
            <p v-if="passed">{{ formatPoints(team1result ?? 0) }}</p>
          </div>
        </span>
        <img v-if="twoteams" class="h-10 w-10" :src="`icon/match.png`" />
        <span class="flex flex-col justify-center text-xs" :class="{
          winner: team2 === winner,
          loser: team1 === winner,
        }">
          <span v-if="twoteams">{{ team2 }}</span>
          <div>
            <p v-if="winner && passed && twoteams" :class="[winner === team2 ? 'text-green-500' : 'text-red-700']">
              {{ winner === team2 ? 'Winner' : 'Loser' }}
            </p>
            <p v-if="passed">{{ formatPoints(team2result ?? 0) }}</p>
          </div>
        </span>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
const props = defineProps<{
  gameType: string;
  gameAddOn: string;
  team1: string;
  team2: string;
  team1result: number | null;
  team2result: number | null;
  start: string;
  clickFunc?: () => void;
  currentActiveMatch?: boolean;
}>();

import { computed } from 'vue';
import { formatPoints } from './formatPoints';

const passed = computed(() => props.team1result !== null);
const twoteams = computed(() => props.team2 !== props.team1);

const winner = computed(() => {
  if (props.team1result !== null && props.team2result !== null) {
    if (props.team1result > props.team2result) {
      return props.team1;
    } else if (props.team1result < props.team2result) {
      return props.team2;
    }
  }
  return '';
});
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
