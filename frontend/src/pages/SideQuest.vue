<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.status?.points"
      :stickers="this.coins.length"
      :refresh="refresh"
    />
    <section>
      <span>{{ $t("sidequest.youhaveunsubmittedanswers", { numAnswers: unsubmittedAnswers.length}) }}</span>
      <button @click="trySubmitAnswers">{{ $t("sidequest.submityouranswers") }}</button>
    </section>
    <section v-for="(round,index) in questions" :key="index">
      <h2>{{ $t("sidequest.gameroundtitle", {round: index}) }}</h2>
      <div class="round">
        <div v-for="question in round" :key="question.id">
            <h3><RouterLink :to="'sidequest/question/' + question.id">{{ $t("sidequest.questiontypes." + question.t) }}</RouterLink></h3>
        </div>
      </div>
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";

import { messageIcon } from "@/assets/icons/message.svg.ts";

export default {
  name: "SideQuest",
  components: { UserPageLayout, PointsAndStickers },
  data() {
    return {
      loading: false,
      resultParsed: null,
      messageIcon,
    };
  },
  mounted() {
    this.$store.dispatch("checkNewQuestions");
    if(this.questions[1].length == 0) {
      this.$store.commit("unlockNewQuestions", 1);
    }
  },
  created() {

  },
  methods: {
    refresh() {
      this.loading = true;
      this.$store.dispatch("getLeagueStatus", true);

      setTimeout(() => {
        this.loading = false;
      }, 1000);
    },
    trySubmitAnswers() {
      this.$store.dispatch("submitAnswers");
    },
  },
  computed: {
    questions() {
      return this.$store.state.qsOpened;
    },
    unsubmittedAnswers() {
      return this.$store.state.answers;
    },
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

.game-left, .game-right {
  display:block;
  min-width:30%;
  border-radius:15px;
  background-color: var(--gray-2);
}

.round {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin: 1em 0;
}

.game-left {
  clear:both;
  float: left;
}
.game-right {
  float: right;
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
