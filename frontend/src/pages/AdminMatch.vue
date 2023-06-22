<template>
  <AdminPageLayout>
    <nav>
      <button @click="$router.back()">&lt; Back</button>
      <button @click="this.showGameInfo(game)">Game-info</button>
    </nav>

    <header>
      <h3><span class="icon" v-html="icons[game?.name]"></span>
        <span>{{ game?.name }}</span></h3>
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
        <p><strong>Score ({{ units[game?.gameType]}}):</strong> {{ match?.team1Result }}</p>

        <button class="btn btn-blank" v-if="!isChangingScore1 && match?.team1Result > 0" @click="isChangingScore1 = true">Change</button>

        <div v-if="isChangingScore1 || !match?.team1Result">
        <TableSurfingInput v-if="game?.gameType === 'TableSurfing'" v-model="team1Result" />
        <TimeInput v-else-if="['MineField','NerveSpiral'].includes(game?.gameType)" v-model="team1Result" />
        <MonkeyBarsInput v-else-if="game?.gameType === 'MonkeyBars'" v-model="team1Result" />
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
        <p><strong>Score ({{ units[game?.gameType]}}):</strong> {{ match?.team2Result }}</p>

        <button class="btn btn-blank" v-if="!isChangingScore2 && match?.team2Result > 0" @click="isChangingScore2 = true">Change</button>
        <div v-if="isChangingScore2 || !match?.team2Result">
        <TableSurfingInput v-if="game?.gameType === 'TableSurfing'" v-model="team2Result" />
        <TimeInput v-else-if="['MineField','NerveSpiral'].includes(game?.gameType)" v-model="team2Result" />
        <MonkeyBarsInput v-else-if="game?.gameType === 'MonkeyBars'" v-model="team2Result" />
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
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";
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
      units: {
        NerveSpiral: "seconds",
        MineField: "seconds",
        MonkeyBars: "bars completed",
        TableSurfing: "seconds",
        TicketTwist: "nr of tickets",
      },
      icons: {
        Earth: gameEarthIcon,
        Fire: gameFireIcon,
        Metal: gameMetalIcon,
        Wood: gameWoodIcon,
        Water: gameWaterIcon,
      },
    };
  },
  created() {
    if (!this.$store.state.games.length) {
      this.$store.dispatch("getGames");
    }
    if(!this.$store.state.adminMatches.length) {
        this.$store.dispatch("getAdminMatches");
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
    async confirmTeamResult(teamId, result) {
      if (result) {
        const payload = { matchId: this.match.matchId, teamId, result };
        await this.$store.dispatch("confirmTeamResult", payload);
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
      } else {
        alert("Please enter a result");
      }
    }
  },
  computed: {
    matches() {
      return this.$store.state.adminMatches;
    },
    match() {
      return this.matches?.find((match) => match.matchId == this.matchId);
    },
    games() {
      return this.$store.state.games;
    },
    game() {
      return this.games?.find((game) => game.id == this.match?.gameId);
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
  align-items: end;
  background-size: 80%;
  background-position: center;
  background-repeat: no-repeat;
}

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
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
  grid-template-rows: 1fr 1fr 1fr;
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
</style>
