<template>
  <section class="card card-light" @click="() => clickFunc()">
    <div class="card-light-column">
      <h2 class="card-light-text">{{ game }}</h2>
    </div>
    <div class="card-light-column">
      <h2 :class="{ 'card-light-text': true, 'card-teams': true, winner: team1 === winner, loser: team2 === winner }">
        <span>{{ team1 }}</span>
      </h2>
      <h2 :class="{ 'card-light-text': true, 'card-teams': true, winner: team2 === winner, loser: team1 === winner }">
        <span>{{ team2 }}</span>
      </h2>
    </div>
    <div class="card-light-column">
      <h2 class="card-light-text">{{ start }}</h2>
    </div>
  </section>
</template>

<script>
export default {
  name: "MatchListItem",
  props: {
    game: String,
    team1: String,
    team2: String,
    winner: String,
    start: String,
    clickFunc: Function,
  },
};
</script>

<style scoped>
button {
  background-color: inherit;
}

.card {
  padding: 0.25em 0.5em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 20% 65% 15%;
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
  font-size: 0.85em;
  color: inherit;
  margin: 0.35em;
}

.card-btn {
  border-left: 1px solid rgba(255, 255, 255, 0.3);
  border-radius: 0;
  padding: 0.15em 0.15em 0.15em 0.65em;
}

.card-teams {
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
  position: relative;
  padding-left: 1.5em;
}

.card-teams::after {
  content: " ";
  position: absolute;
  top: 50%;
  left: 0;
  transform: translateY(-50%);
  background-color: black;
  width: 1em;
  height: 1em;
  border-radius: 100%;
}
.card-teams.winner::after {
  background-color: var(--green);
}

.card-teams.loser::after {
  background-color: var(--red);
}
</style>
