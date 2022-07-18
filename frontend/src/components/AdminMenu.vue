<template>
  <nav class="user-menu">
    <div class="user-menu-btn-small-wrapper">
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminLeagueStatus' }"
        v-html="homeIcon"
        @click="$router.push({ name: 'AdminLeagueStatus' })"
      ></button>
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminMatchListGame' }"
        v-html="calendarIcon"
        @click="$router.push({ name: 'AdminMatchListGame' })"
      ></button>
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminGames' }"
        v-html="ballIcon"
        @click="$router.push({ name: 'AdminGames' })"
      ></button>
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'AdminMap' }"
        v-html="placeIcon"
        @click="$router.push({ name: 'AdminMap' })"
      ></button>
    </div>
  </nav>
  <CameraScanner ref="cameraScanner"></CameraScanner>
</template>

<script>
import CameraScanner from "@/components/CameraScanner.vue";

import { ballIcon } from "@/assets/icons/ball.svg.ts";
import { homeIcon } from "@/assets/icons/home.svg.ts";
import { calendarIcon } from "@/assets/icons/calendar.svg.ts";
import { placeIcon } from "@/assets/icons/place.svg.ts";
import { scanIcon } from "@/assets/icons/scan.svg.ts";

export default {
  name: "UserMenu",
  components: {
    CameraScanner,
  },
  data() {
    return {
      ballIcon,
      homeIcon,
      calendarIcon,
      placeIcon,
      scanIcon,
    };
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
  max-width: 480px;
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
  color: var(--gray-2);
}

.selected-component {
  color: var(--green);
}
</style>
