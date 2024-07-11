<template>
  <form @submit.prevent="submitForm">
    <div class="p-4 bg-white rounded-md mb-4">
      <div class="block">Er laget så stort at de bruker 2 labyrinter?</div>
      <div class="flex space-x-4">
        <button
          type="button"
          class="btn multiselect-button shadow-md"
          :class="labyrinthCount === 2 ? 'btn-success' : ''"
          @click="labyrinthCount = 2"
        >
          Ja
        </button>
        <button
          type="button"
          class="btn multiselect-button shadow-md"
          :class="labyrinthCount === 1 ? 'btn-success' : ''"
          @click="labyrinthCount = 1"
        >
          Nei
        </button>
      </div>
    </div>

    <div
      v-for="index in labyrinthCount"
      :key="index"
      class="p-4 bg-white rounded-md mb-4"
    >
      <div class="mb-4">
        <div class="block">
          Klarte laget å få ballen gjennom hele labyrinten {{ index }} på under
          10 minutter?
        </div>

        <div class="flex space-x-4">
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="finished[index - 1] === true ? 'btn-success' : ''"
            @click="finished[index - 1] = true"
          >
            Ja
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="finished[index - 1] === false ? 'btn-success' : ''"
            @click="finished[index - 1] = false"
          >
            Nei
          </button>
        </div>
      </div>

      <div v-if="finished[index - 1] === false" class="mb-4">
        <label>Hvor mange checkpoints klarte de å nå?</label>
        <div class="flex space-x-4">
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints[index - 1] === 0 ? 'btn-success' : ''"
            @click="checkpoints[index - 1] = 0"
          >
            0
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints[index - 1] === 1 ? 'btn-success' : ''"
            @click="checkpoints[index - 1] = 1"
          >
            1
          </button>
          <button
            type="button"
            class="btn multiselect-button shadow-md"
            :class="checkpoints[index - 1] === 2 ? 'btn-success' : ''"
            @click="checkpoints[index - 1] = 2"
          >
            2
          </button>
        </div>
      </div>

      <div v-if="finished[index - 1] === true" class="mb-4">
        Tid:
        <TimePicker v-model="dates[index - 1]" placeholder="Tid"></TimePicker>
      </div>
    </div>

    <div class="p-4 bg-white rounded-md mb-4">
      <div class="mb-4" v-if="typeof calculatedResult() === 'number'">
        Beregnet score: {{ calculatedResult() }}
      </div>

      <button
        type="submit"
        class="btn btn-success btn-blank h-14 p-4 shadow-md"
        :disabled="isPending || !isValid"
      >
        Lagre
      </button>
    </div>
  </form>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="error">
    <div class="alert alert-error block">{{ error.message }}</div>
  </div>
  <div class="toast toast-center toast-bottom pb-24 z-20" v-if="showSuccess">
    <div class="alert alert-success block">Lagret</div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import '@vuepic/vue-datepicker/dist/main.css';
import { useConfirmTeamResult } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import { timeToNumber, type TimeType } from './TimeType';
import TimePicker from './TimePicker.vue';
import { clamp, floatToInt, lerp } from './mathHelpers';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const labyrinthCount = ref<number>(1);
const finished = ref<(boolean | undefined)[]>([undefined, undefined]);
const checkpoints = ref<(0 | 1 | 2 | 3 | undefined)[]>([undefined, undefined]);
const dates = ref<(TimeType | undefined)[]>([undefined, undefined]);

const minScore = 0;
const maxScore = 10;
const minTime = timeToNumber({ hours: 0, minutes: 10, seconds: 0 });
const maxTime = timeToNumber({ hours: 0, minutes: 2, seconds: 0 });
const completionBonus = 4;
const pointsPerCheckpoint = 3;
const totalCheckpoints = 2;

const calculatedResult = () => {
  const scores: number[] = [];

  for (let i = 0; i < labyrinthCount.value; i++) {
    if (finished.value[i] === undefined) return undefined;

    if (finished.value[i] === false && checkpoints.value[i] === undefined)
      return undefined;

    if (finished.value[i] === true && !dates.value[i]) return undefined;

    const completedCheckpoints = finished.value[i]
      ? totalCheckpoints
      : checkpoints.value[i] ?? 0;

    const completionBonusValue = finished.value[i] ? completionBonus : 0;

    let effectiveTime = minTime;

    if (finished.value[i] && dates.value[i]) {
      effectiveTime = clamp(maxTime, timeToNumber(dates.value[i]!), minTime);
    }

    const timePoints = lerp(
      maxTime,
      minTime,
      maxScore,
      minScore,
      effectiveTime,
    );

    const totalScore =
      completedCheckpoints * pointsPerCheckpoint +
      completionBonusValue +
      timePoints;

    scores.push(floatToInt(totalScore));
  }

  const averageScore = scores.reduce((a, b) => a + b, 0) / scores.length;

  return floatToInt(averageScore);
};

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const { mutate: confirmResult, isPending, error } = useConfirmTeamResult();

const isValid = () =>
  finished.value.every((f) => f !== undefined) &&
  (finished.value.includes(true) ||
    checkpoints.value.every((c) => c !== undefined));

const submitForm = () => {
  const result = calculatedResult();
  if (!result)
    throw Error(
      'CalculatedResult returned undefined. The form is invalid even if it can be submitted. You might be missing some validation on the form fields.',
    );

  const matchId = props.match.matchId;

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
