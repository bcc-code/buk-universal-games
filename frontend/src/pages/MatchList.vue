<template>
  <UserPageLayout>
    <div v-if="matches.error">
      <h2>Something went wrong</h2>
      <p>{{ matches.error }}</p>
      <p>Please try refreshing the page.</p>
    </div>
    <MatchCard :selectedMatch="selectedMatch" :game="whichGame(selectedMatch.gameId)" :games="games" />
    <section class="match-title">
      <div class="match-title-column">
        <h2 class="match-title-text">Game</h2>
      </div>
      <div class="match-title-column">
        <h2 class="match-title-text">Team 1 / Team 2</h2>
      </div>
      <div class="match-title-column">
        <h2 class="match-title-text">Start</h2>
      </div>
    </section>
    <section class="user-section" v-for="match in matches" :key="match.id">
      <MatchListItem
        :class="{ 'card-dark': match.gameId == selectedMatch.gameId }"
        :game="whichGame(match.gameId)?.name"
        :team1="match.team1"
        :team2="match.team2"
        :start="match.start"
        :winner="match.winner"
        :clickFunc="() => matchClicked(match)"
      />
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "../components/UserPageLayout.vue";
import MatchListItem from "../components/MatchListItem.vue";
import MatchCard from "../components/MatchCard.vue";

export default {
  name: "MatchList",
  props: {
    data: String,
  },
  components: { UserPageLayout, MatchListItem, MatchCard },
  data() {
    return {
      loginError: "Match List",
      selectedMatch: {},
    };
  },
  created() {
    this.getMatches();
    this.getGames();
  },
  methods: {
    getMatches() {
      this.$store.dispatch("getMatches", false);
    },
    getGames() {
      this.$store.dispatch("getGames");
    },
    whichGame(id) {
      if (this.games.error) {
        return {};
      }

      let game = this.games.find((game) => game.id == id);
      return game;
    },
    matchClicked(match) {
      this.selectedMatch = match;
    },
  },
  computed: {
    matches() {
      return this?.$store.state.matches;
    },
    games() {
      return this?.$store.state.games;
    },
  },
};
</script>

<style scoped>
.match-title {
  padding: 0 0.25em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 20% 65% 15%;
}

.match-title-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.match-title-text {
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}
</style>
