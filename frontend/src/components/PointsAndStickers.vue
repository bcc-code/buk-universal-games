<template>
  <section :class="{
    'bg-peach-50 py-3 px-3 border-1 border-peach-200 flex w-full rounded-md': true,
  }">
    <div class="w-40 mr-5">
      <img src="/image/logo_icon.svg" class="rounded-md" />
    </div>
    <div class="space-y-4 w-full">
      <div class="flex w-full space-x-6">
        <div class="w-full">
          <p class="text-label-2 label uppercase">{{ $t('team') }}</p>
          <h2 class="text-label-1">{{ teamName ?? '-' }}</h2>
        </div>
        <div class="w-full">
          <p class="text-label-2 label uppercase">{{ $t('points') }}</p>
          <h2 class="text-label-1">{{ formatPoints(teamPoints) }}</h2>
        </div>
      </div>
      <div class="flex w-full space-x-6">
        <div class="w-full">
          <p class="text-label-2 label uppercase">{{ $t('family') }}</p>
          <h2 class="text-label-1">{{ familyName ?? '-' }}</h2>
        </div>
        <div class="w-full">
          <p class="text-label-2 label uppercase">{{ $t('points') }}</p>
          <h2 class="text-label-1">{{ formatPoints(familyPoints) ?? '-' }}</h2>
        </div>
      </div>
    </div>
    <div>
      <LanguageSwitcher />
      <LogOutButton class="mt-3" />
    </div>
    <div class="align-middle flex">
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import LanguageSwitcher from './LanguageSwitcher.vue';
import LogOutButton from './LogOutButton.vue';
import { useFamilyStatus, useSigninResponse } from '@/hooks/hooks';
import { formatPoints } from './formatPoints';

const { data } = useSigninResponse()

const { data: familyStatus } = useFamilyStatus();

const teamName = computed(() => data.value?.team);
const familyName = computed(() => data.value?.familyName);
const familyPoints = computed(() => familyStatus.value?.myStatus?.familyPoints)
const teamPoints = computed(() => familyStatus.value?.myStatus?.teamPoints)
</script>
<style scoped>
button {
  background-color: inherit;
}

.card-dark {
  background-color: var(--dark);
  padding: 1em;
  border-radius: 1em;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  color: white;
}

.card-dark-column {
  display: flex;
  flex-direction: column;
}

.card-dark-heading {
  font-size: 0.85em;
}

.card-dark-text {
  color: var(--green);
}

.card-btn {
  border-radius: 0;
  margin-left: 0.5em;
  padding: 0.15em 0.15em 0.15em 0.15em;
}

.right-section {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
}

.loading {
  background-repeat: no-repeat;
  background-size: 24em 100%;
  background-image: linear-gradient(to right,
      var(--dark) 0%,
      hsl(323, 50%, 33%) 50%,
      var(--dark) 100%);
  animation-duration: 750ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

.logo {
  height: 5em;
  width: 5em;
  margin: -10px 0;
  background-color: var(--dark);
  color: white;
  border-radius: 1em;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}

.loading .card-dark-heading,
.loading h1 {
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
