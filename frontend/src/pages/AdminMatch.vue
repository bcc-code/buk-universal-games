<template>
  <AdminPageLayout>
    <nav>
      <button @click="$router.back()">&lt; Back</button>
      <button @click="this.showGameInfo(game)">Game-info</button>
    </nav>

    <header>
      <h3><span class="icon" v-html="icons[game.name]"></span>
        <span>{{ game.name }}</span></h3>
      <h2>
        <span>Start: {{ match.start }}</span>
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
        <input type="number" v-model="team1Result" :placeholder="'Result (in ' + units[game.gameType] + ')'" />
        <button class="btn btn-blank" @click="confirmTeamResult(match?.team1Id, team1Result)">Confirm</button>
        <span class="tag" v-if="match?.team1Id === match?.winnerId">Winner</span>
      </div>
      <div
        :class="{
          teamresult: true,
          selected: match?.team2Id === selectedTeam,
          winner: match?.team2Id === match?.winnerId,
          loser: match?.team1Id === match?.winnerId,
        }"
      >
        <p>{{ match?.team2 }}</p>
        <input type="number" v-model="team2Result" :placeholder="'Result (in ' + units[game.gameType] + ')'" />
        <button class="btn btn-blank" @click="confirmTeamResult(match?.team2Id, team2Result)">Confirm</button>
        <span class="tag" v-if="match?.team2Id === match?.winnerId">Winner</span>
      </div>
    </div>

    <div class="buttons">
      <!-- <button :class="{ 'btn-blank': true, 'btn-success': !!selectedTeam }" @click="setWinner">Set winner</button> -->
      <!-- <button class="btn btn-blank">Loser</button> -->
    </div>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
// import { getData } from "@/libs/apiHelper";
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "AdminMatch",
  props: {
    matchId: String,
  },
  components: { AdminPageLayout },
  data() {
    return {
      loginError: "Game Info",
      game: {},
      match: {},
      team1Result: {},
      team2Result: {},
      canEnterResults: false,
      loading: true,
      selectedTeam: null,
      units: {
        NerveSpiral: "seconds",
        MineField: "seconds",
        MonkeyBars: "bars completed",
        TableSurfing: "meters",
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
      this.getGames();
    }
  },
  mounted() {
    if (this.matchId) {
      this.init(this.matchId);
    } else {
      this.$router.back();
    }
  },
  methods: {
    init(matchId) {
      this.match = this.$store.state.adminMatches.find((match) => match.matchId == matchId);
      this.game = this.getGameById(this.match.gameId);
    },
    getGames() {
      this.$store.dispatch("getGames");
    },
    getGameById(id) {
      if (this.games.error) {
        return {};
      }

      const game = this.games.find((game) => game.id == id);
      return game;
    },
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
        this.$store.dispatch("getAdminLeagueStatus");
        this.$store.dispatch("getAdminMatches");
        this.match.team1Result = this.team1Result;
        this.match.team2Result = this.team2Result;
      } else {
        alert("Please enter a result");
      }
    },
    async setWinner() {
      if (this.selectedTeam) {
        const payload = { matchId: this.match.matchId, teamId: this.selectedTeam };
        await this.$store.dispatch("setWinner", payload);
        this.$store.dispatch("getAdminLeagueStatus");
        this.$store.dispatch("getAdminMatches");
        this.match.winnerId = this.selectedTeam;
        this.selectedTeam = null;
      } else {
        alert("Please select a team");
      }
    },
  },
  computed: {
    games() {
      return this?.$store.state.games;
    },
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
  grid-template-columns: repeat(auto-fill, minmax(1fr, 2fr));
  grid-template-rows: 8em 8em;
  list-style: none;
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
