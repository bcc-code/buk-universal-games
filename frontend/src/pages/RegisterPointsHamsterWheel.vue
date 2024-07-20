<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="justify-evenly border-t-1 ">
        <div>
          <div class="font-bold">
            {{ match.team1 }}
          </div>
          <br />
          Tid:
          <br />
          <TimePicker v-model="team1Time" placeholder="Tid"></TimePicker>
          <br />
          <br />
          Antall steg utenfor:
          <br />
          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="team1Steps"
            placeholder="Steg utenfor"
            required
            min="0"
          />
          <br />
          <br />

          <div class="mb-4" v-if="calculatedResult">
            Beregnet score: {{ calculatedResult.team1Score }}
          </div>
        </div>

        <div>
          <div class="font-bold">
            {{ match.team2 }}
          </div>
          <br />
          Tid:
          <br />
          <TimePicker v-model="team2Time" placeholder="Tid"></TimePicker>
          <br />
          <br />
          Antall steg utenfor:
          <br />
          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="team2Steps"
            placeholder="Steg utenfor"
            required
            min="0"
          />
          <br />
          <br />
          <div class="mb-4" v-if="calculatedResult">
            Beregnet score: {{ calculatedResult.team2Score }}
          </div>
        </div>
      </div>

      <LoadingButton
        type="submit"
        class="btn-success btn-blank h-14 p-4"
        :is-loading="isPending"
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
import '@vuepic/vue-datepicker/dist/main.css';
import { useConfirmBothTeamResults } from '@/hooks/hooks';
import type { MatchListItemEntity } from './MatchListItemEntity';
import TimePicker from './TimePicker.vue';
import { timeToNumber, type TimeType } from './TimeType';
import { clamp, floatToInt, lerp } from './mathHelpers';
import LoadingButton from '@/components/LoadingButton.vue';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const team1Time = ref<TimeType>();
const team1Steps = ref<number | ''>('');
const team2Time = ref<TimeType>();
const team2Steps = ref<number | ''>('');

const minScore = 2;
const maxScore = 15;
const minTime = timeToNumber({ hours: 0, minutes: 1, seconds: 0 });
const maxTime = timeToNumber({ hours: 0, minutes: 4, seconds: 0 });
const winBonus = 5;
const penaltyPerStep = 0.2;
const minStepPenalties = 0;
const maxStepPenalties = 10;

const calculateScore = (time: NonNullable<TimeType>, steps: number): number => {
  const totalHengeTid = timeToNumber(time);
  // Note: min and max are switched to invert scale
  const timePoints = lerp(
    minTime,
    maxTime,
    maxScore,
    minScore,
    totalHengeTid,
    'clamp',
  );
  const clampedSteps = clamp(minStepPenalties, steps, maxStepPenalties);
  const penaltyPoints = clampedSteps * penaltyPerStep;
  return timePoints - penaltyPoints;
};

const calculatedResult = computed<
  | {
      team1Score: number;
      team2Score: number;
    }
  | undefined
>(() => {
  const team1StepsLocal = team1Steps.value;
  const team2StepsLocal = team2Steps.value;
  if (!team1Time.value) return;
  if (!team2Time.value) return;
  if (typeof team1StepsLocal !== 'number') return;
  if (typeof team2StepsLocal !== 'number') return;

  const team1Score = calculateScore(team1Time.value, team1StepsLocal);
  const team2Score = calculateScore(team2Time.value, team2StepsLocal);

  const winnerBonus =
    team1Score > team2Score
      ? [winBonus, 0]
      : team2Score > team1Score
        ? [0, winBonus]
        : [0, 0];

  return {
    team1Score: floatToInt(team1Score + winnerBonus[0]),
    team2Score: floatToInt(team2Score + winnerBonus[1]),
  };
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const {
  mutate: confirmBothTeamResults,
  isPending,
  error,
} = useConfirmBothTeamResults();

const submitForm = () => {
  if (!calculatedResult.value)
    throw Error(
      'CalculatedResult returned undefined. The form is invalid even if it can be submitted. You might be missing some validation on the form fields.',
    );

  const matchId = props.match.matchId;
  const resultTeam1 = calculatedResult.value.team1Score;
  const resultTeam2 = calculatedResult.value.team2Score;

  const variables = {
    matchId: matchId,
    team1Result: resultTeam1,
    team2Result: resultTeam2,
  };

  confirmBothTeamResults(variables, {
    onSuccess() {
      showSuccessToast();
    },
  });
};
</script>

<style>
.btn-secondary {
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
</style>
