<template>
  <nav class="fixed bottom-0 w-full bg-ice-blue z-10 p-2 flex justify-around items-center mx-auto"
    style="max-width: 960px;">
    <button class="flex flex-col items-center w-1/3" :class="{
      'selected-component': $route.name == 'AdminLeagueStatus',
    }" @click="$router.push({ name: 'AdminLeagueStatus' })">
      <img class="h-10 w-10" src="/icon/leaderlist.png" />
    </button>
    <button class="flex flex-col items-center user-menu-btn-big bg-white w-1/3" :class="{
      'selected-component': $route.name == 'AdminMatchListGame',
    }" @click="$router.push({ name: 'AdminMatchListGame' })">
      <img class="h-20 w-20" src="/icon/match.png" />
    </button>
    <button class="flex flex-col items-center w-1/3" :class="{
      'selected-component': $route.name == 'AdminMap',
    }" @click="$router.push({ name: 'AdminMap' })">
      <img class="h-10 w-10" src="/icon/place.svg" />
    </button>
  </nav>
</template>

<script setup lang="ts">
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

.selected-component>img {
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
