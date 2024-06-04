<template>
  <AdminPageLayout>
    <div class="flex">
      <nav class="text-dark mr-4">
        <div
          @click="$router.back()"
          class="p-1 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue"
        >
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
      <h1 class="text-white font-bold py-2 px-2 rounded-md flex space-x-3">
        <img
          class="h-10 w-10"
          :src="`/icon/game-${game?.gameType?.replace(/_/g, '')}.svg`"
        />
        <span class="text-label-1">{{ $t('games.' + game?.gameType) }}</span>
      </h1>
    </div>
    <div class="toast toast-center toast-top" v-if="showErrorPopup">
      <div class="alert alert-error block">{{ popupErrorMessage }}</div>
    </div>
    <div class="toast toast-center toast-top" v-if="showSuccess">
      <div class="alert alert-success block">Lagret</div>
    </div>

    <MatchListItem
      class="mb-5"
      v-if="match && match.team1 && match.team2"
      :key="match.matchId"
      :team1="match.team1"
      :team2="match.team2"
      :team1result="match.team1Result ?? null"
      :team2result="match.team2Result ?? null"
      :start="match.start ?? ''"
      :winner="match.winner"
      :gameType="game?.gameType ?? ''"
      addOn=""
      gameAddOn=""
    />
    <button @click="showError">show Error</button>

    <div class="teams">
      <div
        :class="{
          teamresult: true,
        }"
      >
        <p>{{ match?.team1 }}</p>
        <p v-if="match?.team1Result">
          <strong>Score:</strong> {{ match?.team1Result }}
        </p>
        <div>
          <input
            class="bg-slate-100"
            type="number"
            v-model="team1Result"
            :placeholder="'Input result'"
          />
          <button
            class="btn btn-success btn-blank ml-3"
            @click="confirmTeamResult(match?.team1Id, team1Result)"
            :disabled="confirmDisabled"
          >
            Save
          </button>
        </div>
      </div>
      <div
        v-if="match?.team1Id !== match?.team2Id"
        :class="{
          teamresult: true,
        }"
      >
        <p>{{ match?.team2 }}</p>
        <p v-if="match?.team2Result">
          <strong>Score:</strong> {{ match?.team2Result }}
        </p>

        <div>
          <input
            class="bg-slate-100"
            type="number"
            v-model="team2Result"
            :placeholder="'Input result'"
          />
          <button
            class="btn btn-success btn-blank ml-3"
            @click="confirmTeamResult(match?.team2Id, team2Result)"
            :disabled="confirmDisabled"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  </AdminPageLayout>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useRouter, useRoute } from 'vue-router';
import { useAdminMatches, useGames, useConfirmTeamResult } from '@/hooks/hooks';
import AdminPageLayout from '@/components/AdminPageLayout.vue';
import { ArrowLeftIcon } from '@heroicons/vue/24/solid';
import { useStore } from 'vuex';
import MatchListItem from '@/components/MatchListItem.vue';

const route = useRoute();
const router = useRouter();
const matchId = Number(route.params.matchId as string);
const leagueId = useStore().state.adminLeagueSelected;

const { data: matches } = useAdminMatches(leagueId);
const { data: games } = useGames();
const { mutate: confirmResult, isPending } = useConfirmTeamResult();

const isChangingScore1 = ref(false);
const isChangingScore2 = ref(false);
const popupErrorMessage = ref<string | null>(null);
const showErrorPopup = computed(() => !!popupErrorMessage.value);

const match = computed(() =>
  matches.value?.find((m: any) => m.matchId == matchId),
);
const game = computed(() =>
  games.value?.find((g: any) => g.id == match.value?.gameId),
);
const team1Result = ref<number | null>(match.value?.team1Result ?? null);
const team2Result = ref<number | null>(match.value?.team2Result ?? null);

const confirmDisabled = computed(() => isPending.value);

const showSuccess = ref<boolean>(false);

const showError = () => {
  popupErrorMessage.value =
    'Submitting failed, please check connection and try again';
  setTimeout(() => {
    popupErrorMessage.value = null;
  }, 5000);
};

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const confirmTeamResult = async (
  teamId: number | undefined,
  result: number | null,
) => {
  if (!match.value || !teamId) {
    alert('noe gikk galt');
    return;
  }
  if (
    typeof result === 'number' &&
    Number.isInteger(result) &&
    result >= 1 &&
    result <= 20
  ) {
    confirmResult(
      { matchId, teamId, result },
      {
        onSuccess: () => {
          if (teamId === match.value?.team1Id) {
            isChangingScore1.value = false;
          } else {
            isChangingScore2.value = false;
          }
          showSuccessToast();
        },
        onError: () => {
          showError();
        },
      },
    );
  } else {
    alert('Please enter a whole number between 1 and 20');
  }
};

if (!matchId) {
  router.back();
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

/* .btn-success {
  border: 2px solid hsl(158, 93%, 40%);
} */

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

button[disabled] {
  background-color: #e0e0e0;
  color: #a0a0a0;
  cursor: not-allowed;
}
</style>
