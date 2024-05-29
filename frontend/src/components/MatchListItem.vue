<template>
  <section class="rounded-md flex flex-col items-center py-3 px-3 bg-gray-200" :class="[
    passed ? 'bg-gray-200' : 'bg-white',
    currentActiveMatch
      ? 'bg-yellow-50 border-yellow-100 border-1'
      : 'bg-white border-ice-200 border-1',
    clickFunc ? 'shadow-lg' : '',
  ]" @click="() => {
    clickFunc?.();
  }">
    <div class="flex w-full justify-between items-center mb-1">
      <div class="flex items-center space-x-5">
        <img class="w-10 h-10" :src="`icon/game-${gameType.replace(/_/g, '')}.svg`" />
        <p class="text font-bold">{{ $t(`games.${gameType}`) }}</p>
      </div>
      <div class="flex items-center space-x-3 flex-col">
        <p class="text-right">
          <span class="text-xs font-bold">Start: </span>
          <span class="text-xs">{{ start }}</span>
        </p>
        <div v-if="clickFunc" class="flex items-center space-x-1">
          <p class="text-xs">Details</p>
          <ArrowRightIcon class="h-4 w-4" />
        </div>
      </div>
    </div>
    <hr class="w-full border-b-1 border-dark-blue mt-2 mb-4" />

    <div v-if="twoteams" class="text-label-1 flex w-full justify-between items-center">
      <span class="flex flex-col items-center w-1/3" :class="{ winner: team1 === winner, loser: team2 === winner }">
        <span class="font-bold">{{ team1 }}</span>
        <p v-if="winner" :class="[winner === team1 ? 'text-green-500' : 'text-red-700']">{{ winner === team1 ? 'Winner'
          : 'Loser' }}</p>
        <p>+ {{ formatPoints(team1result ?? 0) }}</p>
      </span>
      <img class="h-10 w-10 mx-5" :src="`icon/match.png`" />
      <span class="flex flex-col items-center w-1/3" :class="{ winner: team2 === winner, loser: team1 === winner }">
        <span class="font-bold">{{ team2 }}</span>
        <p v-if="winner" :class="[winner === team2 ? 'text-green-500' : 'text-red-700']">{{ winner === team2 ? 'Winner'
          : 'Loser' }}</p>
        <p>+ {{ formatPoints(team2result ?? 0) }}</p>
      </span>
    </div>
    <div v-else class="w-full mt-4 text-center">
      <p class="font-bold">{{ team1 }}</p>
      <p v-if="passed">+ {{ formatPoints(team1result ?? 0) }}</p>
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
import { ArrowRightIcon } from '@heroicons/vue/24/solid';

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
.winner {
  color: var(--green);
}

.loser {
  color: var(--red);
}

.text-xs {
  font-size: 0.75rem;
}

.text-label-1 {
  font-size: 1rem;
}

.text-label-2 {
  font-size: 1.25rem;
}

.border-b-1 {
  border-bottom-width: 1px;
}

.border-dark-blue {
  border-color: #b9c9da;
}

.bg-gray-200 {
  background-color: #e5e5e5;
}

.bg-yellow-50 {
  background-color: #fef3c7;
}

.border-yellow-100 {
  border-color: #fde68a;
}

.bg-white {
  background-color: white;
}

.border-ice-200 {
  border-color: #e0f2fe;
}

.rounded-md {
  border-radius: 0.375rem;
}

.flex {
  display: flex;
}

.items-center {
  align-items: center;
}

.justify-between {
  justify-content: space-between;
}

.w-full {
  width: 100%;
}

.space-x-3> :not([hidden])~ :not([hidden]) {
  --tw-space-x-reverse: 0;
  margin-right: calc(0.75rem * var(--tw-space-x-reverse));
  margin-left: calc(0.75rem * calc(1 - var(--tw-space-x-reverse)));
}

.space-x-5> :not([hidden])~ :not([hidden]) {
  --tw-space-x-reverse: 0;
  margin-right: calc(1.25rem * var(--tw-space-x-reverse));
  margin-left: calc(1.25rem * calc(1 - var(--tw-space-x-reverse)));
}

.text-center {
  text-align: center;
}

.mt-4 {
  margin-top: 1rem;
}
</style>
