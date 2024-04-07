<template>
  <section :class="{ 'bg-vanilla py-5 px-3 border-r-2 border-dark flex justify-between rounded-md': true, loading }">
    <div class="card-dark-column">
      <img src="image/logo_icon.svg" class="logo" />
    </div>
    <div class="card-dark-column">
      <div class="text-dark-blue">{{ $t('points') }}</div>
      <h1 class="card-dark-text">{{ points ?? "-" }}</h1>
    </div>
    <div class="card-dark-column">
      <div class="text-dark-blue">{{ $t('coins') }}</div>
      <h1 class="card-dark-text">{{ stickers ?? "-" }}</h1>
    </div>
    <div></div>
    <div class="right-section">
      <LanguageSwitcher />
      <button class="card-btn" @click="refresh"><img src="icon/refresh.svg" /></button>
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
  margin-left:.5em;
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
