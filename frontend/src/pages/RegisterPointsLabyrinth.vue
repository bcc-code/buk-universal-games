<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="mb-4">
        <div class="block">
          Klarte laget å få ballen gjennom hele labyrinten på under 10 minutter?
        </div>

        <div class="flex space-x-4">
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="finished === true ? 'btn-success' : ''"
            @click="finished = true"
          >
            Ja
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="finished === false ? 'btn-success' : ''"
            @click="finished = false"
          >
            Nei
          </button>
        </div>
      </div>

      <div v-if="finished === false" class="mb-4">
        <label>Hvor mange checkpoints klarte de å nå?</label>
        <div class="flex space-x-4">
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints === 0 ? 'btn-success' : ''"
            @click="checkpoints = 0"
          >
            0
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints === 1 ? 'btn-success' : ''"
            @click="checkpoints = 1"
          >
            1
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints === 2 ? 'btn-success' : ''"
            @click="checkpoints = 2"
          >
            2
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints === 3 ? 'btn-success' : ''"
            @click="checkpoints = 3"
          >
            3
          </button>
        </div>
      </div>

      <div v-if="finished === true" class="mb-4">
        Tid:
        <TimePicker v-model="date" placeholder="Tid"></TimePicker>
      </div>

      <div class="mb-4" v-if="typeof calculatedResult === 'number'">
        Beregnet score: {{ calculatedResult }}
      </div>

      <button
        type="submit"
        class="btn btn-success btn-blank h-14 p-4 shadow-md"
        :disabled="isPending || !isValid"
      >
        Lagre
      </button>
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
import '@vuepic/vue-datepicker/dist/main.css';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { timeToNumber, type TimeType } from './TimeType';
import TimePicker from './TimePicker.vue';
import { clamp, floatToInt, lerp } from './mathHelpers';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const date = ref<TimeType>();
const finished = ref<boolean | undefined>(undefined);
const checkpoints = ref<0 | 1 | 2 | 3 | undefined>(undefined);

const minScore = 0;
const maxScore = 10;
const minTime = 10 / (24 * 60); // Convert 10 minutes to fraction of a day
const maxTime = 2 / (24 * 60); // Convert 2 minutes to fraction of a day
const completionBonus = 4;
const pointsPerCheckpoint = 2;
const totalCheckpoints = 3;

const calculatedResult = computed<number | undefined>(() => {
  if (finished.value === undefined) return undefined;
  if (finished.value === false && checkpoints.value === undefined) return undefined;
  if (finished.value === true && !date.value) return undefined;

  const completedCheckpoints = finished.value ? totalCheckpoints : checkpoints.value ?? 0;
  const completionBonusValue = finished.value ? completionBonus : 0;

  let effectiveTime = minTime;
  if (finished.value && date.value) {
    effectiveTime = clamp(maxTime, timeToNumber(date.value), minTime);
  }

  const timePoints = lerp(maxTime, minTime, minScore, maxScore, effectiveTime);
  const totalScore = completedCheckpoints * pointsPerCheckpoint + completionBonusValue + timePoints;
  return floatToInt(totalScore);
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const { mutate: confirmResult, isPending, error } = useConfirmTeamResult();

const isValid = computed(
  () =>
    finished.value !== undefined &&
    (finished.value === true || checkpoints.value !== undefined),
);

const submitForm = () => {
  if (!calculatedResult.value)
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

<style scoped>
.multiselect-button {
  width: 0;
  padding-left: 2rem;
  padding-right: 2rem;
}
</style>
