<template>
  <UserPageLayout :showTitle="false">
    <nav>
      <button @click="$router.back()">&lt;</button>
      <h3>Games</h3>
    </nav>

    <header>
      <h2>
        <span class="icon" v-html="icons[gameParsed.name]"></span>
        <span>{{ gameParsed.name }}</span>
      </h2>

      <div class="banner" :style="{ 'background-image': `url(${bannerImage})` }">
        <div>
          <p>W: {{ gameParsed.winnerPoints }}pt</p>
        </div>
        <div>
          <p>L: {{ gameParsed.looserPoints }}pt</p>
        </div>
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
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
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
  components: { UserPageLayout },
  data() {
    return {
      gameParsed: {},
      bannerImage: "/images/illustrations/Universal-BUK-Games-Water.svg",
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
      this.gameParsed = JSON.parse(this.game);
      this.bannerImage = `/images/illustrations/Universal-BUK-Games-${this.gameParsed.name}.svg`;
    } else {
      this.$router.back();
    }
  },
  methods: {},
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
</style>
