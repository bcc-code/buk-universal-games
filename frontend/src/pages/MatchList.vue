<template>
  <UserPageLayout>
    <MatchCard :selectedMatch="selectedMatch" :game="whichGame(selectedMatch.gameId)" :games="games"/>
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
    <section class="user-section" v-for="(match) in matches" :key="match.id">
      <MatchListItem :class="{'card-dark' : match.gameId == selectedMatch.gameId}" :game="whichGame(match.gameId)?.name" :team1="match.team1" :team2="match.team2" :start="match.start" :clickFunc="() => matchClicked(match)" />
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
      selectedMatch: {}
    };
  },
  created() {
    if(!this.$store.state.matches.length) {
       this.getMatches() 
    }

    if(!this.$store.state.games.length) {
       this.getGames() 
    }
  },
  methods: {
    getMatches() {
      this.$store.dispatch("getMatches")
    },
    getGames() {
      this.$store.dispatch("getGames")
    },
    whichGame(id) {
      let game = this.games.find(game => game.id == id)
      return game
    },
    matchClicked(match) {
      this.selectedMatch = match;
    }
  },
  computed: {
    matches() {
      return this?.$store.state.matches
    },
    games() {
      return this?.$store.state.games
    },
  },
};
</script>

<style scoped>
  .match-title {
      padding: 0 1em;
      border-radius: .75em;
      display: grid;
      grid-template-columns: 15% 75% 10%;
  }

  .match-title-column {
      display: flex;
      flex-direction: column;
      width: 100%;
  }

  .match-title-text {
      font-size: .85em;
      color: var(--gray-2);
      margin: 1em 0 0;
  }
</style>
