<template>
  <UserPageLayout>
    <PointsAndStickers :points="teamStatus?.status?.points" :stickers="teamStatus?.status?.stickers" :refresh="() => refreshPoints()" />

    <div v-if="resultParsed && !resultParsed.error">
      <h2 class="heading-text">{{ headingText }}</h2>
      <div :class="{ 'heading-icon': true, success: resultParsed.success, failure: !resultParsed.success }">
        <span v-html="headingIcon"></span>
      </div>

      <div v-if="resultParsed.message" class="message">
        <span class="message-icon" v-html="messageIcon"></span>
        <p class="message-text">{{ resultParsed.message }}</p>
      </div>
    </div>
    <div v-else>
      <p>Something went wrong ...</p>
      <p>Make sure you are scanning a QR code belonging to your league.</p>
    </div>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";

import { bukSmileIcon } from "@/assets/icons/buk-smile.svg.ts";
import { bukSadIcon } from "@/assets/icons/buk-sad.svg.ts";
import { messageIcon } from "@/assets/icons/message.svg.ts";

export default {
  name: "ScanResult",
  props: {
    result: String,
  },
  components: { UserPageLayout, PointsAndStickers },
  data() {
    return {
      resultParsed: null,
      headingText: "",
      headingIcon: "",
      bukSmileIcon,
      bukSadIcon,
      messageIcon,
    };
  },
  mounted() {
    // const result = JSON.stringify({
    //   code: "LKJQMNG",
    //   success: true,
    //   message: "Nice! You found a new sticker and 154 code to your team!",
    //   points: 154,
    // });
    console.log("Sticker result", this.result);

    if (this.result) {
      try {
        this.resultParsed = JSON.parse(this.result);

        if (this.resultParsed.success) {
          this.headingText = "Sticker added";
          this.headingIcon = bukSmileIcon;
        } else {
          this.headingText = "Sticker not added";
          this.headingIcon = bukSadIcon;
        }
      } catch (error) {
        console.error(error);
        this.$router.push({ name: "Login" });
      }
    } else {
      this.$router.push({ name: "Login" });
    }
  },
  created() {
    if (Object.keys(this.$store.state.teamStatus).length === 0) {
      this.refreshPoints();
    }
  },
  methods: {
    refreshPoints() {
      this.$store.dispatch("getTeamStatus", true);
    },
  },
  computed: {
    teamStatus() {
      return this?.$store.state.teamStatus;
    },
  },
};
</script>

<style scoped>
.heading-text {
  text-align: center;
}

.heading-icon {
  background-color: var(--gray-2);
  border-radius: 1em;
  padding: 0.5em 1.5em;
  text-align: center;
  color: #000;
  display: flex;
  justify-content: center;
  align-items: center;
}

.heading-icon > span {
  width: 3em;
  height: 3em;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: auto;
}

.heading-icon.failure {
  background-color: var(--red);
  color: hsl(353, 100%, 10%);
}
.heading-icon.success {
  background-color: var(--green);
  color: hsl(158, 93%, 5%);
}

.message {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1.5em;
  background-color: var(--dark);
  color: #fff;
  text-align: center;
}

.message .message-icon {
  width: 4em;
  height: 4em;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: auto;
}
.message .message-text {
  font-size: 1.5em;
}
</style>
