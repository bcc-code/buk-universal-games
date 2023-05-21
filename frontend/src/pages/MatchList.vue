<template>
  <UserPageLayout>
    <div v-if="matches.error">
      <h2>Something went wrong</h2>
      <div class="message">
        <p class="message-text">{{ matches.error }}</p>
        <p class="message-text">Please try refreshing the page.</p>
      </div>
    </div>
    <MatchCard :selectedMatch="selectedMatch" :game="whichGame(selectedMatch.gameId)" :games="games" />
    <section class="match-title" v-if="matches.length">
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
      <div style="height:2px;width:100%;background-color: var(--dark);margin:5px 0 10px 0;" v-if="match.gameId == currentActiveMatch.gameId"></div>
      <MatchListItem
        :class="{ 'card-dark': match.gameId == selectedMatch.gameId }"
        :game="whichGame(match.gameId)?.name"
        :gameAddOn="match.addOn"
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
      currentActiveMatch: {}
    };
  },
  created() {
    this.getMatches();
    this.getGames();
  },
  methods: {
    async getMatches() {
      await this.$store.dispatch("getMatches", false);

       let minutesBeforeNow = 20;
       let now = new Date(new Date().getTime() - (minutesBeforeNow * 60 * 1000));
       let currentTime = now.getHours() + ':' + now.getMinutes();
      //let currentTime = "12:49";

      this.initMatch(this.matches.filter((match) => match.start >= currentTime)[0] || this.matches[0]);
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
    initMatch(match) {
      this.selectedMatch = match;
      if(match != this.matches[0])
      {
        this.currentActiveMatch = match;
      }
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

.message {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1.5em;
  background-color: var(--dark);
  color: #fff;
  text-align: center;
}

.message .message-text {
  font-size: 1.5em;
}
</style>
