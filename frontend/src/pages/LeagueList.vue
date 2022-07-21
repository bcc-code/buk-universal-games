<template>
  <UserPageLayout class="league-list">
    <section class="user-section">
      <PointsAndStickers
        :points="teamStatus?.status?.points"
        :stickers="teamStatus?.status?.stickers"
        :refresh="() => getTeamStatus(true)"
      />
    </section>
    <section class="league-title">
      <div class="league-title-column index-column"></div>
      <div class="league-title-column">
        <h2 class="league-title-text">Team</h2>
      </div>
      <div class="league-title-column">
        <h2 class="league-title-text">Stickers</h2>
      </div>
      <div class="league-title-column">
        <h2 class="league-title-text">Points</h2>
      </div>
    </section>
    <section class="user-section" v-for="(status, i) in leagueStatus" :key="status.id">
      <LeagueListItem
        :class="{ 'card-light': i > 4, 'green-font': status.teamId == teamStatus?.status?.teamId }"
        :index="i + 1"
        :team="status.team"
        :stickers="status.stickers"
        :points="status.points"
      />
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "../components/UserPageLayout.vue";
import PointsAndStickers from "../components/PointsAndStickers.vue";
import LeagueListItem from "../components/LeagueListItem.vue";

export default {
  name: "LoginPage",
  props: {
    data: String,
  },
  components: { UserPageLayout, PointsAndStickers, LeagueListItem },
  created() {
    if (Object.keys(this.$store.state.teamStatus).length === 0) {
      this.getTeamStatus(false);
    }

    if (Object.keys(this.$store.state.leagueStatus).length === 0) {
      this.getLeagueStatus();
    }
  },
  methods: {
    getTeamStatus(override) {
      this.$store.dispatch("getTeamStatus", override);
    },
    getLeagueStatus() {
      this.$store.dispatch("getLeagueStatus");
    },
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus;
    },
    leagueStatus() {
      // return this?.$store.state.leagueStatus.status?.slice(0, 10)
      return this?.$store.state.leagueStatus.status;
    },
  },
};
</script>

<style scoped>
.league-title {
  padding: 0 1em;
  border-radius: 0.75em;
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
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}

.index-column {
  width: 25%;
}

.green-font {
  color: var(--green);
}
</style>
