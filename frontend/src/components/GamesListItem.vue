<template>
  <section
    class="bg-vanilla hover:bg-ice-blue rounded-md flex items-center py-3 px-5"
  >
    <img
      class="h-10 w-10 mr-7"
      :src="`icon/game-${gameType.replace(/_/g, '')}.svg`"
    />
    <p class="w-full">{{ $t(`games.${gameType}`) }}</p>
    <p class="w-full">{{ gameStart }}</p>
    <div>
      <CheckIcon class="h-4 text-dark-blue" />
    </div>
  </section>
</template>

<script setup lang="ts">
import { CheckIcon } from '@heroicons/vue/24/solid';
import { cva, type VariantProps } from 'class-variance-authority';
console.log('STOP');
const gameslistitem = cva('gameslistitem', {
  variants: {
    result: {
      true: true,
      false: false,
    },
    twoteams: {
      true: true,
      false: false,
    },
    passed: {
      true: true,
      false: false,
    },
  },
  compoundVariants: [{ result: false, twoteams: false, passed: false }],
});

type GamesListItemProps = VariantProps<typeof gameslistitem>;

withDefaults(
  defineProps<{
    gameId: string | number;
    gameName?: string;
    gameType: string;
    gameStart: string;
    team1Name?: string;
    team2Name?: string;
    team1Id?: number;
    team2Id?: number;
    team1Result?: number;
    team2Result?: number;
    winnerId?: number;
    winnerResult?: number;
    result: GamesListItemProps['result'];
    twoteams: GamesListItemProps['twoteams'];
    passed: GamesListItemProps['passed'];
  }>(),
  {
    result: false,
    twoteams: false,
    passed: false,
  },
);
</script>

<style scoped>
button {
  background-color: inherit;
}

.card {
  padding: 0 1em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 10% 75% 15%;
}

.card-row {
  display: flex;
  align-items: center;
  width: 100%;
}

.card-dark {
  background-color: var(--label-1);
  color: white;
}

.card-light {
  background-color: white;
  color: var(--label-1);
}

.card-dark-column:nth-child(-n + 3) {
  align-items: flex-start;
}

.card-dark-column:nth-last-child(-n + 1) {
  align-items: flex-end;
}

.card-dark-column:nth-last-child(-n + 2) {
  align-items: flex-end;
}

.card-dark-text {
  font-size: 0.85em;
  color: inherit;
  white-space: nowrap;
}

.hide-text-overflow {
  text-overflow: ellipsis;
  overflow: hidden;
}

.card-btn {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 0;
  padding: 0.15em 0.15em 0.15em 0.65em;
}

.buk-icon {
  margin-right: 0.75em;
}

.loading {
  background-repeat: no-repeat;
  background-size: 24em 100%;
  background-image: linear-gradient(
    to right,
    transparent 0%,
    hsl(323, 50%, 33%) 50%,
    transparent 100%
  );
  animation-duration: 1000ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

.loading .card-dark-text,
.loading .buk-icon {
  color: hsl(323, 50%, 33%);
  background-color: hsl(323, 50%, 33%);
}

@keyframes shimmer {
  0% {
    background-position: -24em 0;
  }

  100% {
    background-position: 24em 0;
  }
}
</style>
