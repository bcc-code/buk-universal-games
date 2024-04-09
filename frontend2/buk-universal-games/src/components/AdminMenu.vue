<template>
  <nav class="user-menu">
    <div class="user-menu-btn-small-wrapper">
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminLeagueStatus' }"
        @click="$router.push({ name: 'AdminLeagueStatus' })"><img src="../assets/icon/home.svg" /></button>
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminMatchListGame' }"
        @click="$router.push({ name: 'AdminMatchListGame' })"><img src="../assets/icon/calendar.svg" /></button>
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminGames' }"
        @click="$router.push({ name: 'AdminGames' })"><img src="../assets/icon/ball.svg" /></button>
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminMap' }"
        @click="$router.push({ name: 'AdminMap' })"><img src="../assets/icon/place.svg" /></button>
    </div>
  </nav>
  <CameraScanner ref="cameraScanner"></CameraScanner>
</template>

<script>
import CameraScanner from "@/components/CameraScanner.vue";

export default {
  name: "UserMenu",
  components: {
    CameraScanner,
  },
  methods: {},
};
</script>

<style scoped>
input[type="file"] {
  display: none;
}

.user-menu {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 2em;
  position: fixed;
  bottom: 0;
  width: 100%;
  max-width: 960px;
  margin: 0 auto;
  background-color: transparent;
  background-color: white;
}

button {
  background-color: inherit;
}

.user-menu-btn-small-wrapper {
  display: flex;
  width: 100%;
  justify-content: space-between;
}

.user-menu-btn-small {
  align-self: flex-start;
  padding: 0;
}

.user-menu-btn-small>img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--inactive-yellow-button)
}

.selected-component>img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--active-green-button);
}
</style>
