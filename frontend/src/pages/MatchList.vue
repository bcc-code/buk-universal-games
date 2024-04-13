<template>
  <UserPageLayout>
    <div v-if="matches.error">
      <h2>{{ $t('general_error') }}</h2>
      <div class="message">
        <p class="message-text">{{ matches.error }}</p>
        <p class="message-text">{{ $t('pleaserefresh') }}</p>
      </div>
    </div>

    <section class="match-title" v-if="matches.length">
      <div class="match-title-column">
        <h2 class="match-title-text">{{ $t('game') }}</h2>
      </div>
      <div class="match-title-column">
        <h2 class="match-title-text">{{ $t('team1team2') }}</h2>
      </div>
      <div class="match-title-column">
        <h2 class="match-title-text">{{ $t('start') }}</h2>
      </div>
    </section>
    <section class="user-section" v-for="match in matches" :key="match.id">
      <div
        style="
          height: 2px;
          width: 100%;
          background-color: var(--dark);
          margin: 5px 0 10px 0;
        "
        v-if="match == currentActiveMatch"
      ></div>
      <MatchListItem
        :gameType="whichGame(match.gameId)?.gameType"
        :gameAddOn="match.addOn"
        :team1="match.team1"
        :team2="match.team2"
        :team1result="match.team1Result"
        :team2result="match.team2Result"
        :start="match.start"
        :winner="match.winner"
        :clickFunc="() => matchClicked(match)"
        :currentActiveMatch="initMatch(match)"
        @click="gameClicked(match.gameId)"
      />
    </section>
  </UserPageLayout>
</template>
<script>
import UserPageLayout from '../components/UserPageLayout.vue';
import MatchListItem from '../components/MatchListItem.vue';
import MatchCard from '../components/MatchCard.vue';

export default {
  name: 'MatchList',
  props: {
    data: String,
  },
  components: { UserPageLayout, MatchListItem, MatchCard },
  data() {
    return {
      loginError: 'Match List',
      selectedMatch: {},
      currentActiveMatch: {},
    };
  },
  created() {
    this.$store.dispatch('getMatches', false);
    this.$store.dispatch('getGames', false);
  },
  mounted() {
    this.getMatches();
  },
  methods: {
    async getMatches() {
      let minutesBeforeNow = 20;
      let now = new Date(new Date().getTime() - minutesBeforeNow * 60 * 1000);
      let currentTime = now.getHours() + ':' + now.getMinutes();
      //let currentTime = "12:49";

      this.initMatch(
        this.matches.filter((match) => match.start >= currentTime)[0] ||
          this.matches[0],
      );
    },
    gameClicked(id) {
      this.$router.push({
        name: 'GameInfoDetail',
        params: {
          game: id,
        },
      });
    },
    getGames() {},
    whichGame(id) {
      if (this.games.error) {
        return {};
      }

      let game = this.games.find((game) => game.id == id);
      return game;
    },
    matchClicked(match) {
      this.selectedMatch = match;
    },
    initMatch(match) {
      let minutesBeforeNow = 20;
      let now = new Date(new Date().getTime() - minutesBeforeNow * 60 * 1000);
      let currentTime = now.getHours() + ':' + now.getMinutes();

      // return this.match.start >= currentTime;
      return false;
    },
  },
  computed: {
    matches() {
      console.log(this.$store.state.matches);
      console.log('stop here');
      return this.$store.state.matches;
    },
    games() {
      return this.$store.state.games;
    },
  },
};
</script>

<style scoped>
.match-title {
  padding: 0 0.25em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 40% 47% 13%;
}

.match-title-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.match-title-text {
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}

.message {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1.5em;
  background-color: var(--dark);
  color: #fff;
  text-align: center;
}

.message .message-text {
  font-size: 1.5em;
}
</style>
