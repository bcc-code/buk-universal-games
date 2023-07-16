<template>
  <section :class="{ card: true, 'card-dark': true, loading }">
    <div class="card-dark-column index-column">
      <h2 class="card-dark-text">{{ index }}</h2>
    </div>
    <div class="card-dark-column">
      <div class="card-row">
        <span class="buk-icon"><img src="icon/buk-icon.svg" /></span>
        <h2 class="card-dark-text hide-text-overflow">{{ team }}</h2>
      </div>
    </div>
    <div class="card-dark-column">
      <h2 class="card-dark-text">{{ points }}</h2>
    </div>
  </section>
</template>

<script>
export default {
  name: "LeagueListItem",
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
  background-color: var(--dark);
  color: white;
}

.card-light {
  background-color: white;
  color: var(--dark);
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
  background-image: linear-gradient(to right, transparent 0%, hsl(323, 50%, 33%) 50%, transparent 100%);
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
