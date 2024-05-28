<template>
  <UserPageLayout :showTitle="false">
    <div class="flex">
      <nav class="text-dark mr-4">
        <div
          @click="$router.back()"
          class="p-1 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-label-1"
        >
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
      <h1 class="text-label-1 font-bold py-2 px-2 rounded-md flex space-x-3">
        <img
          class="h-10 w-10"
          :src="`/icon/game-${gameParsed.gameType.replaceAll('_', '')}.svg`"
        />
        <span>{{ $t(`games.${gameParsed.gameType}`) }}</span>
      </h1>
    </div>
    <div class="mb-5">
      <!-- 
      <div class="banner">
        <video autoplay muted playsinline controls>
          <source :src="`/video/${gameParsed.gameType.replaceAll('_','')}.mp4`" type="video/mp4" />
        </video>
      </div> -->
    </div>
    <div class="bg-ice-blue rounded-md p-4">
      <p class="text-dark-blue whitespace-pre-wrap">
        {{ $t('leadstory.' + gameParsed.gameType) }}
      </p>
      <p class="text-dark-blue flex mt-4 whitespace-pre-line">
        <img class="w-5 h-5 mr-3" src="/icon/circle-info.svg" />
        {{ $t('explanation.' + gameParsed.gameType) }}
      </p>
      <div class="mt-3 mb-1">
        <span class="font-bold">{{ $t('rulestitle') }}</span>
        <p class="text-dark-blue whitespace-pre-wrap">
          {{ $t('rules.' + gameParsed.gameType) }}
        </p>
      </div>
      <div class="mt-3 mb-1">
        <span class="font-bold">{{ $t('categorytitle') }}</span>
        <p class="text-dark-blue whitespace-pre-wrap">
          {{ $t('category.' + gameParsed.gameType) }}
        </p>
      </div>
      <div class="mt-3 mb-1">
        <span class="font-bold">{{ $t('pointstitle') }}</span>
        <p class="text-dark-blue whitespace-pre-wrap">
          {{ $t('pointexplenation.' + gameParsed.gameType) }}
        </p>
      </div>
    </div>

    <section>
      <h2 class="text-label-1 mt-5 mb-3">{{ $t('yourmatch') }}</h2>
      <MatchListItem
        v-if="match"
        :key="match.id"
        :gameType="getGameById(match.gameId)?.gameType"
        :gameAddOn="match.addOn"
        :team1="match.team1"
        :team2="match.team2"
        :team1result="match.team1Result"
        :team2result="match.team2Result"
        :start="match.start"
        :winner="match.winner"
      />
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from '@/components/UserPageLayout.vue';
import MatchListItem from '@/components/MatchListItem.vue';
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';

export default {
  name: 'GameInfoDetail',
  props: {
    game: String,
  },
  components: { UserPageLayout, MatchListItem, ArrowLeftIcon },
  data() {
    return {
      gameParsed: {},
    };
  },
  created() {
    this.$store.dispatch('getLeagueStatus');
    if (this.game) {
      this.gameParsed = this.$store.state.games.find(
        (game) => game.id == this.game,
      );
    } else {
      this.$router.back();
    }
  },
  methods: {
    getGameById(id) {
      if (this.games.error) {
        return {};
      }

      let game = this.games.find((game) => game.id == id);
      return game;
    },
  },
  computed: {
    teamStatus() {
      return this?.leagueStatus?.status?.total?.find(
        (score) => score.team == this.$store.state.loginData?.team,
      );
    },
    match() {
      return this.$store.state.matches.find(
        (match) => match.gameId == this.gameParsed?.id,
      );
    },
    ranking() {
      const gameType = this.gameParsed?.gameType;
      const leagueStatus = this.$store.state.leagueStatus;
      if (gameType) {
        return leagueStatus?.status?.[gameType] || [];
      }
      return [];
    },
    rules() {
      return this.$t('rules.' + this.gameParsed.gameType).split('|');
    },
    games() {
      return this?.$store.state.games;
    },
  },
};
</script>

<style scoped>
.banner {
  width: 100%;
  border: 5px solid var(--dark);
  border-radius: 20px;
  margin: 1.5em auto;
  background-color: var(--dark);
  color: #fff;
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  background-size: 80%;
  background-position: center;
  background-repeat: no-repeat;
}

video {
  width: 100%;
  height: 100%;
  object-fit: aspect-fill;
  border-radius: 15px;
}

.icon {
  max-width: 4em;
}

nav {
  display: flex;
  align-items: center;
}

nav h3 {
  padding: 0 0.75em;
}

.leadstory {
  color: #555;
  font-style: italic;
  line-height: 1.7;
  font-size: medium;
  white-space: break-spaces;
  margin: 1.5em -1em;
  padding: 1em;
  background: #fff;
}

.description {
  color: #555;
  line-height: 1.5;
  white-space: break-spaces;
  margin: 1.5em 0;
}

.description > img {
  width: 24px;
}

header h2 {
  display: flex;
  align-items: center;
  gap: 0.5em;
  margin: 0.5em 0;
}

.league-title {
  padding: 0 1em;
  border-radius: 0.75em;
  display: grid;
  grid-template-columns: 10% 75% 15%;
}

.league-title-column {
  display: flex;
  flex-direction: column;
  width: 100%;
}

.league-title-text {
  font-size: 0.85em;
  color: var(--gray-2);
  margin: 1em 0 0;
}

.card-currentTeam {
  color: var(--dark);
  padding: 0.1em 1em;
  box-shadow: 2px 2px;
  background-color: var(--yellow);
}
</style>
