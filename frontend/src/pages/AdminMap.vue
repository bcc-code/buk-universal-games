<template>
  <div class="root">
    <div ref="map" class="map-wrapper">
      <img src="/images/map.jpg" alt="" />
    </div>
    <AdminMenu />
  </div>
</template>

<script>
import AdminMenu from "@/components/AdminMenu.vue";
import PinchZoom from "pinch-zoom-js";

export default {
  name: "MapPage",
  components: { AdminMenu },
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
}

.map-wrapper img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}
</style>
