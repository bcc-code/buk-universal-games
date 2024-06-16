<template>
  <AdminPageLayout>
    <div class="flex">
      <nav class="text-dark mr-4 mb-5 bg-ice-blue cursor-pointer rounded-md hover:bg-vanilla text-dark-blue btn"
        @click="$router.back()">
        <div class=" ">
          <ArrowLeftIcon class="h-5" />
        </div>
      </nav>
    </div>
    <div class="toast toast-center toast-top" v-if="showErrorPopup">
      <div class="alert alert-error block">{{ popupErrorMessage }}</div>
    </div>
    <div class="toast toast-center toast-top" v-if="showSuccess">
      <div class="alert alert-success block">Lagret</div>
    </div>

    <MatchListItem class="mb-5" v-if="match && match.team1 && match.team2" :key="match.matchId" :team1="match.team1"
      :team2="match.team2" :team1result="match.team1Result ?? null" :team2result="match.team2Result ?? null"
      :start="match.start ?? ''" :winner="match.winner" :gameType="game?.gameType ?? ''" addOn="" gameAddOn="" />

    <div class="teams">
      <div :class="{
        teamresult: true,
      }">
        <p>{{ match?.team1 }}</p>
        <p v-if="match?.team1Result">
          <strong>Score:</strong> {{ match?.team1Result }}
        </p>
        <div>
          <input class="bg-slate-100" type="number" v-model="team1Result" :placeholder="'Input result'" />
          <button class="btn btn-success btn-blank ml-3" @click="confirmTeamResult(match?.team1Id, team1Result)"
            :disabled="confirmDisabled">
            Save
          </button>
        </div>
      </div>
      <div v-if="match?.team1Id !== match?.team2Id" :class="{
        teamresult: true,
      }">
        <p>{{ match?.team2 }}</p>
        <p v-if="match?.team2Result">
          <strong>Score:</strong> {{ match?.team2Result }}
        </p>

        <div>
          <input class="bg-slate-100" type="number" v-model="team2Result" :placeholder="'Input result'" />
          <button class="btn btn-success btn-blank ml-3" @click="confirmTeamResult(match?.team2Id, team2Result)"
            :disabled="confirmDisabled">
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
nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
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
</style>
