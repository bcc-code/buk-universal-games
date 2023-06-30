<template>
  <UserPageLayout>
    <PointsAndStickers
      :loading="loading"
      :points="teamStatus?.status?.points"
      :stickers="this.coins"
      :refresh="refresh"
    />
    <section v-if="step==1">
      <div class="heading-text">
        <h2>Are you ready?</h2>
        </div>  
      <Timer :seconds="5" @timer-finished="nextStep" />
    </section>
    <section v-if="step==2">
      <div class="heading-text">
        <h2>What color shirt was Herman wearing?</h2>
        <component :is="questionComponent" />
        </div>  
      <Timer ref="questionTimer" :seconds="30" @timer-finished="questionFinished" />
    </section>
  </UserPageLayout>
</template>

<script>
import UserPageLayout from "@/components/UserPageLayout.vue";
import PointsAndStickers from "@/components/PointsAndStickers.vue";

import Timer from "@/components/CircularTimer.vue";

export default {
  name: "SideQuestQuestion",
  props: {
    question: Number,
  },
  components: { UserPageLayout, PointsAndStickers, Timer },
  data() {
    return {
      loading: true,
      resultParsed: null,
     questionComponent,
      step: 1,
    };
  },
  mounted() {
    
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
