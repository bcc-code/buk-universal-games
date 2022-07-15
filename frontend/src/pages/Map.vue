<template>
  <div class="root">
    <div ref="map" class="map-wrapper">
      <img src="/images/map.jpg" alt="" />
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
    const pinchZoom = new PinchZoom(this.$refs.map, {
      minZoom: 1,
      maxZoom: 10,
      animationDuration: 150,
      tapZoomFactor: 5,
    });
    console.log("pinchZoom", pinchZoom);
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
