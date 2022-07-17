<template>
  <nav class="user-menu">
    <div class="user-menu-btn-small-wrapper">
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'LeagueList' }"
        v-html="homeIcon"
        @click="$router.push({ name: 'LeagueList', params: { code: $store.state.loginData.code } })"
      ></button>
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'MatchList' }"
        v-html="calendarIcon"
        @click="$router.push({ name: 'MatchList', params: { code: $store.state.loginData.code } })"
      ></button>
    </div>
    <div class="user-menu-btn-big-wrapper">
      <button class="user-menu-btn-big" v-on:click="$refs.cameraScanner.start()">
        <p v-html="scanIcon"></p>
      </button>
    </div>
    <div class="user-menu-btn-small-wrapper">
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'GameInfo' }"
        v-html="ballIcon"
        @click="$router.push({ name: 'GameInfo', params: { code: $store.state.loginData.code } })"
      ></button>
      <button
        :class="{ 'user-menu-btn-small': true, 'selected-component': $route.name == 'Map' }"
        v-html="placeIcon"
        @click="$router.push({ name: 'Map', params: { code: $store.state.loginData.code } })"
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
  width: 50%;
  justify-content: space-between;
}

.user-menu-btn-small {
  align-self: flex-start;
  padding: 0;
  color: var(--gray-2);
}

.user-menu-btn-big-wrapper {
  width: 100%;
  display: flex;
  justify-content: center;
}

.user-menu-btn-big {
  position: absolute;
  height: 5em;
  width: 5em;
  top: -1.5em;
  background-color: var(--dark);
  color: white;
  border-radius: 10em;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}

.selected-component {
  color: var(--green);
}
</style>
