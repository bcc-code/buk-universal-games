<template>
  <UserPageLayout class="league-list">
    <section class="user-section">
      <PointsAndStickers />
      <div v-if="teamStatus?.error">
        <p>{{ $t('general_error') }}</p>
        <p>{{ teamStatus.error }}</p>
        <p>{{ $t('please_refresh') }}</p>
      </div>
    </section>
    <FamilyStatus></FamilyStatus>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '../components/UserPageLayout.vue';
import PointsAndStickers from '../components/PointsAndStickers.vue';
import FamilyStatus from '@/components/FamilyStatus.vue';

export default {
  name: 'LeagueList',
  components: { UserPageLayout, PointsAndStickers, FamilyStatus },
  data() {
    return {
      loading: false,
    };
  },
  created() {
    this.getLeagueStatus(false);
  },
  methods: {
    getLeagueStatus(override) {
      this.$store.dispatch('getLeagueStatus', override);
    },
  },
  computed: {
    teamStatus() {
      return this?.leagueStatus?.status?.total?.find(
        (score) => score.team == this.$store.state.loginData?.team,
      );
    },
    leagueStatus() {
      return this?.$store.state.leagueStatus;
    },
  },
};
</script>

<style scoped>
.ranking-title {
  padding: 0 1em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 10% 75% 15%;
}

.ranking-title-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.ranking-title-text {
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}

.index-column {
  width: 25%;
}

.card-currentTeam {
  color: var(--dark-brown);
  padding: 0.1em 1em;
  box-shadow: 2px 2px;
  background-color: var(--yellow);
}

.nodata {
  text-align: center;
  margin-top: 1em;
  font-size: 1.5em;
  color: #888;
}

.message {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1.5em;
  background-color: var(--dark);
  color: #fff;
  text-align: center;
}

.message-white {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1em 1em 4em 1em;
  background-color: #fff;
}

.rankingfrozen-message {
  border-radius: 1em;
  margin-top: 1em;
  padding: 0.5em;
  background-color: var(--yellow);
  color: var(--red);
  text-align: center;
}

.btn-success {
  color: #333;
}

.message-white button {
  margin-top: 10px;
  float: right;
  padding: 10px;
}

.message .message-text {
  font-size: 1.5em;
}
</style>
