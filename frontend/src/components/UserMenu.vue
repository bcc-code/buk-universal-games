<template>
  <nav
    class="fixed bottom-0 w-full bg-ice-blue z-10 p-2 flex justify-around items-center"
  >
    <button
      :class="{
        'selected-component': isSelected('LeagueList'),
      }"
      @click="navigateTo('LeagueList')"
      class="flex flex-col items-center"
    >
      <img class="h-10 w-10" src="/icon/leaderlist.png" />
    </button>

    <button
      class="flex flex-col items-center user-menu-btn-big bg-white"
      @click="navigateTo('MatchList')"
      :class="{
        'selected-component': isSelected('MatchList'),
      }"
    >
      <img src="/icon/match.png" class="h-20 w-20" />
    </button>

    <button
      :class="{
        'selected-component': isSelected('Map'),
      }"
      @click="navigateTo('Map')"
      class="flex flex-col items-center"
    >
      <img class="h-10 w-10" src="/icon/place.svg" />
    </button>
  </nav>
</template>

<script>
import { defineComponent, reactive, toRefs } from 'vue';
import { useRouter, useRoute } from 'vue-router';

export default defineComponent({
  name: 'UserMenu',
  setup() {
    const router = useRouter();
    const route = useRoute();

    const state = reactive({
      selectedRoute: route.name,
    });

    const navigateTo = (name) => {
      router.push({ name });
      state.selectedRoute = name;
    };

    const isSelected = (name, additionalName = null) => {
      return (
        state.selectedRoute === name || state.selectedRoute === additionalName
      );
    };

    return { ...toRefs(state), navigateTo, isSelected };
  },
});
</script>

<style scoped>
input[type='file'] {
  display: none;
}

.user-menu-btn-small > img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--dark-brown);
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

.selected-component > img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--active-green-button);
}
</style>
