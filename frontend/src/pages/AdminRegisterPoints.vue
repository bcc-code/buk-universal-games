<template>
  <div class="toast toast-center toast-top" v-if="showErrorPopup">
    <div class="alert alert-error block">{{ popupErrorMessage }}</div>
  </div>
  <div class="toast toast-center toast-top" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>

  
  <TimePicker :match="match"></TimePicker>
  
<!-- 🧹remove -->
  <!-- <div class="teams">
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
  -->
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import TimePicker from "./TimePicker.vue"

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const { mutate: confirmResult, isPending } = useConfirmTeamResult();

const isChangingScore1 = ref(false);
const isChangingScore2 = ref(false);
 const showSuccess = ref<boolean>(false);
 const popupErrorMessage = ref<string | null>(null);

 const confirmDisabled = computed(() => isPending.value);
 const showErrorPopup = computed(() => !!popupErrorMessage.value);
 const team1Result = ref(props.match.team1Result);
 const team2Result = ref(props.match.team2Result);

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
  if (!props.match || !teamId) {
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
      { matchId: props.match.matchId, teamId, result },
      {
        onSuccess: () => {
          if (teamId === props.match.team1Id) {
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

const showError = () => {
  popupErrorMessage.value =
    'Submitting failed, please check connection and try again';
  setTimeout(() => {
    popupErrorMessage.value = null;
  }, 5000);
};
</script>

<style scoped>
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
