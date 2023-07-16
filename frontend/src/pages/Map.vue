<template>
  <div class="root">
    <div ref="map" class="map-wrapper">
      <img :src="map" alt="" />
      <img :src="icons" style="position:absolute;" alt="" />
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
      map: null,
      icons: null,
    };
  },
  created() {
    if (this.league == "k") {
      this.map = "/image/ubg-beach-small.png";
    } else {
      this.map = "/image/ubg-arena-small.png";
      this.defaultZoom
    }
    this.icons = `/image/ubg-${this.league}-liga-icons.svg`;
  },
  mounted() {
    new PinchZoom(this.$refs.map, {
      minZoom: 1,
      maxZoom: 10,
      animationDuration: 150,
      tapZoomFactor: 3,
      draggableUnzoomed: true,
    });
  },
  methods: {},
  computed: {
    league() {
      return this.$store.state.loginData.league?.substring(0, 1).toLowerCase();
    },
  }
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
