<template>
  <AdminPageLayout>
    <nav>
      <button @click="$router.back()">&lt;</button>
      <button
        @click="
          $router.push({
            name: 'AdminGameInfoDetail',
            params: {
              game: JSON.stringify(game),
            },
          })
        "
      >
        Game-info
      </button>
    </nav>

    <header>
      <h3>Game</h3>
      <h2>
        <span class="icon" v-html="icons[game.name]"></span>
        <span>{{ game.name }}</span>
      </h2>
    </header>

    <ul>
      <li
        :class="{
          selected: matchParsed?.team1Id === selectedTeam,
          winner: matchParsed?.team1Id === matchParsed?.winnerId,
          loser: matchParsed?.team2Id === matchParsed?.winnerId,
        }"
        v-on:click="selectedTeam = matchParsed?.team1Id"
      >
        <p>{{ matchParsed?.team1 }}</p>
        <span class="tag" v-if="matchParsed?.team1Id === matchParsed?.winnerId">Winner</span>
        <span class="tag" v-else-if="!!matchParsed?.winnerId">Loser</span>
      </li>
      <li
        :class="{
          selected: matchParsed?.team2Id === selectedTeam,
          winner: matchParsed?.team2Id === matchParsed?.winnerId,
          loser: matchParsed?.team1Id === matchParsed?.winnerId,
        }"
        v-on:click="selectedTeam = matchParsed?.team2Id"
      >
        <p>{{ matchParsed?.team2 }}</p>
        <span class="tag" v-if="matchParsed?.team2Id === matchParsed?.winnerId">Winner</span>
        <span class="tag" v-else-if="!!matchParsed?.winnerId">Loser</span>
      </li>
    </ul>

    <div class="buttons">
      <button :class="{ 'btn-blank': true, 'btn-success': !!selectedTeam }" @click="setWinner">Set winner</button>
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
  name: "LoginPage",
  props: {
    match: String,
  },
  components: { AdminPageLayout },
  data() {
    return {
      loginError: "Game Info",
      // gameParsed: {},
      game: {},
      matchParsed: {},
      loading: true,
      selectedTeam: null,
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
    if (this.match) {
      this.init(this.match);
    } else {
      this.$router.back();
    }
  },
  methods: {
    init(matchJson) {
      this.matchParsed = JSON.parse(matchJson);
      this.game = this.getGameById(this.matchParsed.gameId);
    },
    getGames() {
      this.$store.dispatch("getGames");
    },
    getGameById(id) {
      const game = this.games.find((game) => game.id == id);
      return game;
    },
    async setWinner() {
      if (this.selectedTeam) {
        const payload = { matchId: this.matchParsed.matchId, teamId: this.selectedTeam };
        await this.$store.dispatch("setWinner", payload);
        this.$store.dispatch("getAdminLeagueStatus");
        this.$store.dispatch("getAdminMatches");
        this.matchParsed.winnerId = this.selectedTeam;
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

ul {
  display: grid;
  gap: 1em;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 10em;
  list-style: none;
  margin: 0 0 2em 0;
  padding: 0;
}

ul li {
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #fff;
  border-radius: 1em;
  text-align: center;
  position: relative;
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

ul li .tag {
  position: absolute;
  bottom: 1em;
  left: 1em;
  text-transform: uppercase;
  border-radius: 4px;
  padding: 0.25em 0.5em;
}

ul li.winner .tag {
  color: #000;
  background-color: var(--green);
}

ul li.loser .tag {
  color: #000;
  background-color: var(--red);
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
