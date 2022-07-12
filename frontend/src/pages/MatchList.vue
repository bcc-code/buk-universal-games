<template>
  <UserPageLayout>
    <section class="match-title">
        <div class="match-title-column">
            <h2 class="match-title-text">Game</h2>
        </div>
        <div class="match-title-column">
            <h2 class="match-title-text">Team 1</h2>
        </div>
        <div class="match-title-column">
            <h2 class="match-title-text">Team 2</h2>
        </div>
        <div class="match-title-column">
            <h2 class="match-title-text">Start</h2>
        </div>
    </section>
    <section class="user-section" v-for="(match) in matches" :key="match.id">
      <MatchListItem :game="whichGame(match.gameId)" :team1="match.team1" :team2="match.team2" :start="match.start " />
    </section>  
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "../components/UserPageLayout.vue";
import MatchListItem from "../components/MatchListItem.vue";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout, MatchListItem },
  data() {
    return {
      loginError: "Match List",
    };
  },
  created() {
    if(Object.keys(this.$store.state.matches).length === 0) {
       this.getMatches() 
    }

    if(Object.keys(this.$store.state.games).length === 0) {
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
      return game?.name
    }
  },
  computed: {
    matches() {
      return this?.$store.state.matches
    },
    games() {
      return this?.$store.state.games
    },
  }
};
</script>

<style scoped>
  .match-title {
      padding: 0 1em;
      border-radius: .75em;
      display: flex;
      flex-direction: row;
      justify-content: space-between;
      align-items: center;
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
