<template>
  <div class="root">
    <img ref="mapImage" :src="map" alt="map" />
    <UserMenu />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import PinchZoom from 'pinch-zoom-js';
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

onMounted(() => {
  const mapImage = document.querySelector(
    '.map-wrapper img',
  ) as HTMLImageElement | null;

  if (mapImage) {
    new PinchZoom(mapImage, {
      minZoom: 1,
      maxZoom: 10,
      animationDuration: 150,
      tapZoomFactor: 3,
      draggableUnzoomed: true,
    });
  }
});
</script>

<style scoped>
.root {
  width: 100%;
  height: 100%;
}
</style>
