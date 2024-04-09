<template>
  <nav class="user-menu">
    <div class="user-menu-btn-small-wrapper">
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': isSelected('LeagueList') }"
        @click="navigateTo('LeagueList')"><img src="/icon/home.svg" /></button>

      <button :class="{ 'user-menu-btn-small': true, 'selected-component': isSelected('MatchList') }"
        @click="navigateTo('MatchList')"><img src="/icon/calendar.svg" /></button>
    </div>
    <div class="user-menu-btn-big-wrapper">
      <button class="user-menu-btn-big" @click="navigateTo('SideQuest')">
        <img src="/icon/sidequest.svg" style="width:5em" />
      </button>
    </div>
    <div class="user-menu-btn-small-wrapper">
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': isSelected('GameInfo', 'GameInfoDetail') }"
        @click="navigateTo('GameInfo')"><img src="/icon/ball.svg" /></button>
      <button :class="{ 'user-menu-btn-small': true, 'selected-component': isSelected('Map') }"
        @click="navigateTo('Map')"><img src="/icon/place.svg" /></button>
    </div>
  </nav>
</template>

<script>
import { defineComponent, reactive, toRefs } from 'vue'
import { useRouter, useRoute } from 'vue-router'

export default defineComponent({
  name: "UserMenu",
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
      return state.selectedRoute === name || state.selectedRoute === additionalName;
    };

    return { ...toRefs(state), navigateTo, isSelected };
  },
});
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
  width: 50%;
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
  padding: 3px;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}

.selected-component>img {
  /* This trick allows us to colourise <img /> tag SVGs. */
  filter: var(--active-green-button);
}
</style>
