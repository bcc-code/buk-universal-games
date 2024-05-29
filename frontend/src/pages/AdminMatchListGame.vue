<template>
  <AdminPageLayout title="Match list">
    <section class="filters">
      <div class="filter">
        <p>Zone:</p>
        <div class="single-filter">
          <AdminLeagueSelector
            v-for="league in adminLeagues"
            :class="[
               league.id === $store.state.adminLeagueSelected ? 'bg-dark-brown text-white' : 'bg-vanilla'
            ]"
            :key="league.id"
            :name="league.name"
            @click="selectLeague(league.id)"
          />
        </div>
      </div>
      <div class="filter">
        <p>Game type:</p>
        <div class="single-filter flex">
          <AdminLeagueSelector
            name="All"
            class="min-w-min whitespace-nowrap"
            :class="[
              $store.state.adminFilterGameSelected === null?'bg-dark-blue text-white': 'bg-ice-blue',
            ]"
            @click="$store.commit('resetAdminFilterGameSelected')"
          />
          <AdminLeagueSelector
            v-for="game in games"
            :key="game.id"
            class="min-w-min whitespace-nowrap"
            :class="[
              $store.state.adminFilterGameSelected === game.id?'bg-dark-blue text-white': 'bg-ice-blue',
            ]"
            :name="game.name"
            @click="$store.commit('setAdminFilterGameSelected', game.id)"
          />
        </div>
      </div>
    </section>
    <section class="space-y-5 mt-4">
      <div class = "w-full text-center mt-24" v-if="Object.keys(adminMatchGroups).length === 0">No matches for this filter</div>
      <div
        v-else
        v-for="matchGroupKey in Object.keys(adminMatchGroups)"
        :key="matchGroupKey"
      >
        <h2
          class="text-white mb-3"
          v-if="Object.keys(adminMatchGroups)[0] !== matchGroupKey"
        >
          {{ matchGroupKey }}
        </h2>
        <div v-for="match in adminMatchGroups[matchGroupKey].matches" :key="match.id">
          <MatchListItem
            :key="match.id"
            :gameType="getGameById(match.gameId)?.gameType"
            :gameAddOn="match.addOn"
            :team1="match.team1"
            :team2="match.team2"
            :team1result="match.team1Result"
            :team2result="match.team2Result"
            :start="match.start"
            :winner="match.winner"
            :clickFunc="() => matchClicked(match)"
          />
        </div>
      </div>
    </section>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import MatchListItem from '@/components/MatchListItem.vue';
import AdminLeagueSelector from '@/components/AdminLeagueSelector.vue';

export default {
  name: 'AdminMatchListGame',
  components: {
    AdminPageLayout,
    MatchListItem,
    AdminLeagueSelector,
  },
  created() {
    if (!this.$store.state.adminLeagues.length) {
      this.getAdminLeagues();
    }
    if (!this.$store.state.games.length) {
      this.getGames();
    }
    this.getMatches();
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch('getAdminLeagues');
    },
    getAdminLeagueStatus() {
      this.$store.dispatch('getAdminLeagueStatus');
    },
    async selectLeague(id) {
      await this.$store.dispatch('setAdminLeagueSelected', id);
      this.getMatches();
      this.getAdminLeagueStatus();
    },
    getMatches() {
      this.$store.dispatch('getAdminMatches');
    },
    getGames() {
      this.$store.dispatch('getGames');
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
        name: 'AdminMatch',
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

      this?.$store?.state?.adminMatches?.forEach?.((match) => {
        let shouldPush = true;

        if (
          this?.$store.state.adminFilterGameSelected !== null &&
          match.gameId !== this?.$store.state.adminFilterGameSelected
        ) {
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
  content: ' ';
  position: absolute;
  top: 50%;
  transform: translateY(-50%);
  width: 50%;
  height: 1px;
}

.user-section-single-seperator::before {
  left: -50%;
}

.user-section-single-seperator::after {
  right: -50%;
}
</style>
