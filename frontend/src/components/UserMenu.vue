<template>
  <nav class="fixed bottom-0 w-full bg-teal-50 z-10 p-2 flex justify-around items-center" style="max-width: 960px">
    <button class="flex flex-col items-center w-1/3" :class="{
      'selected-component': $route.name === ('LeagueList'),
    }" @click="$router.push({ name: 'LeagueList' })">
      <img class="h-10 w-10" src="/icon/leaderlist.png" />
    </button>

    <button class="flex flex-col items-center user-menu-btn-big bg-white w-1/3"
      @click="$router.push({ name: 'MatchList' })" :class="{
        'selected-component': $route.name === ('MatchList'),
      }">
      <img src="/icon/match.png" class="h-20 w-20" />
    </button>

    <button class="flex flex-col items-center w-1/3" :class="{
      'selected-component': $route.name === ('Map'),
    }" @click="$router.push({ name: 'Map' })">
      <img class="h-10 w-10" src="/icon/place.svg" />
    </button>
  </nav>
</template>

<script setup lang="ts">
</script>

<style scoped>
.user-menu-btn-big {
  position: absolute;
  height: 5em;
  width: 5em;
  top: -1.5em;
  background-color: var(--teal-50);
  color: white;
  border-radius: 10em;
  display: flex;
  padding: 10px;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}

.selected-component>img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--active-yellow-button);
}
</style>
