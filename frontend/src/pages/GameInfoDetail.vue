<template>
  <UserPageLayout :showTitle="false">
    <nav>
      <button @click="$router.back()">&lt; Back</button>
    </nav>

    <header>
      <h2>
        <img class="icon" :src="require(`@/assets/icons/game-${gameParsed.gameType}.svg`)" />
        <span>{{ $t("games." + gameParsed.gameType) }}</span>
      </h2>

      <div class="banner">
        <video autoplay loop muted playsinline>
          <source src="/video/crowdsurfing.mp4" type="video/mp4" />
        </video>
      </div>
    </header>

    <p class="leadstory">
      {{ $t("leadstory." + gameParsed.gameType) }}
    </p>
    <p class="description">
      <span v-html="circleInfoIcon"></span>
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
    <section>
      <h2>{{ $t("yourmatch") }}</h2>
      <MatchListItem v-if="match" :gameType="gameParsed.gameType" :gameAddOn="match.addOn" :team1="match.team1"
        :team2="match.team2" :start="match.start" :winner="match.winner" :class="{ 'card-light': true }"></MatchListItem>
    </section>

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
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import LeagueListItem from "@/components/LeagueListItem.vue";
import MatchListItem from "@/components/MatchListItem.vue";
import minefieldIcon from "@/assets/icons/game-minefield.svg";
import { gameFireIcon } from "@/assets/icons/game-fire.svg.ts";
import { gameMetalIcon } from "@/assets/icons/game-metal.svg.ts";
import { gameWoodIcon } from "@/assets/icons/game-wood.svg.ts";
import { gameWaterIcon } from "@/assets/icons/game-water.svg.ts";
import { circleInfoIcon } from "@/assets/icons/circle-info.svg.ts";
import { groupIcon } from "@/assets/icons/group.svg.ts";

export default {
  name: "GameInfoDetail",
  props: {
    game: String,
  },
  components: { UserPageLayout, LeagueListItem, MatchListItem },
  data() {
    return {
      gameParsed: {},
      bannerImage: "",
      icons: {
        minefield: minefieldIcon,
        Fire: gameFireIcon,
        Metal: gameMetalIcon,
        Wood: gameWoodIcon,
        Water: gameWaterIcon,
      },
      circleInfoIcon,
      groupIcon
    };
  },
  created() {
    this.$store.dispatch("getLeagueStatus");
    if (this.game) {
      this.gameParsed = this.$store.state.games.find((game) => game.id == this.game);
      this.bannerImage = `/images/illustrations/Universal-BUK-Games-${this.gameParsed.gameType}.svg`;
    } else {
      this.$router.back();
    }
  },
  mounted() {
    // if (this.game) {
    //   this.gameParsed = this.$store.state.games.find((game) => game.id == this.game);
    //   this.bannerImage = `/images/illustrations/Universal-BUK-Games-${this.gameParsed.gameType}.svg`;
    // } else {
    //   this.$router.back();
    // }
  },
  methods: {},
  computed: {
    teamStatus() {
      return this.$store.state.teamStatus;
    },
    match() {
      return this.$store.state.matches.find((match) => match.teamId == this.teamStatus?.status?.teamId && match.gameId == this.gameParsed?.id);
    },
    ranking() {
      const gameType = this.gameParsed?.gameType;
      const leagueStatus = this.$store.state.leagueStatus;
      console.log(gameType);
      if (gameType) {
        console.log(leagueStatus?.status);
        return leagueStatus?.status?.[gameType] || [];
      }
      return [];
    },
    rules() {
      return this.$t('rules.' + this.gameParsed.gameType).split('|');
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
  align-items: end;
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
