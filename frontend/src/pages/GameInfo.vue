<template>
  <UserPageLayout>
    <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => refreshPoints()" />

    <h2>Games</h2>

    <GamesList :games="$store.state.games" :loading="$store.state.gamesLoading"></GamesList>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";
import GamesList from "@/components/GamesList.vue";

export default {
  name: "GameInfo",
  components: { UserPageLayout, PointsAndStickers, GamesList },
  created() {
    this.$store.dispatch("getGames");

    if (Object.keys(this.$store.state.teamStatus).length === 0) {
      this.refreshPoints();
    }
  },
  methods: {
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
