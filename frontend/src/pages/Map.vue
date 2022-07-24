<template>
  <div class="root">
    <div ref="map" class="map-wrapper">
      <img :src="`/images/map-${$store.state.loginData.league.substring(0, 1).toUpperCase()}-liga.png`" alt="" />
    </div>
    <UserMenu />
  </div>
</template>

<script>
import UserMenu from "@/components/UserMenu.vue";
import PinchZoom from "pinch-zoom-js";

export default {
  name: "MapPage",
  components: { UserMenu },
  data() {
    return {
      loginError: "Map",
    };
  },
  mounted() {
    new PinchZoom(this.$refs.map, {
      minZoom: 1,
      maxZoom: 10,
      animationDuration: 150,
      tapZoomFactor: 5,
      draggableUnzoomed: true,
    });
  },
  methods: {},
};
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
