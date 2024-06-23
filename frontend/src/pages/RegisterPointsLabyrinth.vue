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
        <div class="w-64 inline-block shadow-md">
          <VueDatePicker
            v-model="date"
            required
            auto-apply
            enable-seconds
            time-picker
            no-hours-overlay
            :min-time="{ hours: 0, minutes: 0, seconds: 1 }"
            :max-time="{ hours: 0, minutes: 10, seconds: 0 }"
            :start-time="{ hours: 0, minutes: 0, seconds: 1 }"
            minutes-grid-increment="1"
            format="mm:ss"
            :clearable="false"
            :ui="{ input: 'h-14 inline' }"
            placeholder="Tid"
          ></VueDatePicker>
        </div>
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
import { ref, computed, effect } from 'vue';
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import z from 'zod';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const date = ref<unknown>();
const finished = ref<boolean | undefined>(undefined);
const checkpoints = ref<0 | 1 | 2 | 3 | undefined>(undefined);

const timeSchema = z
  .object({
    hours: z.number(),
    minutes: z.number(),
    seconds: z.number(),
  })
  .optional();

const validatedDate = computed(() => {
  return timeSchema.parse(date.value);
});

const minScore = 0;
const maxScore = 10;
const minTime = 10 / (24 * 60); // Convert 10 minutes to fraction of a day
const maxTime = 2 / (24 * 60); // Convert 2 minutes to fraction of a day
const completionBonus = 4;
const pointsPerCheckpoint = 2;
const totalCheckpoints = 3;

const calculatedResult = computed<number | undefined>(() => {
  if (finished.value === undefined) return undefined;
  if (finished.value === false && checkpoints.value === undefined)
    return undefined;
  if (finished.value === true && !validatedDate.value) return undefined;

  
  const completedCheckpoints = finished.value
  ? totalCheckpoints
  : checkpoints.value ?? 0;
  const completionBonusValue = finished.value ? completionBonus : 0;
  
  
  let effectiveTime = minTime;
  if (finished.value && validatedDate.value) {
    effectiveTime = Math.max(maxTime,Math.min(

      (validatedDate.value.hours * 60 +
        validatedDate.value.minutes +
        validatedDate.value.seconds / 60) /
      (24 * 60)
    ,minTime));
  }

  const timePoints =
    maxScore +
    ((maxTime - effectiveTime) * (minScore - maxScore)) / (maxTime - minTime);
  const totalScore =
    completedCheckpoints * pointsPerCheckpoint +
    completionBonusValue +
    timePoints;

  console.log({effectiveTime:effectiveTime*24*60, timePoints, totalScore, completedCheckpoints, completionBonusValue})

  return Math.round(totalScore);
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
/* .btn-secondary {
  background-color: #ccc;
  color: #000;
}
.btn-success {
  background-color: #28a745;
  color: #fff;
}
.btn-danger {
  background-color: #dc3545;
  color: #fff;
}
 */
.multiselect-button {
  /* flex:0 0 1; */
  width: 0;
  padding-left: 2rem;
  padding-right: 2rem;
}
</style>
<style>
.dp__theme_light {
    --dp-disabled-color-text: #00000000;
}
</style>