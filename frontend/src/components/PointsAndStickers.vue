<template>
    <section class="card-dark">
        <div class="card-dark-column">
            <div class="card-dark-heading">Points</div>
            <h1 class="card-dark-text">{{ points }}</h1>
        </div>
        <div class="card-dark-column">
            <div class="card-dark-heading">Stickers</div>
            <h1 class="card-dark-text"> {{ stickers }} </h1>
        </div>
        <div></div>
        <button
            class="card-btn"
            v-html="refreshIcon"
            @click="refresh"
        ></button>
    </section>
</template>

<script>
import { refreshIcon } from "@/assets/icons/refresh.svg.ts";

export default {
  name: 'PointsAndStickers',
  props: {
    points: Number,
    stickers: Number,
    refresh: Function
  },
  data() {
    return {
        refreshIcon
    }
  },
  methods: {

  }
}
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
        font-size: .85em;
    }
    
    .card-dark-text {
        color: var(--green);
    }

    .card-btn {
        border-left: 1px solid rgba(255, 255, 255, 0.3);
        border-radius: 0;
        padding: .15em .15em .15em .65em;
    }
</style>
