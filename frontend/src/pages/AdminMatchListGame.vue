<template>
  <AdminPageLayout title="Match list">
    <section class="leagues">
      <AdminLeagueCard v-for="league in adminLeagues" :class="{'green-font' : league.id == $store.state.adminLeagueSelected}" :key="league.id" :name="league.name" @click="selectLeague(league.id)"/>
    </section>
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
    <section class="user-section" v-for="(match) in adminMatches" :key="match.id">
      <MatchListItem :game="whichGame(match.gameId)?.name" :team1="match.team1" :team2="match.team2" :start="match.start" :clickFunc="() => matchClicked(match)" />
    </section>  
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import MatchListItem from "@/components/MatchListItem.vue";
import AdminLeagueCard from "@/components/AdminLeagueCard.vue";

export default {
  name: "AdminMatchListGame",
  components: { AdminPageLayout, MatchListItem, AdminLeagueCard },
  created() {
    if(!this.$store.state.adminLeagues.length) {
       this.getAdminLeagues() 
    }
    if(!this.$store.state.adminMatches.length) {
       this.getMatches() 
    }
    if(!this.$store.state.games.length) {
       this.getGames() 
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch("getAdminLeagues")
    },
    getAdminLeagueStatus() {
      this.$store.dispatch("getAdminLeagueStatus")
    },
    async selectLeague(id) {
      await this.$store.dispatch("setAdminLeagueSelected", id)
      this.getMatches()
      this.getAdminLeagueStatus()
    },
    getMatches() {
      this.$store.dispatch("getAdminMatches")
    },
    getGames() {
      this.$store.dispatch("getGames")
    },
    whichGame(id) {
      let game = this.games.find(game => game.id == id)
      return game
    },
    matchClicked(match) {
      this.$router.push({name: 'AdminGame', params: {
          match: JSON.stringify(match)
        }
      })
    }
  },
  computed: {
    adminLeagues() {
      return this?.$store.state.adminLeagues
    },
    adminMatches() {
      return this?.$store.state.adminMatches
    },
    games() {
      return this?.$store.state.games
    },
  }
};
</script>

<style scoped>
  .match-title {
      padding: 0 .25em;
      border-radius: .75em;
      display: grid;
      grid-template-columns: 20% 65% 15%;
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

  .leagues {
    display: flex;
    width: 100%;
  }

  .green-font {
    color: var(--green);
  }
</style>
