<template>
  <AdminPageLayout title="Match list">
    <section class="filters">
      <div class="filter">
        <p>League:</p>
        <div class="single-filter">
          <AdminLeagueSelector
            v-for="league in adminLeagues"
            :class="{ 'green-font': league.id === $store.state.adminLeagueSelected }"
            :key="league.id"
            :name="league.name"
            @click="selectLeague(league.id)"
          />
        </div>
      </div>
      <div class="filter">
        <p>Game type:</p>
        <div class="single-filter">
          <AdminLeagueSelector
            name="All"
            :class="{ 'green-font': $store.state.adminFilterGameSelected === null }"
            @click="$store.commit('resetAdminFilterGameSelected')"
          />
          <AdminLeagueSelector
            v-for="game in games"
            :key="game.id"
            :class="{ 'green-font': game.id === $store.state.adminFilterGameSelected }"
            :name="game.name"
            @click="$store.commit('setAdminFilterGameSelected', game.id)"
          />
        </div>
      </div>
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
    <section class="user-section">
      <div class="user-section-single" v-for="matchGroupKey in Object.keys(adminMatchGroups)" :key="matchGroupKey">
        <p class="user-section-single-seperator" v-if="Object.keys(adminMatchGroups)[0] !== matchGroupKey">{{ matchGroupKey }}</p>
        <MatchListItem
          v-for="match in adminMatchGroups[matchGroupKey].matches"
          :key="match.id"
          :game="getGameById(match.gameId)?.name"
          :gameAddOn="match.addOn"
          :team1="match.team1"
          :team2="match.team2"
          :start="match.start"
          :winner="match.winner"
          :clickFunc="() => matchClicked(match)"
        />
      </div>
    </section>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import MatchListItem from "@/components/MatchListItem.vue";
import AdminLeagueSelector from "@/components/AdminLeagueSelector.vue";

export default {
  name: "AdminMatchListGame",
  components: {
    AdminPageLayout,
    MatchListItem,
    AdminLeagueSelector,
  },
  created() {
    if (!this.$store.state.adminLeagues.length) {
      this.getAdminLeagues();
    }
    if (!this.$store.state.adminMatches.length) {
      this.getMatches();
    }
    if (!this.$store.state.games.length) {
      this.getGames();
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch("getAdminLeagues");
    },
    getAdminLeagueStatus() {
      this.$store.dispatch("getAdminLeagueStatus");
    },
    async selectLeague(id) {
      await this.$store.dispatch("setAdminLeagueSelected", id);
      this.getMatches();
      this.getAdminLeagueStatus();
    },
    getMatches() {
      this.$store.dispatch("getAdminMatches");
    },
    getGames() {
      this.$store.dispatch("getGames");
    },
    getGameById(id) {
      if (this.games.error) {
        return {};
      }

      let game = this.games.find((game) => game.id == id);
      return game;
    },
    matchClicked(match) {
      this.$router.push({
        name: "AdminMatch",
        params: {
          matchId: match.matchId.toString(),
        },
      });
    },
  },
  computed: {
    adminLeagues() {
      return this?.$store.state.adminLeagues;
    },
    adminMatchGroups() {
      const matchGroups = [];

      this?.$store.state.adminMatches.forEach((match) => {
        let shouldPush = true;

        if (this?.$store.state.adminFilterGameSelected !== null && match.gameId !== this?.$store.state.adminFilterGameSelected) {
          shouldPush = false;
        }

        if (shouldPush) {
          if (matchGroups[match.start]) {
            shouldPush && matchGroups[match.start].matches.push(match);
          } else {
            matchGroups[match.start] = {
              id: match.start,
              matches: [match],
            };
          }
        }
      });

      return matchGroups;
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

.filters {
  display: flex;
  width: 100%;
  flex-direction: column;
  gap: 0.5em;
}

.filters .filter {
  display: flex;
  flex-direction: column;
}

/* .filters > p {
  white-space: nowrap;
}
*/

.single-filter {
  display: flex;
  align-items: center;
  /* background-color: red; */
  width: 100%;
  overflow: auto;
}

.green-font {
  color: var(--green);
}

.user-section,
.user-section-single {
  display: flex;
  flex-direction: column;
}

.user-section {
  gap: 4em;
  padding-bottom: 10em;
}

.user-section-single {
  gap: 1em;
}

.user-section-single-seperator {
  text-align: center;
  margin: auto;
  padding: 1rem;
  font-weight: 700;
  font-size: 2em;
  position: relative;
}

.user-section-single-seperator::before,
.user-section-single-seperator::after {
  content: " ";
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  width: 50%;
  height: 1px;
  background-color: var(--gray-2);
}

.user-section-single-seperator::before {
  left: -50%;
}

.user-section-single-seperator::after {
  right: -50%;
}
</style>
