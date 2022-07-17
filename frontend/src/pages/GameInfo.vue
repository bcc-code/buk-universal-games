<template>
  <UserPageLayout>
    <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => refreshPoints()" />

    <h2>Games</h2>

    <GamesList :games="games" :loading="loading"></GamesList>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";
import GamesList from "@/components/GamesList.vue";
import { getData } from "@/libs/apiHelper";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout, PointsAndStickers, GamesList },
  data() {
    return {
      loginError: "Game Info",
      games: [],
      loading: true,
    };
  },
  created() {
    if (Object.keys(this.$store.state.teamStatus).length === 0) {
      this.refreshPoints();
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
      this.$store.dispatch("getTeamStatus");
    },
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus;
    },
  },
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
</style>
