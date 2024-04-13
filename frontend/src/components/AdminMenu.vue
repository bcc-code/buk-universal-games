<template>
  <nav
    class="flex w-full bottom-0 bg-ice-blue fixed z-10 align-middle p-2 justify-between"
  >
    <div class="flex w-1/2 justify-between align-middle">
      <button
        class="flex w-1/2 justify-between align-middle"
        :class="{
          'user-menu-btn-small': true,
          'selected-component': $route.name == 'AdminLeagueStatus',
        }"
        @click="$router.push({ name: 'AdminLeagueStatus' })"
      >
        <img class="h-10 w-10" src="/icon/leaderlist.png" />
      </button>
    </div>
    <div class="justify-center flex w-full">
      <button
        class="flex w-1/2 justify-between align-middle"
        :class="{
          'user-menu-btn-big': true,
          'selected-component': $route.name == 'AdminMatchListGame',
        }"
        @click="$router.push({ name: 'AdminMatchListGame' })"
      >
        <img class="h-10 w-10" src="/icon/match.png" />
      </button>
    </div>
    <div class="flex w-1/2 justify-between">
      <button
        class="flex w-1/2 justify-between align-middle"
        :class="{
          'user-menu-btn-small': true,
          'selected-component': $route.name == 'AdminMap',
        }"
        @click="$router.push({ name: 'AdminMap' })"
      >
        <img class="h-10 w-10" src="/icon/place.svg" />
      </button>
    </div>
  </nav>
  <CameraScanner ref="cameraScanner"></CameraScanner>
</template>

<script>
import CameraScanner from '@/components/CameraScanner.vue';

export default {
  name: 'UserMenu',
  components: {
    CameraScanner,
  },
  methods: {},
};
</script>

<style scoped>
input[type='file'] {
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

.selected-component > img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--active-green-button);
}
.user-menu-btn-big {
  position: absolute;
  height: 5em;
  width: 5em;
  top: -1.5em;
  background-color: var(--ice-blue);
  color: white;
  border-radius: 10em;
  display: flex;
  padding: 10px;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}
</style>
