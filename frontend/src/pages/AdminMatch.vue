<template>
  <AdminPageLayout>
    <nav>
      <button @click="$router.back()">&lt; Back</button>
      <button @click="this.showGameInfo(game)">Game-info</button>
    </nav>

    <section class="error-popup" v-if="showErrorPopup">
      <p><span>{{ popupErrorMessage }}</span></p>
    </section>

    <header>
      <h3>
        <img class="icon" :src="`icon/game-${game.gameType}.svg`" />
        <span>{{ $t("games." + game.gameType) }}</span></h3>
      <h2>
        <span>Start: {{ match?.start }}</span>
      </h2>
    </header>

    <div class="teams">
      <div
        :class="{
          teamresult: true,
          selected: match?.team1Id === selectedTeam,
          winner: match?.team1Id === match?.winnerId,
          loser: match?.team2Id === match?.winnerId,
        }"
      >
        <p>{{ match?.team1 }}</p>
        <p v-if="match?.team1Result"><strong>Score:</strong> {{ match?.team1Result }}</p>

        <button class="btn btn-blank" v-if="!isChangingScore1 && match?.team1Result > 0" @click="isChangingScore1 = true">Change</button>

        <div v-if="isChangingScore1 || !match?.team1Result">
        <TableSurfingInput v-if="game?.gameType === 'tablesurfing'" v-model="team1Result" />
        <TimeInput v-else-if="['minefield','nervespiral'].includes(game?.gameType)" v-model="team1Result" />
        <MonkeyBarsInput v-else-if="game?.gameType === 'monkeybars'" v-model="team1Result" />
        <input v-else type="number" v-model="team1Result" :placeholder="'Result (in ' + units[game?.gameType] + ')'" />
        <button class="btn btn-blank" @click="confirmTeamResult(match?.team1Id, team1Result)">Confirm</button>
        </div>
        <span class="tag" v-if="match?.team1Id === match?.winnerId">Winner</span>
      </div>
      <div v-if="match?.team1Id !== match?.team2Id"
        :class="{
          teamresult: true,
          selected: match?.team2Id === selectedTeam,
          winner: match?.team2Id === match?.winnerId,
          loser: match?.team1Id === match?.winnerId,
        }"
      >
        <p>{{ match?.team2 }}</p>
        <p v-if="match?.team2Result"><strong>Score:</strong> {{ match?.team2Result }}</p>

        <button class="btn btn-blank" v-if="!isChangingScore2 && match?.team2Result > 0" @click="isChangingScore2 = true">Change</button>
        <div v-if="isChangingScore2 || !match?.team2Result">
        <TableSurfingInput v-if="game?.gameType === 'tablesurfing'" v-model="team2Result" />
        <TimeInput v-else-if="['minefield','nervespiral'].includes(game?.gameType)" v-model="team2Result" />
        <MonkeyBarsInput v-else-if="game?.gameType === 'monkeybars'" v-model="team2Result" />
        <input v-else type="number" v-model="team2Result" :placeholder="'Result (in ' + units[game.gameType] + ')'" />
        <button class="btn btn-blank" @click="confirmTeamResult(match?.team2Id, team2Result)">Confirm</button>
        </div>
        <span class="tag" v-if="match?.team2Id === match?.winnerId">Winner</span>
      </div>
    </div>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import TableSurfingInput from '@/components/TableSurfingInput.vue';
import TimeInput from "@/components/TimeInput.vue";
import MonkeyBarsInput from "@/components/MonkeyBarsInput.vue";

export default {
  name: "AdminMatch",
  props: {
    matchId: String,
  },
  components: {
    AdminPageLayout,
    TableSurfingInput,
    TimeInput,
    MonkeyBarsInput
},
  data() {
    return {
      loginError: "Game Info",
      team1Result: null,
      team2Result: null,
      canEnterResults: false,
      loading: true,
      isChangingScore1: false,
      isChangingScore2: false,
      selectedTeam: null,
      showErrorPopup: false,
      popupErrorMessage: null,
      match: null,
      game: null,
      units: {
        nervespiral: "seconds",
        minefield: "seconds",
        monkeybars: "bars completed",
        tablesurfing: "seconds",
        tickettwist: "nr of tickets",
      },
    };
  },
  created() {
    if (!this.$store.state.games.length) {
      this.$store.dispatch("getGames");
    }
    if(!this?.$store?.state?.adminMatches?.length) {
        this.$store.dispatch("getAdminMatches");
    }
    if(this.matchId) {
      this.loadMatch();
    }
  },
  mounted() {
    if (!this.matchId) {
      this.$router.back();
    }
  },
  methods: {
    showGameInfo(game) {
      this.$router.push({
            name: 'AdminGameInfoDetail',
            params: {
              game: game.id,
            },
          })
    },
    loadMatch()
    {
      this.match = this.matches?.find((match) => match.matchId == this.matchId);
      this.game = this.games?.find((game) => game.id == this.match?.gameId);
    },
    async confirmTeamResult(teamId, result) {
      if (result) {
        const payload = { matchId: this.match.matchId, teamId, result };
        var response = await this.$store.dispatch("confirmTeamResult", payload);
        console.log(response, response === "failed");
        if(response === "failed") {
          this.popupErrorMessage = "Submitting failed, please check connection and try again";
          this.showErrorPopup = true;
          setTimeout(() => {
            this.showErrorPopup = false;
            this.popupErrorMessage = null;
          }, 5000);
          return;
        }
        this.$forceUpdate();
        this.$store.dispatch("getAdminLeagueStatus");
        if(teamId === this.match.team1Id) {
          this.match.team1Result = this.team1Result;
          this.isChangingScore1 = false;
        }
        else {
          this.match.team2Result = this.team2Result;
          this.isChangingScore2 = false;
        }
        this.loadMatch();
      } else {
        alert("Please enter a result");
      }
    }
  },
  computed: {
    matches() {
      return this.$store.state.adminMatches;
    },
    games() {
      return this.$store.state.games;
    }
  },
};
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  height: 20em;
  border-radius: 1em;
  margin: 2em auto;
  background-color: var(--dark);
  color: #fff;
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  background-size: 80%;
  background-position: center;
  background-repeat: no-repeat;
}

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
}
header h3 {
  display: flex;
  align-items: center;
  gap: 0.5em;
  margin: .5em 0;
}

.icon {
  max-width: 4em;
}

div.teams {
  display: grid;
  gap: 1em;

  margin: 0 0 2em 0;
  padding: 0;
}

div.teamresult {
  padding: 1em;
  background-color: #fff;
  border-radius: 1em;
  position: relative;
  display:grid;
  grid-template-rows: auto auto auto;
  box-shadow: 0 0.5em 1em -0.5em rgba(0, 0, 0, 0.1);
}

ul li.winner {
  /* color: #fff;
  background-color: var(--dark); */
}

ul li.selected {
  color: #fff;
  background-color: var(--dark);
  /* background-color: #fff; */
}

/* ul li.winner.selected {

} */

div.teamresult .tag {
  position: absolute;
  top: 1em;
  right: 1em;
  text-transform: uppercase;
  border-radius: 4px;
  padding: 0.25em 0.5em;
}


div.teamresult .tag {
  color: #000;
  background-color: var(--red);
}

div.teamresult.winner .tag {
  color: #000;
  background-color: var(--green);
}

.buttons {
  display: flex;
  flex-direction: column;
  gap: 1em;
}

.buttons button {
  width: 100%;
}

.btn-success {
  border: 2px solid hsl(158, 93%, 40%);
}

header h2 {
  display: flex;
  align-items: center;
  gap: 0.5em;
}
.error-popup {
  position: fixed;
  top: 4em;
  left:1em;
  right:1em;
  z-index:800;
  background-color: var(--red);
  color: #fff;
  padding:1.5em 1em;
  border-radius:1em;
  box-shadow: 2px 5px 5px 0px rgba(0,0,0,0.5);
}
</style>
