<template>
  <UserPageLayout>
    <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => refreshPoints()"/>

    <h2>Games</h2>

    <ul v-if="loading" class="games games-loading">
      <li v-for="i in [0, 1, 2, 3, 4]" :key="i" class="game">
        <span class="game-icon"></span>
        <h3 class="game-title">Loading</h3>
      </li>
    </ul>
    <ul v-else class="games">
      <li
        class="game"
        v-for="game in games"
        :key="game.id"
        @click="
          $router.push({
            name: 'GameInfoDetail',
            params: {
              game: JSON.stringify(game),
            },
          })
        "
      >
        <span class="game-icon" v-html="icons[game.name]"></span>
        <h3 class="game-title">{{ game.name }}</h3>
      </li>
    </ul>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";
import { getData } from "@/libs/apiHelper";
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout, PointsAndStickers },
  data() {
    return {
      loginError: "Game Info",
      games: [],
      loading: true,
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
    if(Object.keys(this.$store.state.teamStatus).length === 0) {
       this.refreshPoints() 
    }
  },
  mounted() {
    // Only get live data every 30 seconds
    if (this.checkSavedGamesAge() === null || this.checkSavedGamesAge() > 30) {
      this.getLiveGames();
    } else {
      this.getSavedGames(true);
    }
  },
  methods: {
    getLiveGames() {
      getData("/Games")
        .then((r) => r.json())
        .then((r) => {
          if (r.error) {
            throw r.error;
          }

          this.games = r;
          this.loading = false;
          this.saveGames(r);
        })
        .catch((e) => {
          console.error(e);
          this.getSavedGames(true);
        });
    },
    saveGames(data) {
      window.localStorage.setItem(
        "gamesData",
        JSON.stringify({
          timestamp: new Date().getTime(),
          data,
        })
      );
    },
    getSavedGames(use) {
      let savedGames = window.localStorage.getItem("gamesData");

      if (savedGames) {
        try {
          savedGames = JSON.parse(savedGames);

          if (use) {
            this.games = savedGames.data;
            this.loading = false;
          }
        } catch (error) {
          console.error(error);
        }
      }

      return savedGames;
    },
    checkSavedGamesAge() {
      const savedGames = this.getSavedGames(false);

      if (savedGames) {
        const secondsSinceSaved = (new Date().getTime() - savedGames.timestamp) / 1000;
        return Math.floor(secondsSinceSaved);
      }

      return null;
    },
    refreshPoints() {
      this.$store.dispatch("getTeamStatus")
    }
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus
    }
  }
};
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  border-radius: 1em;
  margin: 2em auto;
  background-color: var(--dark);
  color: var(--green);
}

.games {
  display: grid;
  grid-template-columns: 1fr 1fr;
  list-style: none;
  padding: 0;
  margin: 0;
  gap: 1.5em;
}

.games .game {
  color: #fff;
  background-color: var(--dark);
  border-radius: 1em;
  padding: 1.5em;
  display: flex;
  flex-direction: column;
  align-items: start;
  justify-content: center;
}

.games .game .game-title {
  margin: 1rem 0 0;
  font-weight: 400;
}

.games .game .game-icon {
  margin: 0;
  width: 2em;
  height: 2em;
  overflow: hidden;
}

.games-loading .game {
  background-color: var(--dark);
  background-repeat: no-repeat;
  background-size: 10em 100%;
  background-image: linear-gradient(to right, var(--dark) 0%, hsl(323, 50%, 33%) 50%, var(--dark) 100%);
  animation-duration: 750ms;
  animation-fill-mode: forwards;
  animation-iteration-count: infinite;
  animation-name: shimmer;
  animation-timing-function: linear;
}

@keyframes shimmer {
  0% {
    background-position: -12em 0;
  }

  100% {
    background-position: 12em 0;
  }
}

.games-loading .game .game-title,
.games-loading .game .game-icon {
  color: hsl(323, 50%, 33%);
  background-color: hsl(323, 50%, 33%);
}
</style>
