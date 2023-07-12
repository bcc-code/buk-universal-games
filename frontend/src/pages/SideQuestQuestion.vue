<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.status?.points"
      :stickers="coins.length"
      :refresh="refresh"
    />
    <section v-if="step=='pre'">
      <div class="heading-text">
        <h2>Are you ready?</h2>
        </div>  
      <Timer :seconds="5" @timer-finished="startQuestion" />
    </section>
    <section v-if="step=='question'">
      <Timer :seconds="5" @timer-finished="questionFinished" />
      <div class="heading-text">
        <h2>{{ this.question }}</h2>
      </div>
      <img :src="require(`@/assets/img/${id}.jpg`)" v-if="hasImage" />
    </section>
    <section v-if="step=='answer'">
      <Timer :seconds="20" @timer-finished="answerFinished" />
      <div class="heading-text">
        <h2>{{ this.question }}</h2>
      </div>
      <component :is="answerComponent" :options="options" v-model="selectedAnswer" />
    </section>
    <section v-if="step=='done'">
      <div class="heading-text">
        <h2>{{ $t("sidequest.thanks") }}</h2>
      </div>
      <button class="btn-blank" @click="$router.back()">{{ $t("sidequest.back_to_overview") }}</button>
    </section>
    <section v-if="step=='timeranout'">
      <div class="heading-text">
        <h2>{{ $t("sidequest.timeranout") }}</h2>
      </div>
      <button class="btn-blank" @click="$router.back()">{{ $t("sidequest.back_to_overview") }}</button>
    </section>
  </UserPageLayout>
</template>

<script>

import { markRaw } from "vue";
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";
import MultipleChoiceSelector from "@/components/MultipleChoiceSelector.vue";
import SimpleGuessInput from "@/components/SimpleGuessInput.vue";
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
      step: 'pre',
    };
  },
  created() {
    if([1,2].includes(this.id)) {
      this.answerComponent = markRaw(SimpleGuessInput);
    }
    else {
      this.answerComponent = markRaw(MultipleChoiceSelector);
    }
  },
  mounted() {
    const q = this.$store.state.qs.find((q) => q.id == this.id);
    this.coin = this.coins.pop();
    this.question = this.$t("questions." + q.q + ".q");
    this.intro = this.$t("questions." + q.q + ".intro");
    this.hasImage = q.i;
    this.options = q.a.map((option) => ({
      label: this.$t("questions." + q.q + ".a." + option),
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
      if(this.selectedAnswer) {
        this.$store.commit("addAnswer", {questionId: this.id, answer: this.selectedAnswer, coin: this.coin});
        this.step = 'done';
      }
      else
      {
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
