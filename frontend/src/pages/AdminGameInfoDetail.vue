<template>
  <AdminPageLayout :showTitle="false">
    <div class="flex">
    <nav>
    <div
          @click="$router.back()"
          class="p-1 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue"
        >
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
      <h2>
        <img class="icon" :src="`/icon/game-${gameType}.svg`" />
        <span>{{ $t(`games.${gameParsed.gameType}`) }}</span>
      </h2>
    </div>
    <header>


      <div class="banner">
        <video autoplay muted playsinline controls>
          <source :src="`/video/${gameType}.mp4`" type="video/mp4" />
        </video>
      </div>
    </header>

    <p class="leadstory">
      {{ $t("leadstory." + gameParsed.gameType) }}
    </p>
    <p class="description">
      <img src="/icon/circle-info.svg" />
      {{ $t("explanation." + gameParsed.gameType) }}
    </p>

    <p class="description">
      <span>{{ $t("rulestitle") }}</span>
    <ul>
      <li v-for="(rule, index) in rules" :key="index">
        {{ rule }}
      </li>
    </ul>
    </p>

    <section class="league-title" v-if="ranking?.length">
      <div class="league-title-column index-column"></div>
      <div class="league-title-column">
        <h2 class="league-title-text">Team</h2>
      </div>
      <div class="league-title-column">
        <h2 class="league-title-text">Points</h2>
      </div>
    </section>
    <section class="user-section" v-for="(status, i) in ranking" :key="status.id">
      <LeagueListItem :class="{ 'card-light': i > 4, 'green-font': status.teamId == teamStatus?.status?.teamId }"
        :index="i + 1" :team="status.team" :stickers="status.stickers" :points="status.points" />
    </section>
  </AdminPageLayout>
</template>

<script>
import AdminPageLayout from "@/components/AdminPageLayout.vue";
import LeagueListItem from "@/components/LeagueListItem.vue";
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';

export default {
  name: "GameInfoDetail",
  props: {
    game: String,
  },
  components: { LeagueListItem, AdminPageLayout, ArrowLeftIcon },
  data() {
    return {
      gameParsed: {},
    };
  },
  created() {
    if (this.game) {
      this.gameParsed = this.$store.state.games.find((game) => game.id == this.game);
    } else {
      this.$router.back();
    }
  },
  methods: {},
  computed: {
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
    gameType() {
      return this.gameParsed.gameType.replaceAll('_', '');
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
  padding: 0 .75em;
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

.description>img {
  width: 24px;
}

header h2 {
  display: flex;
  align-items: center;
  gap: 0.5em;
  margin: .5em 0;
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
</style>
