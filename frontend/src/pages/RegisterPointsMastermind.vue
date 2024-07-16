<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="mb-4">
        Klarte laget å gjette alle fargene (4 røde) riktig innen 10 minutter?
        <div class="flex space-x-4">
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="guessedAllColors === true ? 'btn-success' : ''"
            @click="guessedAllColors = true"
          >
            Ja
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="guessedAllColors === false ? 'btn-success' : ''"
            @click="guessedAllColors = false"
          >
            Nei
          </button>
        </div>
      </div>

      <div class="mb-4" v-if="guessedAllColors === true">
        Hvor mange runder brukte de?
        <div class="flex -m-2 flex-wrap">
          <button
            type="button"
            class="btn multiselect-button shadow-md m-2"
            v-for="turn in 8"
            :key="turn"
            :class="turnsTaken === turn ? 'btn-success' : ''"
            @click="turnsTaken = turn"
          >
            {{ turn }}
          </button>
        </div>
      </div>

      <div class="mb-4" v-if="guessedAllColors === false">
        Hvor mange riktige farger (røde og hvite pinner) hadde laget på sitt
        siste forsøk?
        <div class="flex -m-2 flex-wrap">
          <button
            type="button"
            class="btn multiselect-button shadow-md m-2"
            v-for="color in 5"
            :key="color"
            :class="correctColors === color - 1 ? 'btn-success' : ''"
            @click="correctColors = color - 1"
          >
            {{ color - 1 }}
          </button>
        </div>
      </div>

      <div class="mb-4" v-if="calculatedResult !== undefined">
        Beregnet score: {{ calculatedResult }}
      </div>

      <LoadingButton
        :is-loading="isPending"
        type="submit"
        class="btn-success btn-blank h-14 p-4"
        :disabled="calculatedResult === undefined || isPending"
      >
        Lagre
      </LoadingButton>
    </form>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="error">
    <div class="alert alert-error block">{{ error.message }}</div>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { floatToInt, lerp } from './mathHelpers';
import LoadingButton from '@/components/LoadingButton.vue';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const guessedAllColors = ref<boolean | undefined>(undefined);
const turnsTaken = ref<number | undefined>(undefined);
const correctColors = ref<number | undefined>(undefined);

const topScore = 20;
const bottomScore = 6;
const maxTurns = 8;

const calculatedResult = computed<number | undefined>(() => {
  if (guessedAllColors.value === true && turnsTaken.value !== undefined) {
    const turnPoints = lerp(
      1,
      maxTurns,
      bottomScore,
      topScore,
      turnsTaken.value,
    );

    return floatToInt(turnPoints);
  } else if (
    guessedAllColors.value === false &&
    correctColors.value !== undefined
  ) {
    return floatToInt(correctColors.value);
  }
  return undefined;
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const { mutate: confirmResult, isPending, error } = useConfirmTeamResult();

const submitForm = () => {
  if (calculatedResult.value === undefined)
    throw Error(
      'CalculatedResult returned undefined. The form is invalid even if it can be submitted. You might be missing some validation on the form fields.',
    );

  const matchId = props.match.matchId;
  const result = calculatedResult.value;
  const teamId = props.match.team1Id;

  const variables = { matchId: matchId, result: result, teamId: teamId };
  confirmResult(variables, {
    onSuccess() {
      showSuccessToast();
    },
  });
};
</script>

<style>
.multiselect-button {
  width: 0;
  padding-left: 2rem;
  padding-right: 2rem;
}
</style>
