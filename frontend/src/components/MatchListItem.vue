<template>
    <section class="card card-light" @click="() => clickFunc()">
        <div class="card-light-column">
            <h2 class="card-light-text">{{ game }}</h2>
        </div>
        <div class="card-light-column">
            <h2 class="card-light-text card-teams">{{ team1 }}</h2>
            <h2 class="card-light-text card-teams"> {{ team2 }} </h2>
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
    clickFunc: Function
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
        display: grid;
        grid-template-columns: 15% 75% 10%;
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

    .card-dark {
        background-color: var(--dark);
        color: white;
    }

    .card-light-text {
        font-size: .85em;
        color: inherit;
        margin: 0.35em;
    }

    .card-btn {
        border-left: 1px solid rgba(255, 255, 255, 0.3);
        border-radius: 0;
        padding: .15em .15em .15em .65em;
    }

    .card-teams {
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;
    }
</style>
