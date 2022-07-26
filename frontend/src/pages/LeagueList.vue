<template>
  <UserPageLayout class="league-list">
    <section class="user-section">
      <PointsAndStickers
        :loading="loading"
        :points="teamStatus?.status?.points"
        :stickers="teamStatus?.status?.stickers"
        :refresh="refresh"
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
    <div v-if="loading">
      <section class="user-section" v-for="i in [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]" :key="i">
        <LeagueListItem
          :class="{ 'card-light': i > 4 }"
          :index="i + 1"
          :team="'Team ' + (i + 1)"
          :stickers="Math.ceil(100 / (i + 1))"
          :points="Math.ceil(10000 / (i + 1))"
          :loading="loading"
        />
      </section>
    </div>
    <div v-else>
      <section class="user-section" v-for="(status, i) in leagueStatus" :key="status.id">
        <LeagueListItem
          :class="{ 'card-light': i > 4, 'green-font': status.teamId == teamStatus?.status?.teamId }"
          :index="i + 1"
          :team="status.team"
          :stickers="status.stickers"
          :points="status.points"
          :loading="loading"
        />
      </section>
    </div>
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
  data() {
    return {
      loading: false,
    };
  },
  created() {
    this.getTeamStatus(false);
    this.getLeagueStatus(false);
  },
  methods: {
    getTeamStatus(override) {
      this.$store.dispatch("getTeamStatus", override);
    },
    getLeagueStatus(override) {
      this.$store.dispatch("getLeagueStatus", override);
    },
    refresh() {
      this.loading = true;
      this.getTeamStatus(true);
      this.getLeagueStatus(true);

      setTimeout(() => {
        this.loading = false;
      }, 1000);
    },
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus;
    },
    leagueStatus() {
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
