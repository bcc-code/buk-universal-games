<template>
  <section
    class="bg"
    :style="{
      'background-image': `url(${image})`,
    }"
  >
    <img src="../assets/logo.png" alt="" class="logo" />
    <section class="leagues">
      <AdminLeagueSelector
        v-for="league in adminLeagues"
        class="league-card"
        :key="league.id"
        :name="league.name"
        @click="selectLeague(league.id)"
      />
    </section>
  </section>
</template>

<script>
import AdminLeagueSelector from "@/components/AdminLeagueSelector.vue";

export default {
  name: "AdminSelectLeague",
  components: { AdminLeagueSelector },
  data() {
    return {
      image: require("@/assets/bg.svg"),
    };
  },
  created() {
    if (!this.$store.state.adminLeagues.length) {
      this.getAdminLeagues();
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch("getAdminLeagues");
    },
    async selectLeague(id) {
      await this.$store.dispatch("setAdminLeagueSelected", id);
      this.$router.push({ name: "AdminLeagueStatus" });
    },
  },
  computed: {
    adminLeagues() {
      return this?.$store.state.adminLeagues;
    },
  },
};
</script>

<style scoped>
.bg {
  height: 100%;
  background-size: cover;
  background-position: center;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 1em;
}

.league-card {
  margin: 0.25em 0;
}
</style>
