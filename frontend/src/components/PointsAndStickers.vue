<template>
  <section :class="{ 'bg-vanilla py-3 px-3 border-r-2 border-dark flex justify-between rounded-md': true, loading }">
    <div class="h-10 w-10">
      <img src="image/logo_icon.svg" class="rounded-md" />
    </div>
    <div class="">
      <p class="text-label-2 label uppercase">{{ $t('team') }}</p>
      <h2 class="text-label-1 ">{{ teamName ?? "-" }}</h2>
    </div>
    <div class="card-dark-column">
      <p class="text-label-2 label uppercase">{{ $t('points') }}</p>
      <h1 class="text-label-1">{{ points ?? "-" }}</h1>
    </div>
    <div></div>
    <div class="align-middle flex">
      <LanguageSwitcher />
      <!--       <button class="ml-3 card-btn" @click="refresh"><img src="icon/refresh.svg" /></button> -->
      <LogOut />
    </div>
  </section>
</template>

<script>
import LanguageSwitcher from "@/components/LanguageSwitcher.vue";

export default {
  name: "PointsAndStickers",
  components: { LanguageSwitcher },
  props: {
    points: Number,
    stickers: Number,
    refresh: Function,
    teamName: String,
    loading: {
      required: false,
      type: Boolean,
      default: false,
    },
  },
  methods: {},
};
</script>

<style scoped>
button {
  background-color: inherit;
}

.card-dark {
  background-color: var(--dark);
  padding: 1em;
  border-radius: 1em;
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  color: white;
}

.card-dark-column {
  display: flex;
  flex-direction: column;
}

.card-dark-heading {
  font-size: 0.85em;
}

.card-dark-text {
  color: var(--green);
}

.card-btn {
  border-radius: 0;
  margin-left: .5em;
  padding: 0.15em 0.15em 0.15em 0.15em;
}

.right-section {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
}

.loading {
  background-repeat: no-repeat;
  background-size: 24em 100%;
  background-image: linear-gradient(to right, var(--dark) 0%, hsl(323, 50%, 33%) 50%, var(--dark) 100%);
  animation-duration: 750ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

.logo {
  height: 5em;
  width: 5em;
  margin: -10px 0;
  background-color: var(--dark);
  color: white;
  border-radius: 1em;
  display: flex;
  justify-content: center;
  align-items: center;
  border: 5px solid white;
}

.loading .card-dark-heading,
.loading h1 {
  color: hsl(323, 50%, 33%);
  background-color: hsl(323, 50%, 33%);
}

@keyframes shimmer {
  0% {
    background-position: -24em 0;
  }

  100% {
    background-position: 24em 0;
  }
}
</style>
