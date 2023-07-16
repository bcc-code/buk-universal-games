<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.points"
      :stickers="this.coins.length"
      :refresh="refresh"
    />
    <section v-if="unsubmittedAnswers.length > 0">
      <div class="message-white">
        <p>
          <span>{{ $t("sidequest.youhaveunsubmittedanswers", { numAnswers: unsubmittedAnswers.length}) }}</span>
        </p>
        <button class="btn-success" @click="trySubmitAnswers">{{ $t("sidequest.submityouranswers") }}</button>
      </div>
    </section>
    <section v-for="(questions,index) in questionsPerRound" :key="index">
      <h2>{{ $t("sidequest.gameroundtitle", {round: index + 1}) }}</h2>
      <div class="round">
        <div class="message-white" v-for="question in questions" :key="question.id">
            <h3><RouterLink :to="'sidequest/question/' + question.id">{{ $t("sidequest.questiontypes." + question.t) }}</RouterLink></h3>
        </div>
      </div>
    </section>
    <section>
      <h2>Round {{ questionsPerRound.length + 1 }}</h2>
      <div class="" v-if="questionsPerRound.length < 5">
        <div class="round">
          <p>{{ $t("sidequest.noroundyet", {nextRound: questionsPerRound.length + 1}) }}</p>
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
    this.$store.dispatch("checkNewQuestions", this.$store.state.matches);
    if(this.questionsPerRound.length == 0) {
      this.$store.commit("unlockNewQuestions", 1);
    }
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
    questionsPerRound() {
      console.log(this.$store.state.qsOpened);
      return this.$store.state.qsOpened.filter(round => round.length > 0)  || [];
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
.round div {
  border-radius: 1em;
  padding: 0.5em;
  background-color: #fff;
  width: 100%;
  margin:0 0 0 .5em;
}
.round div:first-child {
  margin:0 .5em 0 0;
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

.message-white {
  border-radius: 1em;
  margin-top: 1em;
  padding: 1em 1em 4em 1em;
  background-color: #fff;
}

.message-white button {
  margin-top:10px;
  float:right;
  padding:10px;
}

</style>
