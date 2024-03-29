<template>
  <UserPageLayout class="league-list">
    <section class="user-section">
      <PointsAndStickers
        :loading="loading"
        :points="teamStatus?.points"
        :stickers="coins.length"
        :refresh="refresh"
      />
      <div v-if="teamStatus?.error">
        <h2>{{ $t("general_error") }}</h2>
        <p>{{ teamStatus.error }}</p>
        <p>{{ $t("please_refresh") }}</p>
      </div>
    </section>
    <div v-if="loading">
      <section class="ranking-title">
        <div class="ranking-title-column index-column"></div>
        <div class="ranking-title-column">
          <h2 class="ranking-title-text">Team</h2>
        </div>
        <div class="ranking-title-column">
          <h2 class="ranking-title-text">Points</h2>
        </div>
      </section>
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
      <div v-if="leagueStatus.error">
        <div v-if="leagueStatus.errorCode != 406">
          <h2>{{ $t("general_error") }}</h2>

          <div class="message">
            <p class="message-text">{{ leagueStatus.error }}</p>
            <br />
            <p class="message-text">{{ $t("pleaserefresh") }}</p>
          </div>
        </div>
      </div>
      <div class="message-white">
        <p>
          {{$t('league.viewgamerankings-title')}}
        </p>
        <button class="btn-success" @click="this.$router.push('games')">{{ $t("league.viewgamerankings") }}</button>
      </div>
      <div class="rankingfrozen-message" v-if="$store.getters.currentRound > 3">
        <h3>{{ $t("league.ranking-frozen") }}</h3>
      </div>
      <section class="ranking-title" v-if="leagueStatus.status?.total.length">
        <div class="ranking-title-column index-column"></div>
        <div class="ranking-title-column">
          <h2 class="ranking-title-text">Team</h2>
        </div>
        <div class="ranking-title-column">
          <h2 class="ranking-title-text">Points</h2>
        </div>
      </section>
      <section v-else>
        <div class="nodata">
          <p class="message-text">{{ $t("league.rankingisempty") }}</p>
        </div>
      </section>
      <section class="user-section" v-for="(status, i) in leagueStatus?.status?.total.sort((a, b) => b.points - a.points)" :key="status.id">
        <LeagueListItem
          :class="{ 'card-light': status.teamId != teamStatus?.teamId, 'card-currentTeam': status.teamId == teamStatus?.teamId }"
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
  name: "LeagueList",
  components: { UserPageLayout, PointsAndStickers, LeagueListItem },
  data() {
    return {
      loading: false,
    };
  },
  created() {
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
      this.getLeagueStatus(true);

      setTimeout(() => {
        this.loading = false;
      }, 1000);
    },
  },
  computed: {
    coins() {
      return this.$store.state.coins;
    },
    teamStatus() {
      return this?.leagueStatus?.status?.total?.find((score) => score.team == this.$store.state.loginData?.team);
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
  color: var(--dark);
  padding:.1em 1em;
  box-shadow:2px 2px;
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
  padding: .5em;
  background-color: var(--yellow);
  color: var(--red);
  text-align: center;
}

.btn-success {
  color:#333;
}

.message-white button {
  margin-top:10px;
  float:right;
  padding:10px;
}

.message .message-text {
  font-size: 1.5em;
}
</style>
