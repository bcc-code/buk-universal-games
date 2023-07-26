<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.points"
      :stickers="this.coins.length"
      :refresh="refresh"
    />

    <section class="error-popup" v-if="showErrorPopup">
      <p><span>{{ popupErrorMessage }}</span></p>
    </section>

    <section>
      <h2>{{ $t("menu.sidequest") }}</h2>
      <div class="introduction">
        <h3 @click="showExplanation">{{ $t("sidequest.explanation") }} <span :class="{'toggle-arrow': true, 'rotated': isExplanationVisible}"></span> </h3>
        <span :class="{'show': isExplanationVisible}">{{ $t("sidequest.explanation_text") }}</span>
      </div>
    </section>

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
      <div :class="{'round':true, 'locked': ($store.getters.currentRound > 0 && index + 1 < $store.getters.currentRound)}">
        <div class="question" v-for="question in questions" :key="question.id" @click="questionClicked(question.id, index + 1)">
          <div class="question-button">
          <img :src="`/icon/sq-${question.t}.svg`" />
          <h3>
            <span>{{ $t("sidequest.questiontypes." + question.t) }}</span>
          </h3>
        </div>
        <span class="locked-indicator" v-if="(index + 1 < $store.getters.currentRound)">{{ $t("sidequest.question_locked") }}</span>
      </div>
      </div>
    </section>
    <section v-if="questionsPerRound.length < 4">
      <h2>{{ $t("sidequest.gameroundtitle", {round: questionsPerRound.length + 1}) }}</h2>
      <div class="round">
        <p>{{ $t("sidequest.noroundyet", {nextRound: questionsPerRound.length + 1}) }}</p>
      </div>
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";

export default {
  name: "SideQuest",
  components: { UserPageLayout, PointsAndStickers },
  data() {
    return {
      loading: false,
      resultParsed: null,
      isExplanationVisible: false,
      showExplanationText: false,
      showErrorPopup: false,
      popupErrorMessage: ""
    };
  },
  mounted() {
    this.$store.dispatch("checkNewQuestions", this.$store.state.matches);
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
      this.$store.dispatch("submitAnswers").then((x) => {
        console.log(x);
        if (x == "failed") {
          this.popupErrorMessage = this.$t("sidequest.submitfailed");
          this.showErrorPopup = true;
          setTimeout(() => {
            this.showErrorPopup = false;
            this.popupErrorMessage = null;
          }, 5000);
        }
      });
    },
    showExplanation() {
      this.isExplanationVisible = !this.isExplanationVisible;
    },
    questionClicked(questionId, round) {
      if(this.coins.length == 0)
      {
        this.popupErrorMessage = this.$t("sidequest.no_coins");
        this.showErrorPopup = true;
          setTimeout(() => {
            this.showErrorPopup = false;
            this.popupErrorMessage = null;
          }, 5000);
        return;
      }
      if(round === this.$store.getters.currentRound)
      {
        this.$router.push(`/sidequest/question/${questionId}`);
      }
    },
  },
  computed: {
    questionsPerRound() {
      return this.$store.state.qsOpened.filter(questions => questions.length > 0)  || [];
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

.introduction {
  color: #555;
  font-style: italic;
  line-height: 1.7;
  font-size: medium;
  white-space: break-spaces;
  margin: 1.5em -1em;
  padding: 0.1em 1em;
  background: #fff;
}
.introduction > span {
  display: block;
  max-height: 0;
  overflow: hidden;
  transition: max-height 0.3s ease-out, margin-bottom 0.5s ease-out;
}

.introduction > span.show {
  max-height: 1000px;
  transition: max-height 0.3s ease-in;
  margin-bottom:1em;
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
.error-popup {
  position: fixed;
  top: 4em;
  left:1em;
  right:1em;
  z-index:800;
  background-color: var(--red);
  color: #fff;
  padding:1.5em 1em;
  border-radius:1em;
  box-shadow: 2px 5px 5px 0px rgba(0,0,0,0.5);
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

.question {
  width:100%;
  margin:0 0 0 .5em;
  position:relative;
  }

.question-button {
  border-radius: 1em;
  padding: 1em;
  background-color: var(--darkgreen);
  display: flex;
  flex-direction: column;
  justify-content: center;
  width:100%;
  cursor: pointer;
}

.toggle-arrow {
  display: inline-block;
  width: 0;
  height: 0;
  margin-left: 5px;
  vertical-align: middle;
  border-top: 5px solid transparent;
  border-bottom: 5px solid transparent;
  border-left: 5px solid black;
  transform: rotate(0deg);
  transition: transform 0.2s ease-in-out;
}

.toggle-arrow.rotated {
  transform: rotate(90deg);
}

.round.locked .question-button {
  background-color: var(--red);
  filter: blur(2px);
  cursor: not-allowed;
}
.question-button img {
  height:5em;
}

.question:first-child {
  margin:0 .5em 0 0;
}

.question-button h3 {
  font-size: 1.4em;
  margin: 0;
  text-align: center;
  width:100%;
  color: #fff;
}

.question-button a {
  color: #fff;
  text-decoration: none;
}
.locked-indicator {
  position: absolute;
  filter:none;
  font-size: 1.5em;
  opacity: 0.7;
  top: 30%;
  rotate: -15deg;
  margin: 0;
  text-align: center;
  width:100%;
  color: #fff;
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
