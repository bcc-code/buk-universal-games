<template>
  <AdminPageLayout>
    <h2>Games</h2>

    <GamesList :games="$store.state.games" :loading="$store.state.gamesLoading" @clicked="gameClicked"></GamesList>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import GamesList from "@/components/GamesList.vue";

export default {
  name: "AdminGames",
  components: { AdminPageLayout, GamesList },
  data() {
    return {};
  },
  created() {
    this.$store.dispatch("getGames");
  },
  methods: {
    gameClicked(game) {
      this.$router.push({
        name: "AdminGameInfoDetail",
        params: {
          game: game.id,
        },
      });
    },
  },
  computed: {},
};
</script>

<style scoped></style>
