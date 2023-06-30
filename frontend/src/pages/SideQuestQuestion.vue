<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.status?.points"
      :stickers="coins"
      :refresh="refresh"
    />
    <section v-if="step==1">
      <div class="heading-text">
        <h2>Are you ready?</h2>
        </div>  
      <Timer :seconds="3" @timer-finished="nextStep" />
    </section>
    <section v-if="step==2">
      <Timer ref="questionTimer" :seconds="15" @timer-finished="questionFinished" />
      <div class="heading-text">
        <h2>{{ this.question }}</h2>
        </div>
        <component :is="questionComponent" :options="answers" :value="selectedAnswer" />
    </section>
    <section v-if="step==3">
      <div class="heading-text">
        <h2>{{ $t("sidequest.thanks") }}</h2>
        </div>
        <component :is="questionComponent" :options="answers" :value="selectedAnswer" />
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
  data() {
    return {
      loading: false,
      questionComponent: markRaw(MultipleChoiceSelector),
      selectedAnswer: null,
      question: null,
      answers: [],
      step: 1,
    };
  },
  mounted() {
    const q = this.$store.state.qs[0];
    this.question = this.$t("questions." + q.q + ".q");
    this.answers = q.a.map((option) => ({
      label: this.$t("questions." + q.q + ".a." + option),
      value: option,
    }));
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
    nextStep() {
      this.step = this.step + 1;
      console.log("next step")
    },
    questionFinished() {
      this.step = this.step + 1;
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
  margin-top:20%
}

.circle {
  margin-left:-50px
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
