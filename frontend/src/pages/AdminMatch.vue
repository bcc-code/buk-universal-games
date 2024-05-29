<template>
  <AdminPageLayout>
    <div class="flex">
      <nav class="text-dark mr-4">
        <div @click="$router.back()" class="p-1 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue">
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
      <h1 class="text-white font-bold py-2 px-2 rounded-md flex space-x-3">
        <img class="h-10 w-10" :src="`/icon/game-${game?.gameType.replace(/_/g, '')}.svg`" />
        <span class="text-label-1">{{ $t('games.' + game?.gameType) }}</span>
      </h1>
    </div>
    <section class="error-popup" v-if="showErrorPopup">
      <p>
        <span>{{ popupErrorMessage }}</span>
      </p>
    </section>

    <header>
      <h2 class="text-white">
        <span>Start: {{ match?.start }}</span>
      </h2>
    </header>

    <div class="teams">
      <div :class="{
        teamresult: true,
        selected: match?.team1Id === selectedTeam,
        winner: match?.team1Id === match?.winnerId,
        loser: match?.team2Id === match?.winnerId,
      }">
        <p>{{ match?.team1 }}</p>
        <p v-if="match?.team1Result">
          <strong>Score:</strong> {{ match?.team1Result }}
        </p>

        <button class="btn btn-blank" v-if="!isChangingScore1 && match?.team1Result > 0"
          @click="isChangingScore1 = true">
          Change
        </button>

        <div v-if="isChangingScore1 || !match?.team1Result">
          <input class="bg-slate-100" type="number" v-model="team1Result" :placeholder="'Input result'" />
          <button class="btn btn-blank" @click="confirmTeamResult(match?.team1Id, team1Result)">
            Confirm
          </button>
        </div>
      </div>
      <div v-if="match?.team1Id !== match?.team2Id" :class="{
        teamresult: true,
        selected: match?.team2Id === selectedTeam,
        winner: match?.team2Id === match?.winnerId,
        loser: match?.team1Id === match?.winnerId,
      }">
        <p>{{ match?.team2 }}</p>
        <p v-if="match?.team2Result">
          <strong>Score:</strong> {{ match?.team2Result }}
        </p>

        <button class="btn btn-blank" v-if="!isChangingScore2 && match?.team2Result > 0"
          @click="isChangingScore2 = true">
          Change
        </button>
        <div v-if="isChangingScore2 || !match?.team2Result">
          <input class="bg-slate-100" type="number" v-model="team2Result" :placeholder="'Input result'" />
          <button class="btn btn-blank" @click="confirmTeamResult(match?.team2Id, team2Result)">
            Confirm
          </button>
        </div>
      </div>
    </div>
  </AdminPageLayout>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, defineProps } from 'vue';
import { useRouter } from 'vue-router';
import { useStore } from 'vuex';
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';

const props = defineProps<{
  matchId: string;
}>();

const router = useRouter();
const store = useStore();

const team1Result = ref<number | null>(null);
const team2Result = ref<number | null>(null);
const isChangingScore1 = ref(false);
const isChangingScore2 = ref(false);
const selectedTeam = ref<string | null>(null);
const showErrorPopup = ref(false);
const popupErrorMessage = ref<string | null>(null);
const match = ref<any>(null);
const game = ref<any>(null);


const matches = computed(() => store.state.adminMatches);
const games = computed(() => store.state.games);

const loadMatch = () => {
  match.value = matches.value?.find((m: any) => m.matchId == props.matchId);
  game.value = games.value?.find((g: any) => g.id == match.value?.gameId);
};

const confirmTeamResult = async (teamId: string, result: number | null) => {
  if (
    typeof result === 'number' &&
    Number.isInteger(result) &&
    result >= 1 &&
    result <= 20
  ) {
    const payload = { matchId: match.value.matchId, teamId, result };
    const response = await store.dispatch('confirmTeamResult', payload);
    if (response === 'failed') {
      popupErrorMessage.value =
        'Submitting failed, please check connection and try again';
      showErrorPopup.value = true;
      setTimeout(() => {
        showErrorPopup.value = false;
        popupErrorMessage.value = null;
      }, 5000);
      return;
    }
    store.dispatch('getAdminLeagueStatus');
    if (teamId === match.value.team1Id) {
      match.value.team1Result = team1Result.value;
      isChangingScore1.value = false;
    } else {
      match.value.team2Result = team2Result.value;
      isChangingScore2.value = false;
    }
    loadMatch();
  } else {
    alert('Please enter a whole number between 1 and 20');
  }
};

onMounted(() => {
  if (!props.matchId) {
    router.back();
  } else {
    loadMatch();
  }
});

if (!store.state.games.length) {
  store.dispatch('getGames');
}
if (!store.state.adminMatches.length) {
  store.dispatch('getAdminMatches');
}
</script>

<style scoped>
.banner {
  width: 100%;
  padding: 2em;
  height: 20em;
  border-radius: 1em;
  margin: 2em auto;
  background-color: var(--dark);
  color: #fff;
  display: flex;
  justify-content: space-between;
  align-items: flex-end;
  background-size: 80%;
  background-position: center;
  background-repeat: no-repeat;
}

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

header h3 {
  display: flex;
  align-items: center;
  gap: 0.5em;
  margin: 0.5em 0;
}

.icon {
  max-width: 4em;
}

div.teams {
  display: grid;
  gap: 1em;

  margin: 0 0 2em 0;
  padding: 0;
}

div.teamresult {
  padding: 1em;
  background-color: #fff;
  border-radius: 1em;
  position: relative;
  display: grid;
  grid-template-rows: auto auto auto;
  box-shadow: 0 0.5em 1em -0.5em rgba(0, 0, 0, 0.1);
}

ul li.winner {
  /* color: #fff;
  background-color: var(--dark); */
}

ul li.selected {
  color: #fff;
  background-color: var(--dark);
  /* background-color: #fff; */
}

/* ul li.winner.selected {

} */

div.teamresult .tag {
  position: absolute;
  top: 1em;
  right: 1em;
  text-transform: uppercase;
  border-radius: 4px;
  padding: 0.25em 0.5em;
}

div.teamresult .tag {
  color: #000;
  background-color: var(--red);
}

div.teamresult.winner .tag {
  color: #000;
  background-color: var(--green);
}

.buttons {
  display: flex;
  flex-direction: column;
  gap: 1em;
}

.buttons button {
  width: 100%;
}

.btn-success {
  border: 2px solid hsl(158, 93%, 40%);
}

header h2 {
  display: flex;
  align-items: center;
  gap: 0.5em;
}

.error-popup {
  position: fixed;
  top: 4em;
  left: 1em;
  right: 1em;
  z-index: 800;
  background-color: var(--red);
  color: #fff;
  padding: 1.5em 1em;
  border-radius: 1em;
  box-shadow: 2px 5px 5px 0px rgba(0, 0, 0, 0.5);
}
</style>
