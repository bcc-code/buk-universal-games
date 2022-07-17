<template>
    <section v-if="selectedMatch.team1" class="card card-dark">
        <section>
            <div class="card-dark-column">
                <h1 class="card-dark-text">{{ game.name }}</h1>
            </div>
        </section>
        <section class="card-row">
            <div class="card-dark-column">
                <h2 class="card-dark-text">{{ selectedMatch?.team1 }}</h2>
                <div v-if="selectedMatch.winner">
                    <h2 v-if="selectedMatch.winnerId == selectedMatch.team1Id" class="card-dark-text winner">{{ game.winnerPoints }}</h2>
                    <h2 v-else class="card-dark-text">{{ game.looserPoints }}</h2>
                </div>
            </div>
            <div class="vl"></div>
            <div class="card-dark-column">
                <h2 class="card-dark-text"> {{ selectedMatch?.team2 }} </h2>
                <div v-if="selectedMatch.winner">
                    <h2 v-if="selectedMatch.winnerId == selectedMatch.team2Id" class="card-dark-text winner">{{ game.winnerPoints }}</h2>
                    <h2 v-else class="card-dark-text">{{ game.looserPoints }}</h2>
                </div>
            </div>
        </section>
    </section>
</template>

<script>

export default {
  name: 'MatchCard',
  props: {
    selectedMatch: Object,
    game: Object,
  },
}
</script>

<style scoped>
    h1 {
        font-weight: 500;
    }

    h2 {
        font-weight: normal;
        margin: 0;
    }

    button {
        background-color: inherit;
    }

    .card {
        padding: 1em;
        border-radius: .75em;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }
    
    .card-row {
        display: flex;
        justify-content: center;
        align-items: center;
        width: 100%;
    }
    
    .card-dark {
        background-color: var(--dark);
        color: white;
    }

    .card-dark-column {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        width: 100%;
    }

    .card-dark-text {
        font-size: 1.25em;
        color: inherit;
        text-overflow: ellipsis;
        overflow: hidden;
        white-space: nowrap;
    }

    .vl {
        border-left: 2px solid var(--green);
        height: 80px;
        margin: .5em 0;
    }

    .winner {
        color: var(--green);
    }
</style>
