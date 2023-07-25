<template>
  <div class="root">
    <div ref="mapB" class="map-wrapper">
      <img src="/image/ubg-arena-small.png" alt="" />
      <img src="/image/ubg-b-liga-icons.svg" style="position:absolute;" alt="" />
      <span class="overlay-title-label">B-League - Arena</span>
    </div>
    <div ref="mapU" class="map-wrapper">
      <img src="/image/ubg-arena-small.png" alt="" />
      <img src="/image/ubg-u-liga-icons.svg" style="position:absolute;" alt="" />
      <span class="overlay-title-label">U-League - Arena</span>
    </div>
    <div ref="mapK" class="map-wrapper">
      <img src="/image/ubg-beach-small.png" alt="" />
      <img src="/image/ubg-k-liga-icons.svg" style="position:absolute;" alt="" />
      <span class="overlay-title-label">K-League - Beach</span>
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
  position: relative;
  margin-bottom:1em;
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

.overlay-title-label {
  position: absolute;
  background-color: var(--dark);
  top: 1em;
  right: 1em;
  text-align: center;
  padding: 1em;
  font-size: .5em;
  border-radius: 0.5em;
  color: #fff;
  box-shadow: 3px 3px 5px rgba(0,0,0,0.5);
  font-weight: bold;
}
</style>
