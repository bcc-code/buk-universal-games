<template>
  <UserPageLayout>
    <PointsAndStickers :loading="loading" :points="teamStatus?.points" :stickers="coins.length"
      :refresh="refresh" />
    <section v-if="step == 'pre'">
      <div class="heading-text">
        <h2>{{ $t("sidequest.areyouready") }}</h2>
      </div>
      <Timer class="timer-center" :seconds="3" @timer-finished="startQuestion" />
    </section>
    <section v-if="step == 'question'">
      <div class="timer-top-right">
        <Timer :seconds="questionTime" @timer-finished="questionFinished" />
      </div>
      <div class="heading-text">
        <h2>{{ this.intro }}</h2>
      </div>
      <img :src="`image/${id}.jpg`" v-if="hasImage" />
    </section>
    <section v-if="step == 'answer'">
      <div class="timer-top-right">
        <Timer :seconds="20" @timer-finished="answerFinished" />
      </div>
      <div class="heading-text">
        <h2>{{ this.question }}</h2>
      </div>
      <component :is="answerComponent" :options="options" v-model="selectedAnswer" />
      <div class="next-button">
        <button class="btn-blank btn-continue-answer" @click="answerFinished">&#x2794;</button>
      </div>
    </section>
    <section v-if="['timeranout', 'done', 'alreadyAnswered'].includes(step)" class="final-screen">
      <div class="heading-text">
        <h2>{{ $t(doneMessage[step]) }}</h2>
      </div>
      <button class="btn-blank btn-return" @click="$router.back()">{{ $t("sidequest.back_to_overview") }}</button>
    </section>
  </UserPageLayout>
</template>

<script>

import { markRaw } from "vue";
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";
import MultipleChoiceSelector from "@/components/MultipleChoiceSelector.vue";
import Timer from "@/components/CircularTimer.vue";

export default {
  name: "SideQuestQuestion",
  components: { UserPageLayout, PointsAndStickers, Timer },
  props: {
    id: {
      type: Number
    },
  },
  data() {
    return {
      loading: false,
      selectedAnswer: null,
      answerComponent: null,
      question: null,
      coin: null,
      image: null,
      hasImage: false,
      intro: null,
      options: [],
      questionTime: 10,
      step: 'pre',
      doneMessage: {
        done: "sidequest.thanks",
        alreadyAnswered: "sidequest.alreadyAnswered",
        timeranout: "sidequest.timeranout",
      }
    };
  },
  created() {
    this.answerComponent = markRaw(MultipleChoiceSelector);
  },
  mounted() {
    const q = this.$store.state.qs.find((q) => q.id == this.id);
    if (!this.$store.state.qsOpened[this.$store.getters.currentRound - 1].some((q) => q.id == this.id)) {
      this.$router.back();
      return;
    }
    if (this.$store.state.answers.some((a) => a.questionId == this.id) || this.$store.state.submittedAnswers.some((a) => a.questionId == this.id)) {
      this.step = 'alreadyAnswered';
      return;
    }
    if(this.coins.length == 0)
    {
      this.$router.back();
      return;
    }
    this.coin = this.coins.slice(-1)[0];
    this.$store.commit("removeCoin", this.coin);
    this.question = this.$t("questions." + q.q + ".q");
    this.intro = this.$t("questions." + q.q + ".intro");
    this.hasImage = q.i;
    switch (q.id) {
      case 7:
        this.questionTime = 45;
        break;
      case 4:
        this.questionTime = 20;
        break;
      default:
        this.questionTime = 10;
    }

    const shuffled = q.a.slice();
    for (let i = shuffled.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [shuffled[i], shuffled[j]] = [shuffled[j], shuffled[i]];
    }

    this.options = shuffled.map((option) => ({
      label: [1,2].includes(q.id) ? this.$n(parseInt(option)) : this.$t("questions." + q.q + ".a." + option),
      value: option,
    }));
  },
  methods: {
    refresh() {
      this.loading = true;
      this.$store.dispatch("getLeagueStatus", true);

      setTimeout(() => {
        this.loading = false;
      }, 1000);
    },
    startQuestion() {
      this.step = "question";
    },
    questionFinished() {
      this.step = "answer";
    },
    answerFinished() {
      if(this.step != "answer") return;

      if (this.selectedAnswer) {
        this.$store.commit("addAnswer", { questionId: this.id, answer: this.selectedAnswer, coin: this.coin });
        this.step = 'done';
      }
      else {
        this.step = 'timeranout';
      }
    },
  },
  computed: {
    coins() {
      return this.$store.state.coins;
    },
    qs() {
      return this.$store.state.qs;
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
  margin-top: 20%
}

.circle {
  margin-left: -50px;
}

.timer-top-right {
  width: 100%;
  display: flex;
  justify-content: flex-end;
  padding-right: 30px;
  margin-top: 30px;
}

.timer-center {
  position: absolute;
  top: 50%;
  left: 50%;
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

.heading-icon>span {
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

.final-screen {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.next-button {
  margin-top:1em;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
