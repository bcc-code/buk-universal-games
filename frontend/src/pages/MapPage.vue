<template>
  <div class="root">
    <div class="scroll-container" ref="scrollContainer">
      <PinchScrollZoom
        :width="containerWidth"
        :height="containerHeight"
        :min-scale="0.3"
        :max-scale="6"
        key-actions
        :within="false"
        :content-width="500"
        :content-height="500"
      >
        <img ref="mapImage" :src="map" alt="map" />
      </PinchScrollZoom>
    </div>

    <UserMenu />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import UserMenu from '../components/UserMenu.vue';
import { useSigninResponse } from '../hooks/hooks';
import PinchScrollZoom from '@coddicat/vue-pinch-scroll-zoom';

const { data } = useSigninResponse();

const leagueId = computed(() => data.value?.leagueId);

const map = computed(() => {
  const localLeagueId = leagueId.value;
  return localLeagueId ? `/image/Sone ${localLeagueId}.jpg` : undefined;
});

const scrollContainer = ref<HTMLDivElement | null>(null);
const containerWidth = ref(0);
const containerHeight = ref(0);

onMounted(() => {
  if (scrollContainer.value) {
    containerWidth.value = scrollContainer.value.clientWidth;
    containerHeight.value = scrollContainer.value.clientHeight;
  }
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
  height: 100vh;
  overflow: auto;
}
</style>
