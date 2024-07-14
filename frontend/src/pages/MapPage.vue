<template>
  <div class="root">
    <div ref="mapWrapper" class="map-wrapper">
      <img ref="mapImage" :src="map" alt="map" />
    </div>
    <UserMenu />
  </div>
</template>
<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import PinchZoom from 'pinch-zoom-js';
import UserMenu from '@/components/UserMenu.vue';
import { useStore } from 'vuex';

const store = useStore();

const mapWrapper = ref<HTMLElement | null>(null);
const mapImage = ref<HTMLImageElement | null>(null);
const map = ref<string | null>(null);

const league = computed(() => {
  return store.state.loginData.league?.substring(0, 1).toLowerCase();
});

const initializeMap = () => {
  if (league.value === 'k') {
    map.value = '/image/ubg-beach-small.png';
  } else {
    map.value = '/image/ubg-arena-small.png';
  }
};

onMounted(() => {
  initializeMap();
  mapWrapper.value = document.querySelector(
    '.map-wrapper',
  ) as HTMLElement | null;
  mapImage.value = document.querySelector(
    '.map-wrapper img',
  ) as HTMLImageElement | null;

  if (mapWrapper.value) {
    new PinchZoom(mapWrapper.value, {
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

.map-wrapper {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #5fa46e;
  background-image: linear-gradient(135deg, #5fa46e, #118144);
}

.map-wrapper img {
  height: 100%;
  width: 100%;
  object-fit: contain;
  display: block;
}
</style>
