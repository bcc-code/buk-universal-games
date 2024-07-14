<template>
  <div class="root">
    <div ref="map" class="map-wrapper relative">
      <img :src="map" alt="" />
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

// import PinchZoom from 'pinch-zoom-js'

//   new PinchZoom(this.$refs.map, {
//     minZoom: 1,
//     maxZoom: 10,
//     animationDuration: 150,
//     tapZoomFactor: 3,
//     draggableUnzoomed: true
//   })
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
}
</style>
