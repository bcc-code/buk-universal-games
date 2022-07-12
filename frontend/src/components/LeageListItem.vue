<template>
    <section class="card card-dark">
        <div class="card-dark-column index">
            <h2 class="card-dark-text">{{ index }}</h2>
        </div>
        <div class="card-dark-column">
            <div class="card-row">
                <span class="buk-icon" v-html="bukIcon"></span>
                <h2 class="card-dark-text">{{ team }}</h2>
            </div>
        </div>
        <div class="card-dark-column">
            <h2 class="card-dark-text"> {{ stickers }} </h2>
        </div>
        <div class="card-dark-column">
            <h2 class="card-dark-text"> {{ points }} </h2>
        </div>
    </section>
</template>

<script>
import { bukIcon } from "@/assets/icons/buk-icon.svg.ts";

export default {
  name: 'LeageListItem',
  props: {
    index: Number,
    team: String,
    points: Number,
    stickers: Number,
  },
    data() {
        return {
            bukIcon
        }
  },
}
</script>

<style scoped>
    button {
        background-color: inherit;
    }

    .card {
        padding: 0 1em;
        border-radius: .75em;
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;
    }

    .card-row {
        display: flex;
        align-items: center;
    }
    
    .card-dark {
        background-color: var(--dark);
        color: white;
    }

    .card-light {
        background-color: white;
        color: var(--dark);
    }

    .card-dark-column {
        display: flex;
        flex-direction: column;
        width: 100%;
    }

    .card-dark-column:nth-child(-n + 2) {
        align-items: flex-start;
    }

    .card-dark-column:nth-last-child(-n + 2) {
        align-items: flex-end;
    }

    .card-dark-text {
        font-size: .85em;
        color: inherit;
    }

    .card-btn {
        border-left: 1px solid rgba(255, 255, 255, 0.3);
        border-radius: 0;
        padding: .15em .15em .15em .65em;
    }

    .buk-icon {
        margin-right: .75em;
    }

    .index {
        width: 25%;
    }
</style>
