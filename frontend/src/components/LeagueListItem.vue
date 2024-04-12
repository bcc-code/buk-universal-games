<template>
  <section
    class="flex align-middle items-center"
    :class="{ card: true, 'text-label-1': true, loading }"
  >
    <img v-if="index <= 3" class="w-5" :src="`image/${index}_place.png`" />

    <p v-if="index > 3" class="text-label-1">{{ index }}</p>
    <div class="flex my-5">
      <p class="text-label-1 hide-text-overflow">{{ team }}</p>
    </div>
    <p class="text-label-1">{{ points }}</p>
  </section>
</template>

<script>
export default {
  name: 'LeagueListItem',
  props: {
    index: Number,
    team: String,
    points: Number,
    stickers: Number,
    loading: {
      required: false,
      type: Boolean,
      default: false,
    },
  },
};
</script>

<style scoped>
button {
  background-color: inherit;
}

.card {
  padding: 0 1em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 10% 75% 15%;
}

.card-row {
  display: flex;
  align-items: center;
  width: 100%;
}

.card-dark {
  background-color: var(--label-1);
  color: white;
}

.card-light {
  background-color: white;
  color: var(--label-1);
}

.card-dark-column:nth-child(-n + 3) {
  align-items: flex-start;
}

.card-dark-column:nth-last-child(-n + 1) {
  align-items: flex-end;
}

.card-dark-column:nth-last-child(-n + 2) {
  align-items: flex-end;
}

.card-dark-text {
  font-size: 0.85em;
  color: inherit;
  white-space: nowrap;
}

.hide-text-overflow {
  text-overflow: ellipsis;
  overflow: hidden;
}

.card-btn {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 0;
  padding: 0.15em 0.15em 0.15em 0.65em;
}

.buk-icon {
  margin-right: 0.75em;
}

.loading {
  background-repeat: no-repeat;
  background-size: 24em 100%;
  background-image: linear-gradient(
    to right,
    transparent 0%,
    hsl(323, 50%, 33%) 50%,
    transparent 100%
  );
  animation-duration: 1000ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

.loading .card-dark-text,
.loading .buk-icon {
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
