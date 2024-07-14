<template>
  <div class="root">
    <a :href="map" target="_blank">
      <img ref="mapImage" :src="map" alt="map" />
    </a>
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
}

.root img {
  width: 100%;
  height: auto;
  object-fit: contain;
}
</style>
