<template>
    <section class="card card-light">
        <div class="card-light-column">
            <h2 class="card-light-text">{{ game }}</h2>
        </div>
        <div class="card-light-column">
            <h2 class="card-light-text">{{ team1 }}</h2>
        </div>
        <div class="card-light-column">
            <h2 class="card-light-text"> {{ team2 }} </h2>
        </div>
        <div class="card-light-column">
            <h2 class="card-light-text"> {{ start }} </h2>
        </div>
    </section>
</template>

<script>
export default {
  name: 'MatchListItem',
  props: {
    game: String,
    team1: String,
    team2: String,
    start: String,
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


    .card-light {
        background-color: white;
        color: var(--dark);
    }

    .card-light-column {
        display: flex;
        flex-direction: column;
        width: 100%;
    }


    .card-light-text {
        font-size: .85em;
        color: inherit;
    }

    .card-btn {
        border-left: 1px solid rgba(255, 255, 255, 0.3);
        border-radius: 0;
        padding: .15em .15em .15em .65em;
    }
</style>
