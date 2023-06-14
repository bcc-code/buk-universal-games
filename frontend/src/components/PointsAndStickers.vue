<template>
  <section :class="{ 'card-dark': true, loading }">
    <div class="card-dark-column">
      <div class="card-dark-heading">Points</div>
      <h1 class="card-dark-text">{{ points }}</h1>
    </div>
    <div class="card-dark-column" v-if="false">
      <div class="card-dark-heading">Stickers</div>
      <h1 class="card-dark-text">{{ stickers }}</h1>
    </div>
    <div></div>
    <button class="card-btn" v-html="refreshIcon" @click="refresh"></button>
  </section>
</template>

<script>
import { refreshIcon } from "@/assets/icons/refresh.svg.ts";

export default {
  name: "PointsAndStickers",
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
  data() {
    return {
      refreshIcon,
    };
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
  padding: 1.5em;
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
  border-left: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 0;
  padding: 0.15em 0.15em 0.15em 0.65em;
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
