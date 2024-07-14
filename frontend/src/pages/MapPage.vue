<template>
  <div class="root">
    <div class="scroll-container">
      <a :href="map" target="_blank">
        <img ref="mapImage" :src="map" alt="map" />
      </a>
    </div>
    <UserMenu />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import UserMenu from '../components/UserMenu.vue';
import { useSigninResponse } from '../hooks/hooks';

const { data } = useSigninResponse();

const leagueId = computed(() => {
  return data.value?.leagueId;
});

const map = computed(() => {
  const localLeagueId = leagueId.value;
  return localLeagueId ? `/image/Sone ${localLeagueId}.jpg` : undefined;
});
</script>

<style scoped>
.root {
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
}

.scroll-container {
  width: 100%;
  height: 100%;
  overflow: auto;
}

.scroll-container img {
  width: 2000px; /* Fixed large width */
  height: auto; /* Maintain aspect ratio */
  object-fit: contain;
  max-width: unset;
}
</style>
