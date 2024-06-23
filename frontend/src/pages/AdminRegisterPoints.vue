<template>
  <div class="toast toast-center toast-top" v-if="showErrorPopup">
    <div class="alert alert-error block">{{ popupErrorMessage }}</div>
  </div>
  <div class="toast toast-center toast-top" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>



  <RegisterPointsIronGrip       v-if=     "gameType==='iron_grip'"        :match="match"></RegisterPointsIronGrip>
  <RegisterPointsLandWaterBeach v-else-if="gameType==='land_water_beach'" :match="match"></RegisterPointsLandWaterBeach>
  <RegisterPointsLabyrinth v-else-if="gameType==='labyrinth'"        :match="match"></RegisterPointsLabyrinth>
  <div v-else>
    <div class="alert alert-error block">Noe gikk galt</div>
  </div>
    
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
// 🧹rename
import RegisterPointsIronGrip from "./RegisterPointsIronGrip.vue"
import type { GameType } from './GameType';
// 🧹rename
import RegisterPointsLandWaterBeach from './RegisterPointsLandWaterBeach.vue';
import RegisterPointsLabyrinth from './RegisterPointsLabyrinth.vue';


const props = defineProps<{
  match: MatchListItemEntity;
  gameType: GameType;
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
