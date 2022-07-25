<template>
  <div class="root">
    <div ref="mapB" class="map-wrapper">
      <img src="/images/map-B-liga.png" alt="" />
    </div>
    <div ref="mapU" class="map-wrapper">
      <img src="/images/map-U-liga.png" alt="" />
    </div>
    <div ref="mapK" class="map-wrapper">
      <img src="/images/map-K-liga.png" alt="" />
    </div>
    <AdminMenu />
  </div>
</template>

<script>
import AdminMenu from "@/components/AdminMenu.vue";

export default {
  name: "MapPage",
  components: { AdminMenu },
  data() {
    return {
      loginError: "Map",
    };
  },
  mounted() {},
  methods: {},
};
</script>

<style scoped>
.root {
  width: 100%;
  background-color: #5fa46e;
  background-image: linear-gradient(135deg, #5fa46e, #118144);
  padding-top: 1em;
  padding-bottom: 10em;
}
.map-wrapper {
  width: 100%;
  height: 100%;
  display: block;
  padding-bottom: 1em;
}

.map-wrapper img {
  height: 100%;
  width: 100%;
  object-fit: contain;
}
</style>
