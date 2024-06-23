<template>
  <div class="p-4 bg-white rounded-md">
    <form @submit.prevent="submitForm">
      <div class="flex justify-evenly gap-4">
        <div >
          <div class="font-bold">
            {{ match.team1 }} 
          </div>
          <br/>
          Tid:
          <br/>
          <TimePicker v-model="team1Time" placeholder="Tid"></TimePicker>
          <br/>
          <br/>
          Antall steg utenfor:
          <br/>
          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="team1Steps"
            placeholder="Steg utenfor"
            required
            min="0"
          />
          <br/>
          <br/>

          <div class="mb-4" v-if="calculatedResult">
            Beregnet score: {{calculatedResult.team1Score}}
          </div>
        </div>

        <div >
          <div class="font-bold">
            {{ match.team2 }} 
          </div>
          <br/>
          Tid:
          <br/>
          <TimePicker v-model="team2Time" placeholder="Tid"></TimePicker>
          <br/>
          <br/>
          Antall steg utenfor:
          <br/>
          <input
            class="shadow-md border-solid border-2 border-slate-200 rounded-md w-36"
            type="number"
            v-model="team2Steps"
            placeholder="Steg utenfor"
            required
            min="0"
          />
          <br/>
          <br/>
          <div class="mb-4" v-if="calculatedResult">
            Beregnet score: {{calculatedResult.team2Score}}
          </div>
        </div>
      </div>

      <button type="submit" class="btn btn-success btn-blank h-14 p-4 shadow-md" :disabled="isPending">
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
import TimePicker from './TimePicker.vue';
import type { TimeType } from './TimeType';

const props = defineProps<{
  match: MatchListItemEntity;
}>();

const team1Time = ref<TimeType>();
const team1Steps = ref<number | ''>('');
const team2Time = ref<TimeType>();
const team2Steps = ref<number | ''>('');

const minScore = 2;
const maxScore = 15;
const minTime = 1 / (24 * 60); // Convert 1 minute to fraction of a day
const maxTime = 4 / (24 * 60); // Convert 4 minutes to fraction of a day
const winBonus = 5;
const penaltyPerStep = 0.2;
const minStepPenalties = 0;
const maxStepPenalties = 10;

const calculateScore = (time: NonNullable<TimeType>, steps: number):number => {
  const totalHengeTid = (time.hours * 60 + time.minutes + time.seconds / 60) / (24 * 60);
  const clampedTime = Math.max(minTime, Math.min(totalHengeTid, maxTime));
  const timePoints = maxScore - ((clampedTime - minTime) * (maxScore - minScore)) / (maxTime - minTime);
  const clampedSteps = Math.max(minStepPenalties, Math.min(steps, maxStepPenalties));
  const penaltyPoints = clampedSteps * penaltyPerStep;
  return timePoints - penaltyPoints;
};

const calculatedResult = computed<{
  team1Score: number;
  team2Score: number;
} | undefined>(() => {

  const team1StepsLocal = team1Steps.value;
  const team2StepsLocal = team2Steps.value;
  if(!team1Time.value) return;
  if(!team2Time.value) return;
  if(typeof team1StepsLocal !== 'number') return;
  if(typeof team2StepsLocal !== 'number') return;
  console.log(team1Time.value, team2Time.value, team1Steps.value, team2Steps.value)

  const team1Score = calculateScore(team1Time.value, team1StepsLocal);
  const team2Score = calculateScore(team2Time.value, team2StepsLocal);

  const winnerBonus = team1Score > team2Score ? [winBonus, 0] : team2Score > team1Score ? [0, winBonus] : [0, 0];

  return {
    team1Score: Math.round(team1Score + winnerBonus[0]),
    team2Score: Math.round(team2Score + winnerBonus[1]),
  };
});

const showSuccess = ref<boolean>(false);

const showSuccessToast = () => {
  showSuccess.value = true;
  setTimeout(() => {
    showSuccess.value = false;
  }, 3000);
};

const { mutate: confirmResultTeam1, isPending: isPendingTeam1, error: errorTeam1 } = useConfirmTeamResult();
const { mutate: confirmResultTeam2, isPending: isPendingTeam2, error: errorTeam2 } = useConfirmTeamResult();

const error = computed(() => errorTeam1.value || errorTeam2.value);
const isPending = computed(() => isPendingTeam1.value || isPendingTeam2.value);

const submitForm = () => {
  if (!calculatedResult.value) throw Error('CalculatedResult returned undefined. The form is invalid even if it can be submitted. You might be missing some validation on the form fields.');

  const matchId = props.match.matchId;

  const resultTeam1 = calculatedResult.value.team1Score;
  const resultTeam2 = calculatedResult.value.team2Score;

  const team1Id = props.match.team1Id;
  const team2Id = props.match.team2Id;

  const variablesTeam1 = { matchId: matchId, result: resultTeam1, teamId: team1Id };
  const variablesTeam2 = { matchId: matchId, result: resultTeam2, teamId: team2Id };

  confirmResultTeam1(variablesTeam1, {
    onSuccess() {
      confirmResultTeam2(variablesTeam2, {
        onSuccess() {
          showSuccessToast();
        },
      });
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
