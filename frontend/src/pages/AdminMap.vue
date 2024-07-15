<template>
  <div class="root">
    <div class="p-4">
      <div class="filters">
        <div class="filter">
          <p>Zone:</p>
          <div class="single-filter flex">
            <button
              v-for="league in adminLeagues"
              :key="league.id"
              :class="[
                'px-4 py-2 m-2 rounded',
                league.id === currentLeague
                  ? 'bg-dark-brown text-white'
                  : 'bg-vanilla',
              ]"
              @click="selectLeague(league.id)"
            >
              {{ league.name }}
            </button>
          </div>
        </div>
      </div>
    </div>

    <div class="scroll-container" ref="scrollContainer">
      <PinchScrollZoom
        ref="zoomer"
        :width="containerWidth"
        :height="containerHeight"
        :min-scale="0.3"
        :max-scale="6"
        :content-width="500"
        :content-height="500"
        key-actions
        :within="false"
      >
        <img ref="mapImage" :src="map" alt="map" />
      </PinchScrollZoom>
    </div>

    <AdminMenu />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue';
import { useStore } from 'vuex';
import AdminMenu from '@/components/AdminMenu.vue';
import PinchScrollZoom, {
  type PinchScrollZoomExposed,
} from '@coddicat/vue-pinch-scroll-zoom';

const store = useStore();
const currentLeague = computed(() => store.state.adminLeagueSelected);
const adminLeagues = computed(() => store.state.adminLeagues);

const map = computed(() => {
  const leagueId = currentLeague.value;
  return leagueId ? `/image/Sone ${leagueId}.jpg` : undefined;
});

const scrollContainer = ref<HTMLDivElement | null>(null);
const containerWidth = ref(0);
const containerHeight = ref(0);
const zoomer = ref<PinchScrollZoomExposed>();

const selectLeague = async (id: number) => {
  await store.dispatch('setAdminLeagueSelected', id);
  resetZoom();
};

const resetZoom = () => {
  zoomer.value?.setData({
    scale: 1,
    originX: containerWidth.value / 2,
    originY: containerHeight.value / 2,
    translateX: 0,
    translateY: 0,
  });
};

onMounted(() => {
  if (scrollContainer.value) {
    containerWidth.value = scrollContainer.value.clientWidth;
    containerHeight.value = scrollContainer.value.clientHeight;
  }
});

watch(currentLeague, () => {
  resetZoom();
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

.filters {
  display: flex;
  flex-direction: column;
  gap: 0.5em;
}

.filter {
  display: flex;
  flex-direction: column;
}

.single-filter {
  display: flex;
  align-items: center;
  width: 100%;
  overflow: auto;
}
</style>
