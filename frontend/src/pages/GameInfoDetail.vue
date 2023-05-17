<template>
  <UserPageLayout :showTitle="false">
    <nav>
      <button @click="$router.back()">&lt; Back</button>
    </nav>

    <header>
      <h2>
        <span class="icon" v-html="icons[gameParsed.gameType]"></span>
        <span>{{ gameParsed.name }}</span>
      </h2>

      <div class="banner" :style="{ 'background-image': `url(${bannerImage})` }">

      </div>
    </header>

    <p class="description">
      {{ gameParsed.description }}
    </p>
    <p class="description">
      <span v-html="groupIcon"></span>
      {{ gameParsed.participantsInfo }}
    </p>
    <p class="description">
      <span v-html="circleInfoIcon"></span>
      {{ gameParsed.safetyInfo }}
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
        <LeagueListItem
          :class="{ 'card-light': i > 4, 'green-font': status.teamId == teamStatus?.status?.teamId }"
          :index="i + 1"
          :team="status.team"
          :stickers="status.stickers"
          :points="status.points"
          :loading="loading"
        />
      </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import LeagueListItem from "@/components/LeagueListItem.vue";
import { gameEarthIcon } from "@/assets/icons/game-earth.svg.ts";
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
  components: { UserPageLayout, LeagueListItem },
  data() {
    return {
      gameParsed: {},
      ranking: [],
      bannerImage: "",
      icons: {
        Earth: gameEarthIcon,
        Fire: gameFireIcon,
        Metal: gameMetalIcon,
        Wood: gameWoodIcon,
        Water: gameWaterIcon,
      },
      circleInfoIcon,
      groupIcon
    };
  },
  mounted() {
    if (this.game) {
      this.gameParsed = this.$store.state.games.find((game) => game.id == this.game);
      this.ranking = this.$store.state.leagueStatus.status[this.gameParsed.gameType]; 
      this.bannerImage = `/images/illustrations/Universal-BUK-Games-${this.gameParsed.gameType}.svg`;
    } else {
      this.$router.back();
    }
  },
  methods: {},
  computed: {
    gameRanking() {
      return this.$store.state.leagueStatus;
    },
  },
};
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  height: 20em;
  border-radius: 1em;
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

nav {
  display: flex;
  align-items: center;
}

nav h3 {
  padding: 0 .75em;
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
