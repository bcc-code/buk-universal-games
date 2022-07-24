<template>
  <AdminPageLayout title="League status">
    <section class="leagues">
      <AdminLeagueCard v-for="league in adminLeagues" :class="{'green-font' : league.id == $store.state.adminLeagueSelected}" :key="league.id" :name="league.name" @click="selectLeague(league.id)"/>
    </section>
    <section class="league-title">
        <div class="league-title-column index-column">
        </div>
        <div class="league-title-column">
            <h2 class="league-title-text">Team</h2>
        </div>
        <div class="league-title-column">
            <h2 class="league-title-text">Stickers </h2>
        </div>
        <div class="league-title-column">
            <h2 class="league-title-text">Points</h2>
        </div>
    </section>
    <section class="user-section" v-for="(status, i) in adminLeagueStatus" :key="status.id">
      <LeagueListItem
        :class="{ 'card-light': i > 4 }"
        :index="i + 1"
        :team="status.team"
        :stickers="status.stickers"
        :points="status.points"
      />
    </section>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import LeagueListItem from "@/components/LeagueListItem.vue";
import AdminLeagueSelector from "@/components/AdminLeagueSelector.vue";

export default {
  name: "AdminLeagueStatus",
  components: { AdminPageLayout, LeagueListItem, AdminLeagueSelector },
  created() {
    if(!this.$store.state.adminLeagues.length) {
       this.getAdminLeagues() 
    }

    if(Object.keys(this.$store.state.adminLeagueStatus).length === 0) {
       this.getAdminLeagueStatus() 
    }
  },
  methods: {
    getAdminLeagues() {
      this.$store.dispatch("getAdminLeagues")
    },
    getAdminLeagueStatus() {
      this.$store.dispatch("getAdminLeagueStatus")
    },
    async selectLeague(id) {
      await this.$store.dispatch("setAdminLeagueSelected", id)
      this.getAdminLeagueStatus()
      this.getMatches()
    },
    getMatches() {
      this.$store.dispatch("getAdminMatches")
    },
  },
  computed: {
    adminLeagueStatus() {
      // return this?.$store.state.adminLeagueStatus.status?.slice(0, 10)
      return this?.$store.state.adminLeagueStatus.status
    },
    adminLeagues() {
      return this?.$store.state.adminLeagues
    }
  }
};
</script>

<style scoped>
  .league-title {
      padding: 0 1em;
      border-radius: .75em;
      display: grid;
      grid-template-columns: 2fr 8fr 3fr 3fr;
  }

  .league-title-column {
      display: flex;
      flex-direction: column;
      width: 100%;
  }

  .league-title-column:nth-last-child(-n + 2) {
    align-items: flex-end;
  }

  .league-title-text {
      font-size: .85em;
      color: var(--gray-2);
      margin: 1em 0 0;
  }

  .leagues {
    display: flex;
    width: 100%;
  }

  .green-font {
    color: var(--green);
  }
</style>
