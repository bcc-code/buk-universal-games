<template>
  <nav class="flex w-full bottom-0 bg-ice-blue fixed z-10 p-2 justify-between">
    <div class="flex w-1/2 justify-between align-middle">
      <button
        :class="{
          'align-start': true,
          'selected-component': isSelected('LeagueList'),
        }"
        @click="navigateTo('LeagueList')"
      >
        <img class="h-10 w-10" src="/icon/leaderlist.png" />
      </button>
    </div>
    <div class="justify-center flex w-full">
      <button
        class="user-menu-btn-big bg-white"
        @click="navigateTo('MatchList')"
        :class="{
          'selected-component': isSelected('MatchList'),
        }"
      >
        <img src="/icon/match.png" style="width: 5em" />
      </button>
    </div>
    <div class="flex w-1/2 justify-between">
      <button
        :class="{
          'align-start': true,
          'selected-component': isSelected('Map'),
        }"
        @click="navigateTo('Map')"
      >
        <img src="/icon/place.svg" />
      </button>
    </div>
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
